using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMountedWeapon : MonoBehaviour
{
    // Editor Filled
    [SerializeField]
    private GunScriptableObject MountedWeaponSO;

    [SerializeField]
    private GunScriptableObject MountedWeapon;

    [SerializeField]
    private LayerMask EnemyLayer;

    [SerializeField]
    private float enemyDetectionRadiusEditor;

    [SerializeField]
    private float waitUntilNextSearchEditor;

    // Private Variables
    private float enemyDetectionRadius;
    private float waitUntilNextSearch;
    private EnemyHealth nearestEnemy;


    void Start()
    {
        MountedWeapon = MountedWeaponSO.Clone() as GunScriptableObject;

        enemyDetectionRadius = enemyDetectionRadiusEditor;

        MountedWeapon.Spawn(gameObject, this);
    }

    private void Update()
    {
        FindTarget();
        RotateWeapon();
        Shoot();
    }
    private void FindTarget()
    {
        waitUntilNextSearch -= Time.deltaTime;
        if (waitUntilNextSearch <= 0)
        {
            waitUntilNextSearch = waitUntilNextSearchEditor;

            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyDetectionRadius, EnemyLayer);
            if (colliders.Length == 0)
            {
                nearestEnemy = null;
                return;
            }

            Collider nearestCollider = null;
            float minSqrDistance = Mathf.Infinity;
            for (int i = 0; i < colliders.Length; i++)
            {
                float sqrDistanceToCenter = (transform.position - colliders[i].transform.position).sqrMagnitude;
                if (sqrDistanceToCenter < minSqrDistance)
                {
                    minSqrDistance = sqrDistanceToCenter;
                    nearestCollider = colliders[i];
                }
            }

            nearestEnemy = nearestCollider.GetComponent<EnemyHealth>();

            if (colliders.Length > 5)
                enemyDetectionRadius *= 3 / 4f;
            else
                enemyDetectionRadius = Mathf.Clamp(enemyDetectionRadius * 5 / 3, 0, enemyDetectionRadiusEditor);
        }
    }

    private void RotateWeapon()
    {
        if (nearestEnemy == null)
        {
            return;
        }
        //Vector3 targetDirection = nearestEnemy.transform.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(targetDirection);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10 * Time.deltaTime);
        Vector3 targetDirection = nearestEnemy.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 10 * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    private void Shoot()
    {
        if (nearestEnemy == null) return;

        if (nearestEnemy.CurrentHealth == 0)
        {
            nearestEnemy = null;
            return;
        }

        if (Time.time > MountedWeapon.ShootConfig.FireRate + MountedWeapon.lastShootTime)
        {
            MountedWeapon.Shoot(nearestEnemy.transform.position);

            MountedWeapon.lastShootTime = Time.time;
        }
    }
}
