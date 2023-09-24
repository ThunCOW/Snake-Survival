using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Upgrade Tooltip List", menuName = "Guns/Upgrade Tooltip List")]
public class UpgradeTooltipScriptableObject : ScriptableObject, System.ICloneable
{
    public List<UpgradeTooltipInfo> UpgradeTooltipInfos = new List<UpgradeTooltipInfo>();

    public object Clone()
    {
        UpgradeTooltipScriptableObject upgradetooltip = CreateInstance<UpgradeTooltipScriptableObject>();

        foreach(UpgradeTooltipInfo info in UpgradeTooltipInfos)
        {
            upgradetooltip.UpgradeTooltipInfos.Add(new UpgradeTooltipInfo(info));
        }
        return upgradetooltip;
    }
}

[System.Serializable]
public class UpgradeTooltipInfo
{
    public UpgradeTooltipInfo(UpgradeTooltipInfo upgradeTooltipInfo)
    {
        TooltipInfos = new List<TooltipInfo>();
        TooltipInfos.AddRange(upgradeTooltipInfo.TooltipInfos);
        WeaponType = upgradeTooltipInfo.WeaponType;
    }
    public PlayerWeapons WeaponType;
    public List<TooltipInfo> TooltipInfos;
}

[System.Serializable]
public class TooltipInfo
{
    public string Description;
}