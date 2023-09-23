using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public int Speed;
    public float DisappearTime;
    public List<AudioClip> HitSound;
    public float volume;

    public ObjectPool<GameObject> pool;

    public Vector3 StartPoint;
    public Vector3 EndPoint;
    private Vector3 direction;

    private Rigidbody rb;
    private CapsuleCollider col;
    private GameObject projectileObject;
    private GameObject explosionObject;
    
    private AudioSource audioSource;

    public delegate void OnColliderEnterEvent(Projectile projectile, Collider other);
    public event OnColliderEnterEvent OnCollision;

    [SerializeField] private float distance;
    [SerializeField] private float remainingDistance;

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        projectileObject = transform.GetChild(0).gameObject;
        explosionObject = transform.GetChild(1).gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        distance = Vector3.Distance(StartPoint, EndPoint);
        remainingDistance = distance;
        direction = EndPoint - transform.position;
        direction.y = 0;
        //direction = EndPoint;
        //direction.Normalize();

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    bool canMove = true;
    private void Update()
    {
        if (canMove)
        {
            /*transform.parent.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance)));*/

            //remainingDistance -= Speed * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //if (canMove)
        //{
        //    transform.parent.transform.position += direction * Speed * Time.deltaTime;
        //    remainingDistance -= Speed * Time.deltaTime;
        //}
        if (canMove)
        {
            rb.velocity = transform.forward * 1000 * Time.deltaTime;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //rb.velocity = Vector3.zero;
        canMove = false;
        projectileObject.SetActive(false);
        explosionObject.SetActive(true);
        audioSource.PlayOneShot(HitSound[Random.Range(0, HitSound.Count)], volume * GameManager.Instance.SoundVolume);

        OnCollision?.Invoke(this, other);
        OnCollision = null;

        rb.velocity = Vector3.zero;
        col.enabled = false;

        StartCoroutine(BackToPool(DisappearTime));
    }

    private IEnumerator BackToPool(float Time)
    {
        yield return new WaitForSeconds(DisappearTime);

        GameObject parent = transform.parent.gameObject;

        pool.Release(parent);
        parent.SetActive(false);
    }

    private IEnumerator DisappearIfNotHit()
    {
        yield return new WaitForSeconds(15);
        canMove = false;
        projectileObject.SetActive(false);

        OnCollision = null;

        StartCoroutine(BackToPool(DisappearTime));
    }
    
    private void OnDisable()
    {
        canMove = false;
        OnCollision = null;
        rb.velocity = Vector3.zero;
        explosionObject.SetActive(false);
        StopAllCoroutines();
        //rb.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        col.enabled = true;

        distance = Vector3.Distance(StartPoint, EndPoint);
        remainingDistance = distance;
        //direction = EndPoint;
        //direction.Normalize();
        direction = EndPoint - transform.position;
        direction.y = 0;

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;

        projectileObject.SetActive(true);
        canMove = true;
        
        StartCoroutine(DisappearIfNotHit());
    }
}
