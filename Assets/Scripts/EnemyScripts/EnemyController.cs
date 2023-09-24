using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float turnSpeed = 7;

    private Vector3 targetPos;
    private Rigidbody rb;
    private SphereCollider col;

    private bool isDead;

    private EnemyHealth health;
    private FadeOutToObjectPool pool;

    private static ObjectPool<GameObject> CrystalDropPool;

    public GameObject CrystalDropPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pool = GetComponent<FadeOutToObjectPool>();
        health = GetComponent<EnemyHealth>();
        col = GetComponent<SphereCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (CrystalDropPool == null)
            CrystalDropPool = new ObjectPool<GameObject>(() => CreateCrystalDrop());

        //StartCoroutine(GetLocationEveryX(Random.Range(0.25f, 0.7f)));
        health.OnDeath += Death;
    }

    void Update()
    {
        if (!isDead)
        {
            GetLocation();
        }
    }
    
    void FixedUpdate()
    {
        if (!isDead)
        {
            EnemyMovement();
        }
    }

    private void EnemyMovement()
    {
        rb.velocity = transform.forward * speed * Time.deltaTime;
        Vector3 distance = targetPos - transform.position;
        distance.y = 0;
        Quaternion rotation = Quaternion.LookRotation(distance);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    private void GetLocation()
    {
        targetPos = SnakeManager.Instance.SnakeBody[0].transform.position;
    }
    //IEnumerator GetLocationEveryX(float x)
    //{
    //    targetPos = SnakeManager.Instance.SnakeBody[0].transform.position;
    //    
    //    if (Vector3.Distance(targetPos, transform.position) < 10)
    //    {
    //
    //    }
    //
    //    yield return new WaitForSeconds(x);
    //    
    //    StartCoroutine(GetLocationEveryX(x));
    //}

    private void Death(Vector3 forceDir)
    {
        //GameManager.Instance.StartCoroutine(BloodSplash());
        isDead = true;

        //ScoreManager.Instance.IncreaseScore(ScorePoint);

        col.enabled = false;

        //ragdollManager.Activate(true);
        //ragdollManager.RagdollDeath(forceDir);

        pool.FadeOut();

        GameObject crystalDrop = CrystalDropPool.Get();
        crystalDrop.SetActive(true);
        crystalDrop.transform.position = transform.position;

        //if (ReactionRoutine != null)
            //StopCoroutine(ReactionRoutine);

        //GameManager.Instance.KillCount++;
    }

    private GameObject CreateCrystalDrop()
    {
        GameObject instance = Instantiate(CrystalDropPrefab);
        return instance;
    }
    void OnEnable()
    {
        isDead = false;

        col.enabled = true;
    }
}
