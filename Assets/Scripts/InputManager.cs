using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Mouse Aim Variables
    [Header("Mouse Aim")]
    [SerializeField] 
    private LayerMask groundMask;
    public GameObject AimTarget;
    [SerializeField]
    private Camera mainCamera;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleTargetPosition();
    }

    private void HandleTargetPosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            Vector3 dif = new Vector3((hitInfo.point.x - transform.position.x) * -1, 0, (hitInfo.point.z - transform.position.z) * -1);     // -1 because character has reverse coordinates
            float sum = Mathf.Abs(dif.x) + Mathf.Abs(dif.z);
            Vector3 dir = new Vector3(dif.x / sum * -1, 0, dif.z / sum);
            //Debug.Log(dir);

            AimTarget.transform.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);

            //Debug.DrawLine(transform.position, AimTarget.transform.position, Color.red, 0);
        }
        else
        {
            //Debug.LogWarning("Player can't aim!, Missing ground layer!");
        }
    }
}
