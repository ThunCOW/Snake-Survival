using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    public static SnakeManager Instance;

    [Header("*********** Editor Filled **********")]
    [Space]
    [SerializeField]
    private List<GameObject> editorBodyParts;
    [SerializeField]
    private float distanceBetween;

    [Space]
    public GameObject StartWeapon;

    public GameObject WeaponBody;
    public GameObject RocketLauncher;
    public GameObject LaserCannon;
    public GameObject ShockwaveTower;

    [Header("********** Runtime Filled *********")]
    public List<GameObject> SnakeBody = new List<GameObject>();

    [Space]
    public List<SnakeMountedWeapon> SnakeWeapons;

    // ******** Private Fields
    private float countUp = 0;

    private SnakeController snakeController;

    void Awake()
    {
        Instance = this;

        snakeController = GetComponent<SnakeController>();
    }

    void Start()
    {
        InitializeBodyParts();  
        SnakeWeapons = new List<SnakeMountedWeapon>();
        SnakeWeapons.AddRange(GetComponentsInChildren<SnakeMountedWeapon>());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddBodyParts(RocketLauncher);
        }
        else if(Input.GetKeyDown(KeyCode.T))
        {
            AddBodyParts(LaserCannon);
        }
    }
    private void FixedUpdate()
    {
        if (editorBodyParts.Count > 0)
        {
            InitializeBodyParts();
        }
    }

    private void InitializeBodyParts()
    {
        // Instantiate Snake Head
        if (SnakeBody.Count == 0)
        {
            GameObject temp = Instantiate(editorBodyParts[0], transform.position, transform.rotation, transform);
            temp.AddComponent<SnakePartManager>();
            
            SnakeBody.Add(temp);

            editorBodyParts.RemoveAt(0);
        }

        SnakePartManager snakePartM = SnakeBody[SnakeBody.Count - 1].GetComponent<SnakePartManager>();
        if (countUp == 0)
        {
            //snakePartM.ClearUpdateSnakePartLocationList();
        }
        
        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            GameObject tempBody = Instantiate(editorBodyParts[0], snakePartM.SnakeParts[0].position, snakePartM.SnakeParts[0].rotation, transform);
            if (editorBodyParts.Count > 1)
            tempBody.AddComponent<SnakePartManager>();

            SnakeBody.Add(tempBody);
            editorBodyParts.RemoveAt(0);
            //tempBody.GetComponent<SnakePartManager>().ClearUpdateSnakePartLocationList();
            countUp = 0;

            if (editorBodyParts.Count >= 1)
            {
                Vector3 weaponSpawnOffset = new Vector3(0, 1.51f, -0.03045034f);
                GameObject tempWeapon = Instantiate(StartWeapon, tempBody.transform);
                tempWeapon.transform.localPosition = weaponSpawnOffset;
            }
        }
    }

    public void AddBodyParts(GameObject NewWeapon)
    {
        StartCoroutine(snakeController.IncreaseBodySize(distanceBetween));

        SnakePartManager snakePartM = SnakeBody[SnakeBody.Count - 2].GetComponent<SnakePartManager>();

        //editorBodyParts.Insert(editorBodyParts.Count - 2, NewBodyPart);
        GameObject tempBody = Instantiate(WeaponBody, snakePartM.SnakeParts[0].position, snakePartM.SnakeParts[0].rotation, transform);
        tempBody.AddComponent<SnakePartManager>();

        SnakeBody.Insert(SnakeBody.Count - 1, tempBody);
        Vector3 weaponSpawnOffset = new Vector3(0, 1.51f, -0.03045034f);
        GameObject tempWeapon = Instantiate(NewWeapon, tempBody.transform);
        tempWeapon.transform.localPosition = weaponSpawnOffset;

        SnakeWeapons.Add(tempWeapon.GetComponent<SnakeMountedWeapon>());
    }

    public void AddBodyParts(PlayerWeapons playerWeapon)
    {
        GameObject NewBodyPart;
        switch (playerWeapon)
        {
            case PlayerWeapons.RocketLauncher:
                NewBodyPart = RocketLauncher;
                break;
            case PlayerWeapons.LaserCannon:
                NewBodyPart = LaserCannon;
                break;
            case PlayerWeapons.ShockwaveTower:
                NewBodyPart = ShockwaveTower;
                break;
            case PlayerWeapons.LaserBeam:
                NewBodyPart = LaserCannon;
                break;
            default:
                NewBodyPart = RocketLauncher;
                break;
        }
        AddBodyParts(NewBodyPart);
    }
}