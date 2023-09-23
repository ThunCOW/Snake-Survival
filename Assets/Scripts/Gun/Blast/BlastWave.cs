using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWave : MonoBehaviour
{
    public float BlastRepeatTime;

    [Space]
    [SerializeField]
    private int PointsCount;
    [SerializeField]
    private float MaxRadius;
    [SerializeField]
    private float Speed;
    [SerializeField]
    private float StartWidth;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = PointsCount + 1;
    }

    void Start()
    {
        StartCoroutine(Blast());
    }

    private IEnumerator Blast()
    {
        yield return new WaitForSeconds(BlastRepeatTime);

        float currentRadius = 0f;

        while (currentRadius < MaxRadius)
        {
            currentRadius += Time.deltaTime * Speed;
            Draw(currentRadius);
            yield return null;
        }

        StartCoroutine(Blast());
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / PointsCount;

        for (int i = 0; i < PointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0, StartWidth, 1f - currentRadius / (MaxRadius));
    }
}
