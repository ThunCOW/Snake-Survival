using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class GunUpgradeManager : MonoBehaviour
{
    [SerializeField] 
    private GunUpgradeScriptableObject GunUpgradeList;

    private void Awake()
    {
        GunUpgradeList = GunUpgradeList.Clone() as GunUpgradeScriptableObject;

        //ExperienceManager.Instance.LevelUp += Upgrade;
    }

    private void Upgrade(PlayerWeapons UpgradeWeaponType)
    {
        if (GunUpgradeList.Upgrades.Count == 0)
            return;

        List<SnakeMountedWeapon> mountedWeapons;
        foreach (UnityEngine.Object WeaponUpgrade in GunUpgradeList.Upgrades[(int)UpgradeWeaponType].UpgradeL[0].Upgrades)
        {
            mountedWeapons = SnakeManager.Instance.SnakeWeapons.FindAll(gun => gun.WeaponType == UpgradeWeaponType);
            foreach (SnakeMountedWeapon snakeMountedWeapon in mountedWeapons)
                UpgradeWeapon(WeaponUpgrade, snakeMountedWeapon.MountedWeapon);
        }
        /*switch (UpgradeWeaponType)
        {
            case PlayerWeapons.RocketLauncher:
                foreach (UnityEngine.Object WeaponUpgrade in GunUpgradeList.Upgrades[(int)UpgradeWeaponType].UpgradeL[0].Upgrades)
                {
                    mountedWeapons = SnakeManager.Instance.SnakeWeapons.FindAll(gun => gun.WeaponType == UpgradeWeaponType);
                    foreach(SnakeMountedWeapon snakeMountedWeapon in mountedWeapons)
                    UpgradeWeapon(WeaponUpgrade, snakeMountedWeapon.MountedWeapon);
                }
                break;

            case PlayerWeapons.LaserCannon:
                foreach (UpgradeWeapon WeaponUpgrade in GunUpgradeList.Upgrades[0].UpgradeL)
                {
                    // Primary
                    gun = SnakeManager.Instance.SnakeWeapons.Find(gun => gun.WeaponType == PlayerWeapons.RocketLauncher).MountedWeapon;
                    UpgradeWeapon(WeaponUpgrade.Upgrades, gun);
                }
                break;

            case PlayerWeapons.ShockwaveTower:
                foreach (UpgradeWeapon WeaponUpgrade in GunUpgradeList.Upgrades[0].UpgradeL)
                {
                    // Primary
                    gun = SnakeManager.Instance.SnakeWeapons.Find(gun => gun.WeaponType == PlayerWeapons.ShockwaveTower).MountedWeapon;
                    UpgradeWeapon(WeaponUpgrade.Upgrades, gun);
                }
                break;

            case PlayerWeapons.LaserBeam:
                foreach (UpgradeWeapon WeaponUpgrade in GunUpgradeList.Upgrades[0].UpgradeL)
                {
                    // Primary
                    gun = SnakeManager.Instance.SnakeWeapons.Find(gun => gun.WeaponType == PlayerWeapons.LaserBeam).MountedWeapon;
                    UpgradeWeapon(WeaponUpgrade.Upgrades, gun);
                }
                break;
        }*/
        GunUpgradeList.Upgrades[(int)UpgradeWeaponType].UpgradeL.RemoveAt(0);

        //if (GunUpgradeList.Upgrades.Count != 0 && GunUpgradeList.Upgrades[0].Level < CurrentLevel) Upgrade(UpgradeWeaponType);
    }

    private void UpgradeWeapon<T>(T Upgrade, GunScriptableObject Gun)
    {
        Type type = Gun.GetType();
        
        foreach (FieldInfo field in type.GetFields())
        {
            //Debug.Log(field.FieldType + " VS " + Upgrade.GetType());
            if (field.FieldType == Upgrade.GetType())
            {
                field.SetValue(Gun, Upgrade);
                break;
            }
        }
    }
}