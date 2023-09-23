using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

// GunType is being used to find the gun from the Guns list, like an ID
public enum GunType
{
    P_Handgun, S_Handgun,
    P_Uzi, S_Uzi,
    P_Shotgun,
    P_Rocket,
}

// Player weapons are the main weapons player will see and select
public enum PlayerWeapons
{
    Handgun,
    Uzi,
    Shotgun,
    Rocket,
    Locked
}

[CreateAssetMenu(fileName = "SnakeGun", menuName = "SnakeGun/Mounted Gun", order = 0)]
public class GunScriptableObject : ScriptableObject, System.ICloneable
{
    public GunType ID;
    public PlayerWeapons WeaponType;
    public Sprite SelectedWeaponSprite;
    public Sprite InventorySprite;
    public string Name;
    //public GameObject ModelPrefab;
    public string AnimatorLayerName;
    public float pushDistance;
    public bool Automatic;
    public float lastShootTime;

    public DamageConfigScriptableObject DamageConfig;
    public GunAmmoConfigScriptableObject AmmoConfig;
    public ShootConfigurationScriptableObject ShootConfig;
    public TrailConfigurationScriptableObject TrailConfig;
    public GunAudioConfigScriptableObject AudioConfig;

    public AudioSource ShootingAudioSource;

    private MonoBehaviour activeMonoBehaviour;
    //private GameObject model;
    private Animator gunAnimator;
    //private ParticleSystem shootSystem;
    private GameObject shootSystem;
    private GameObject weapon;
    private static ObjectPool<TrailRenderer> TrailPool;
    private static ObjectPool<GameObject> RocketPool;
    private static ObjectPool<GameObject> LaserAmmoPool;

