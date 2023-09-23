using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    public int IncreaseEXP;

    private Collider boxCollider;

    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        ExperienceManager.Instance.currentEXP += IncreaseEXP;
        boxCollider.enabled = false;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        boxCollider.enabled = true;
    }
}
