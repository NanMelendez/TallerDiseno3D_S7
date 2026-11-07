using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData data;

    [SerializeField] private Transform firePoint;

    private int currentAmmoInClip;
    private int currentAmmoReserve;

    private bool isReloading = false;
    private bool isBursting = false;

    private float nextFireTime = 0f;

    public WeaponData Data => data;
    public int CurrentAmmo => currentAmmoInClip;
    public int MaxAmmo => data != null ? data.MaxAmmoInClip : 0;
    public int ReserveAmmo => currentAmmoReserve;
    public bool IsReloading => isReloading;

    public event System.Action OnAmmoChanged;
    public event System.Action OnReloadStateChanged;

    private void Awake()
    {
        if (data != null)
            Initialize(data);
    }

    public void Initialize(WeaponData weaponData)
    {
        data = weaponData;
        currentAmmoInClip = data.MaxAmmoInClip;
        currentAmmoReserve = data.MaxAmmoReserve;
    }

    public void TryShoot()
    {
        if (data == null || isReloading)
            return;

        if (Time.time < nextFireTime)
            return;

        if (isBursting)
            return;

        if (currentAmmoInClip <= 0)
        {
            if (currentAmmoReserve > 0)
                StartCoroutine(ReloadCoroutine());
            return;
        }

        Shoot();
    }

    private void Shoot()
    {
        if (data.BulletPrefab == null)
        {
            Debug.LogWarning($"[{data.WeaponName}] No hay prefab de bala asignado.");
            return;
        }

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Vector3 baseDirection = firePoint != null ? firePoint.forward : transform.forward;

        switch (data.FireMode)
        {
            case FireMode.Single:
                ConsumeAmmoAndCooldown();
                SpawnBullet(spawnPos, baseDirection);
                break;

            case FireMode.Shotgun:
                ConsumeAmmoAndCooldown();
                for (int i = 0; i < data.PelletsCount; i++)
                {
                    Vector3 spreadDir = ApplySpread(baseDirection, data.SpreadAngle);
                    SpawnBullet(spawnPos, spreadDir);
                }
                break;

            case FireMode.Burst:
                ConsumeAmmoAndCooldown();
                StartCoroutine(BurstCoroutine(spawnPos, baseDirection));
                break;
        }
    }

    private void ConsumeAmmoAndCooldown()
    {
        nextFireTime = Time.time + data.FireRate;
        currentAmmoInClip--;
        OnAmmoChanged?.Invoke();
    }

    private void SpawnBullet(Vector3 spawnPos, Vector3 direction)
    {
        GameObject bulletGO = Instantiate(data.BulletPrefab, spawnPos, Quaternion.LookRotation(direction));
        BulletPlayer bullet = bulletGO.GetComponent<BulletPlayer>();

        if (bullet != null)
        {
            bullet._bulletType = data._bulletType;
            bullet.Damage = data.Damage;
            bullet.Range = data.Range;
            bullet.Launch(direction);
        }
    }

    private IEnumerator BurstCoroutine(Vector3 spawnPos, Vector3 baseDirection)
    {
        isBursting = true;
        for (int i = 0; i < data.BurstCount; i++)
        {
            if (currentAmmoInClip < 0)
            {
                currentAmmoInClip = 0;
                OnAmmoChanged?.Invoke();
                break;
            }

            SpawnBullet(spawnPos, baseDirection);
            
            if (i > 0)
            {
                currentAmmoInClip--;
                OnAmmoChanged?.Invoke();
            }

            yield return new WaitForSeconds(data.BurstFireRate);
        }

        isBursting = false;
    }

    private Vector3 ApplySpread(Vector3 direction, float angle)
    {
        float half = angle / 2f;
        Quaternion spread = Quaternion.Euler(Random.Range(-half, half), Random.Range(-half, half), 0f);
        return spread * direction;
    }

    public void TryReload()
    {
        if (isReloading)
            return;

        if (currentAmmoInClip >= data.MaxAmmoInClip)
            return;

        if (currentAmmoReserve <= 0)
            return;

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        OnReloadStateChanged?.Invoke();
        yield return new WaitForSeconds(data.ReloadTime);
        int needed = data.MaxAmmoInClip - currentAmmoInClip;
        int taken = Mathf.Min(needed, currentAmmoReserve);
        currentAmmoInClip += taken;
        currentAmmoReserve -= taken;
        isReloading = false;
        OnAmmoChanged?.Invoke();
        OnReloadStateChanged?.Invoke();
    }

    public void AddAmmo(int amount)
    {
        currentAmmoReserve = Mathf.Min(currentAmmoReserve + amount, data.MaxAmmoReserve * 2);
        OnAmmoChanged?.Invoke();
        Debug.Log($"[{data.WeaponName}] +{amount} munición. Reserva: {currentAmmoReserve}");
    }
}