using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<Weapon> weaponSlots = new List<Weapon>();

    private int currentIndex = -1;
    private Weapon activeWeapon;

    public Weapon ActiveWeapon => activeWeapon;
    public int CurrentIndex => currentIndex;
    public int WeaponCount => weaponSlots.Count;

    public delegate void WeaponChangedEvent(Weapon newWeapon, int index);
    public event WeaponChangedEvent OnWeaponChanged;

    private void Start()
    {
        foreach (Weapon w in weaponSlots)
        {
            if (w == null) continue;
            w.gameObject.SetActive(false);
        }

        if (weaponSlots.Count > 0)
            SelectWeapon(0);
    }

    private void Update()
    {
        HandleWeaponInput();

        if (activeWeapon == null) return;

        if (Input.GetButton("Fire1"))
            activeWeapon.TryShoot();

        if (Input.GetKeyDown(KeyCode.R))
            activeWeapon.TryReload();
    }

    private void HandleWeaponInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) CycleWeapon(-1);
        else if (scroll < 0f) CycleWeapon(1);

        for (int i = 0; i < Mathf.Min(weaponSlots.Count, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectWeapon(i);
                break;
            }
        }
    }

    private void CycleWeapon(int direction)
    {
        if (weaponSlots.Count == 0) return;
        int newIndex = (currentIndex + direction + weaponSlots.Count) % weaponSlots.Count;
        SelectWeapon(newIndex);
    }

    public void SelectWeapon(int index)
    {
        if (index < 0 || index >= weaponSlots.Count) return;
        if (index == currentIndex) return;

        if (activeWeapon != null)
            activeWeapon.gameObject.SetActive(false);

        currentIndex = index;
        activeWeapon = weaponSlots[index];
        activeWeapon.gameObject.SetActive(true);

        OnWeaponChanged?.Invoke(activeWeapon, currentIndex);
    }

    public bool AddAmmoToWeapon(BulletType bulletType, int amount)
    {
        foreach (Weapon w in weaponSlots)
        {
            if (w != null && w.Data._bulletType == bulletType)
            {
                w.AddAmmo(amount);
                return true;
            }
        }
        return false;
    }

    public Weapon GetWeapon(int index)
    {
        if (index < 0 || index >= weaponSlots.Count) return null;
        return weaponSlots[index];
    }

    public List<Weapon> GetAllWeapons() => new List<Weapon>(weaponSlots);
}