using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    private UpgradeTooltip[] UpgradeTooltipArray;

    [Space]
    [SerializeField]
    private UpgradeTooltipScriptableObject UpgradeTooltipSO;

    [Space]
    [SerializeField]
    private SnakeMountedWeapon[] SnakeMountedWeapons;

    private GameObject Menu;

    void Start()
    {
        //UpgradeTooltipArray = GetComponentsInChildren<UpgradeTooltip>();

        UpgradeTooltipSO = UpgradeTooltipSO.Clone() as UpgradeTooltipScriptableObject;

        ExperienceManager.Instance.LevelUp += LevelUp;

        Menu = transform.GetChild(0).gameObject;
    }

    private void LevelUp()
    {
        Time.timeScale = 0;

        Menu.SetActive(true);

        int RandomNumber = Random.Range(0, 3);

        GunScriptableObject gun;
        switch (RandomNumber)
        {
            case 0:
                // Rocket Launcher
                gun = SnakeMountedWeapons[0].MountedWeapon;
                break;
            case 1:
                // Laser Cannon
                gun = SnakeMountedWeapons[1].MountedWeapon;
                break;
            case 2:
                // Shockwave Tower
                gun = SnakeMountedWeapons[2].MountedWeapon;
                break;
            case 3:
                // Laser Beam
                gun = SnakeMountedWeapons[3].MountedWeapon;
                break;
            default:
                gun = null;
                break;
        }

        PopulateTooltipMenu(gun, UpgradeTooltipArray[0], UpgradeTooltipSO.UpgradeTooltipInfos[RandomNumber].TooltipInfos[0].Description);

        int newRand = Random.Range(0, 3);
        while(newRand == RandomNumber)
            newRand = Random.Range(0, 3);

        switch (newRand)
        {
            case 0:
                // Rocket Launcher
                gun = SnakeMountedWeapons[0].MountedWeapon;
                break;
            case 1:
                // Laser Cannon
                gun = SnakeMountedWeapons[1].MountedWeapon;
                break;
            case 2:
                // Shockwave Tower
                gun = SnakeMountedWeapons[2].MountedWeapon;
                break;
            case 3:
                // Laser Beam
                gun = SnakeMountedWeapons[3].MountedWeapon;
                break;
            default:
                gun = null;
                break;
        }

        PopulateTooltipMenu(gun, UpgradeTooltipArray[1], UpgradeTooltipSO.UpgradeTooltipInfos[newRand].TooltipInfos[0].Description);
    }

    private void PopulateTooltipMenu(GunScriptableObject gun, UpgradeTooltip tooltipMenu, string Desc)
    {
        tooltipMenu.UpgradeWeapon = gun;
        tooltipMenu.Populate(Desc);
    }

    public void Select(UpgradeTooltip tooltip)
    {
        GunScriptableObject selectedGun = tooltip.UpgradeWeapon;

        SnakeManager.Instance.AddBodyParts(selectedGun.WeaponType);

        UpgradeTooltipSO.UpgradeTooltipInfos[(int)selectedGun.WeaponType].TooltipInfos.RemoveAt(0);

        Menu.SetActive(false);

        Time.timeScale = 1;
    }
}