    public void Spawn(GameObject Weapon, MonoBehaviour ActiveMonoBehaviour)
    {
        activeMonoBehaviour = ActiveMonoBehaviour;

        CreateProjectilePool();

        gunAnimator = Weapon.GetComponentInChildren<Animator>();
        ShootingAudioSource = Weapon.GetComponent<AudioSource>();

        shootSystem = Weapon;
        weapon = Weapon;
    }
    private void CreateProjectilePool()
    {
        switch (TrailConfig.ProjectileType)
        {
            case ProjectileType.Trail:
                if (TrailPool == null)
                    TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
                break;
            case ProjectileType.Rocket:
                if (RocketPool == null)
                    RocketPool = new ObjectPool<GameObject>(() => CreateProjectile(TrailConfig.ProjectileObject, RocketPool));
                break;
            case ProjectileType.LaserBullet:
                if (LaserAmmoPool == null)
                    LaserAmmoPool = new ObjectPool<GameObject>(() => CreateProjectile(TrailConfig.ProjectileObject, LaserAmmoPool));
                break;
            default:
                Debug.LogError("Create Projectile Type Here");
                break;
        }
    }
    public static void ClearProjectilePool()
    {
        if (TrailPool != null)
            TrailPool.Clear();
        if (RocketPool != null)
            RocketPool.Clear();
        if (LaserAmmoPool != null)
            LaserAmmoPool.Clear();
    }
    public void Shoot(Vector3 targetPos)       // bullet for multiple bullets per shot like how shotgun works
    {
        //gunAnimator.SetTrigger("Shoot");

        AudioConfig.PlayShootingClip(ShootingAudioSource);

        if (TrailConfig.ProjectileType == ProjectileType.Trail)
        {
            //for (int bullet = AmmoConfig.BulletPerShot; bullet > 0; bullet--)
            for (int bullet = 1; bullet > 0; bullet--)
            {
                //shootSystem.Play();
                Vector3 shootDirection = weapon.transform.forward
                //Vector3 shootDirection = model.transform.forward
                    + new Vector3(
                        Random.Range(
                            -ShootConfig.Spread.x,
                            ShootConfig.Spread.x
                            ),
                        Random.Range(
                            -ShootConfig.Spread.y,
                            ShootConfig.Spread.y
                            ),
                        Random.Range(
                            -ShootConfig.Spread.z,
                            ShootConfig.Spread.z)
                        );
                shootDirection.Normalize();

                if (Physics.Raycast(shootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
                {
                    activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));
                }
                else
                {
                    activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position, shootSystem.transform.position + (shootDirection * TrailConfig.MissDistance), new RaycastHit()));
                }
            }
        }
        else
        {
            Vector3 shootDirection = weapon.transform.forward
                    //Vector3 shootDirection = model.transform.forward
                    + new Vector3(
                        Random.Range(
                            -ShootConfig.Spread.x,
                            ShootConfig.Spread.x
                            ),
                        Random.Range(
                            -ShootConfig.Spread.y,
                            ShootConfig.Spread.y
                            ),
                        Random.Range(
                            -ShootConfig.Spread.z,
                            ShootConfig.Spread.z)
                        );
            shootDirection.Normalize();
            shootDirection = weapon.transform.position - targetPos;

            //PlayProjectile(shootSystem.transform.position, shootSystem.transform.position + (shootDirection * TrailConfig.MissDistance) * 8);
            PlayProjectile(weapon.transform.position, targetPos);
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit hit)
    {
        #region Spawned
        //                                                                                          ************* Spawned **********
        GameObject instance = null;
        switch (TrailConfig.ProjectileType)
        {
            case ProjectileType.Trail:
                TrailRenderer trail = TrailPool.Get();
                yield return null; //avoid position carry-over from last frame if reused
                trail.emitting = true;

                instance = trail.gameObject;
                break;
            case ProjectileType.Rocket:
                instance = RocketPool.Get();
                instance.GetComponentInChildren<Projectile>().OnCollision += HandleExplosion;
                instance.transform.position = StartPoint;
                instance.transform.GetChild(0).gameObject.SetActive(true);
#pragma warning disable
                instance.GetComponent<ParticleSystem>().enableEmission = true;
#pragma warning restore
                yield return null;
                break;
            case ProjectileType.LaserBullet:
                break;
            default:
                Debug.LogError("Create Projectile Type Here");
                break;
        }

        instance.transform.position = StartPoint;
        instance.transform.forward = SnakeManager.Instance.SnakeBody[0].transform.forward * -1;
        instance.SetActive(true);
        #endregion

        #region Flying
        //                                                                                              ************** Flying **********

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance)));

            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }
        #endregion

        #region Hit
        //                                                                                              ************* Hit **************

        instance.transform.position = EndPoint;

        switch (TrailConfig.ProjectileType)
        {
            case ProjectileType.Trail:
                if (hit.collider != null)
                {
                    // Handle Impact 
                    if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                    {
                        Vector3 forceDir = hit.transform.position - SnakeManager.Instance.SnakeBody[0].transform.position;
                        forceDir.y = 0;
                        forceDir.Normalize();
                        damageable.TakeDamage(DamageConfig.GetDamage(distance), forceDir, pushDistance);
                    }
                }
                yield return new WaitForSeconds(TrailConfig.Duration);
                yield return null;
                TrailRenderer trail = instance.GetComponent<TrailRenderer>();
                trail.emitting = false;
                instance.gameObject.SetActive(false);
                TrailPool.Release(trail);
                break;
            case ProjectileType.Rocket:
                yield return new WaitForSeconds(2.5f);      // wait for particle effect to disappear
                instance.gameObject.SetActive(false);
                RocketPool.Release(instance);
                break;
            case ProjectileType.LaserBullet:
                break;
            default:
                Debug.LogError("Create Projectile Type Here");
                break;
        }
        #endregion
    }

    private void PlayProjectile(Vector3 StartPoint, Vector3 EndPoint)
    {
        GameObject instance = null;
        switch (TrailConfig.ProjectileType)
        {
            case ProjectileType.Rocket:
                instance = RocketPool.Get();
                Projectile projectile = instance.GetComponentInChildren<Projectile>();
                projectile.OnCollision += HandleExplosion;
                projectile.StartPoint = StartPoint;
                projectile.EndPoint = EndPoint;
                instance.transform.position = StartPoint;
                instance.transform.GetChild(0).gameObject.SetActive(true);
#pragma warning disable
                instance.GetComponent<ParticleSystem>().enableEmission = true;
#pragma warning restore
                break;
            case ProjectileType.LaserBullet:
                instance = LaserAmmoPool.Get();
                projectile = instance.GetComponentInChildren<Projectile>();
                //projectile.OnCollision += HandleExplosion;
                projectile.StartPoint = StartPoint;
                projectile.EndPoint = EndPoint;
                instance.transform.position = StartPoint;
                instance.transform.GetChild(0).gameObject.SetActive(true);
#pragma warning disable
                //instance.GetComponent<ParticleSystem>().enableEmission = true;
#pragma warning restore

                break;
            default:
                Debug.LogError("Create Projectile Type Here");
                break;
        }

        //instance.transform.position = StartPoint;
        //instance.transform.forward = SnakeManager.Instance.SnakeBody[0].transform.forward * -1;
        instance.SetActive(true);
    }
    
    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    private GameObject CreateProjectile(GameObject ProjectilePrefab, ObjectPool<GameObject> ProjectilePool)
    {
        GameObject instance = Instantiate(TrailConfig.ProjectileObject);
        Projectile projectile = instance.GetComponentInChildren<Projectile>();
        projectile.pool = ProjectilePool;

        return instance;
    }

    private void HandleExplosion(Projectile projectile, Collider collider)
    {
        // sphere cast + explosion
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(projectile.transform.position, 4, mask);
        foreach (Collider hit in hitColliders)
        {
            if (hit != null)
            {
                float distance = Vector3.Distance(hit.transform.position, projectile.transform.position);
                Vector3 forceDir = hit.transform.position - projectile.transform.position;
                forceDir.y = 0;
                forceDir.Normalize();
                // Handle Impact 
                if (hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.TakeDamage(DamageConfig.GetDamage(distance), forceDir, pushDistance);
                }
            }
        }
    }

    public object Clone()
    {
        GunScriptableObject config = CreateInstance<GunScriptableObject>();

        config.ID = ID;
        config.WeaponType = WeaponType;
        config.SelectedWeaponSprite = SelectedWeaponSprite;
        config.Name = Name;
        //config.ModelPrefab = ModelPrefab;
        config.AnimatorLayerName = AnimatorLayerName;
        config.pushDistance = pushDistance;
        //config.BulletPerShot = BulletPerShot;
        config.Automatic = Automatic;
        //config.SpawnPoint = SpawnPoint;
        //config.SpawnRotation = SpawnRotation;

        config.DamageConfig = DamageConfig.Clone() as DamageConfigScriptableObject;
        //config.AmmoConfig = AmmoConfig.Clone() as GunAmmoConfigScriptableObject;
        config.ShootConfig = ShootConfig.Clone() as ShootConfigurationScriptableObject;
        config.TrailConfig = TrailConfig.Clone() as TrailConfigurationScriptableObject;
        config.AudioConfig = AudioConfig;

        return config;
    }
}