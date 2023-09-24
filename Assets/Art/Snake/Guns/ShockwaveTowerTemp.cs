using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveTowerTemp : MonoBehaviour
{
    public AudioClip ShockSound;

    void Start()
    {
        StartCoroutine(ShockwaveStart(0));
    }

    IEnumerator ShockwaveStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        HandleExplosion();

        GetComponent<AudioSource>().PlayOneShot(ShockSound, 0.3f * GameManager.Instance.SoundVolume);

        StartCoroutine(ShockwaveStart(.95f));
    }

    private void HandleExplosion()
    {
        // sphere cast + explosion
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 13, mask);
        foreach (Collider hit in hitColliders)
        {
            if (hit != null)
            {
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                Vector3 forceDir = hit.transform.position - transform.position;
                forceDir.y = 0;
                forceDir.Normalize();
                // Handle Impact 
                if (hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.TakeDamage(150, forceDir, 2);
                }
            }
        }
    }
}
