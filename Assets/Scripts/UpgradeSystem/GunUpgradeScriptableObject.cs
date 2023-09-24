using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade List", menuName = "Guns/Upgrade List", order = 5)]
public class GunUpgradeScriptableObject : ScriptableObject, System.ICloneable
{
    public List<UpgradeList> Upgrades = new List<UpgradeList>();

    public object Clone()
    {
        GunUpgradeScriptableObject gunUpgrade = CreateInstance<GunUpgradeScriptableObject>();
        
        //gunUpgrade.Upgrades.AddRange(this.Upgrades);

        foreach (UpgradeList upgradeList in Upgrades)
        {
            gunUpgrade.Upgrades.Add(new UpgradeList(upgradeList));
        }

        return gunUpgrade;
    }
}

[System.Serializable]
public class UpgradeList
{
    public UpgradeList(UpgradeList upgradeList)
    {
        UpgradeL = new List<UpgradeWeapon>();
        UpgradeL.AddRange(upgradeList.UpgradeL);
        UpgradeType = upgradeList.UpgradeType;

    }
    public PlayerWeapons UpgradeType;
    public List<UpgradeWeapon> UpgradeL;
}

[System.Serializable]
public class UpgradeWeapon
{
    public int Level;
    public List<Object> Upgrades;
}