using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTooltip : MonoBehaviour
{
    [Header("Weapon")]
    public GunScriptableObject UpgradeWeapon;

    [Header("Editor Filled")]
    [SerializeField]
    private TMP_Text WeaponNameText;
    [SerializeField]
    private TMP_Text WeaponDesc;
    [SerializeField]
    private Image WeaponIMG;

    private UpgradeManager upgradeManager;

    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GetComponentInParent<UpgradeManager>();
    }

    public void Populate(string Desc)
    {
        WeaponNameText.text = UpgradeWeapon.name;
        WeaponDesc.text = Desc;
        WeaponIMG.sprite = UpgradeWeapon.SelectedWeaponSprite;
    }

    public void ButtonSelect()
    {
        upgradeManager.Select(this);
    }
}
