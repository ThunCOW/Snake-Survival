using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("*********** Unit Prefabs *********")]
    [SerializeField]
    private GameObject Fighter_Weak_Prefab;
    [SerializeField]
    private GameObject Tanky_1_Prefab;

    public enum EnemyType
    {
        Fighter_Weak,
    }

    [Header("********* Spawn Phases ************")]
    public bool CanSpawn;

    public List<SpawnPhase> SpawnPhases;
    public int SpawnPhaseCount;
    public List<GameObject> SpawnBorderPoints;
    
    public ObjectPool<GameObject> FighterWeakPool;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        FighterWeakPool = new ObjectPool<GameObject>(() => CreatePool_FighterWeak(Fighter_Weak_Prefab, FighterWeakPool));

        StartCoroutine(AutomatedSpawnCoRoutine(0));
    }

    // Update is called once per frame
    void Update()
    {
        AutomatedSpawn();
    }

    private void AutomatedSpawn()
    {
        if (CanSpawn)
        {
            
        }
    }

    private IEnumerator AutomatedSpawnCoRoutine(float wait)
    {
        yield return new WaitForSeconds(wait);

        if (CanSpawn)
        {
            if (GameTimeCounter.Instance.CurrentPlayTime >= SpawnPhases[SpawnPhaseCount].SpawnUntil_X_MinuteIsPassed)
            {
                SpawnPhaseCount++;
            }

            Vector3 randomSpawnPos = Vector3.zero;

            int randomSpawnPoint = Random.Range(0, 4);
            if (randomSpawnPoint < 3)
                randomSpawnPos = GetRandomVector3Between(SpawnBorderPoints[randomSpawnPoint].transform.position, SpawnBorderPoints[randomSpawnPoint + 1].transform.position);
            else
                randomSpawnPos = GetRandomVector3Between(SpawnBorderPoints[randomSpawnPoint].transform.position, SpawnBorderPoints[0].transform.position);

            //Debug.Log(randomSpawnPoint);
            //UnityEditor.EditorApplication.isPaused = true;

            //GameObject spawn = Instantiate(Fighter_Weak_Prefab, randomSpawnPos, Fighter_Weak_Prefab.transform.rotation);
            GameObject spawn = GetPool(SpawnPhases[SpawnPhaseCount].EnemyList[Random.Range(0, SpawnPhases[SpawnPhaseCount].EnemyList.Count)]).Get();
            spawn.SetActive(true);
            spawn.transform.position = randomSpawnPos;

            StartCoroutine(AutomatedSpawnCoRoutine(SpawnPhases[SpawnPhaseCount].SpawnEvery_X_Seconds));
        }
    }

    public Vector3 GetRandomVector3Between(Vector3 v1, Vector3 v2)
    {
        return new Vector3(Random.Range(v1.x, v2.x), 0, Random.Range(v1.z, v2.z)); ;
    }

    private GameObject CreatePool_FighterWeak(GameObject UnitPrefab, ObjectPool<GameObject> UnitPool)
    {
        GameObject instance = Instantiate(UnitPrefab);
        instance.GetComponent<FadeOutToObjectPool>().Pool = UnitPool;

        return instance;
    }

    public ObjectPool<GameObject> GetPool(EnemyType Type)
    {
        switch (Type)
        {
            case EnemyType.Fighter_Weak:
                return FighterWeakPool;
        }
        
        return null;
    }

    [System.Serializable]
    public class SpawnPhase
    {
        public List<EnemyType> EnemyList;
        public float SpawnEvery_X_Seconds;
        public float SpawnUntil_X_MinuteIsPassed;
    }
}