using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private string weaponName = "Pistol";
    [SerializeField] private Sprite weaponIcon;

    [SerializeField] private BulletType bulletType;

    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float range = 50f;

    [SerializeField] private int maxAmmoInClip = 30;
    [SerializeField] private int maxAmmoReserve = 120;

    [SerializeField] private float reloadTime = 1.5f;

    [SerializeField] private GameObject bulletPrefab;
    //[SerializeField] private GameObject muzzleFlashPrefab;

    [SerializeField] private FireMode fireMode = FireMode.Single;

    [SerializeField] private int pelletsCount = 8;
    [Range(0f, 45f)]
    [SerializeField] private float spreadAngle = 15f;

    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstFireRate = 0.08f;
    public string WeaponName
    {
        get { return weaponName; }
        set { weaponName = value; }
    }
    public Sprite WeaponIcon
    {
        get { return weaponIcon; }
        set { weaponIcon = value; }
    }
    public BulletType _bulletType
    {
        get { return bulletType; }
        set { bulletType = value; }
    }
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }
    public float Range
    {
        get { return range; }
        set { range = value; }
    }
    public int MaxAmmoInClip
    {
        get { return maxAmmoInClip; }
        set { maxAmmoInClip = value; }
    }
    public int MaxAmmoReserve
    {
        get { return maxAmmoReserve; }
        set { maxAmmoReserve = value; }
    }
    public float ReloadTime
    {
        get { return reloadTime; }
        set { reloadTime = value; }
    }
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
        set { bulletPrefab = value; }
    }
    /*public GameObject MuzzleFlashPrefab
    {
        get { return muzzleFlashPrefab; }
        set { muzzleFlashPrefab = value; }
    }*/
    public FireMode FireMode
    {
        get { return fireMode; }
        set { fireMode = value; }
    }
    public int PelletsCount
    {
        get { return pelletsCount; }
        set { pelletsCount = value; }
    }
    public float SpreadAngle
    {
        get { return spreadAngle; }
        set { spreadAngle = value; }
    }
    public int BurstCount
    {
        get { return burstCount; }
        set { burstCount = value; }
    }
    public float BurstFireRate
    {
        get { return burstFireRate; }
        set { burstFireRate = value; }
    }
}
public enum FireMode
{
    Single,
    Shotgun,
    Burst
}