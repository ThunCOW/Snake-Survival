using PostEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    // VFX Settings
    private BloomCamera bloomCamera;
    private PostProcessVolume ppVolume;

    public GameObject SnakeHead;

    private Vector3 distOffset;
    void Start()
    {
        SnakeHead = SnakeManager.Instance.SnakeBody[0];
        distOffset = transform.position - SnakeHead.transform.position;

        bloomCamera = GetComponent<BloomCamera>();
        ppVolume = GetComponent<PostProcessVolume>();

        GameManager.Instance.SettingChanged += SettingChanged;

        SettingChanged();
    }

    private void SettingChanged()
    {
        if (!GameManager.Instance.VFX_Active)
        {
            bloomCamera.enabled = false;
            ppVolume.enabled = false;
        }
        else
        {
            bloomCamera.enabled = true;
            ppVolume.enabled = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = SnakeHead.transform.position + distOffset;   
    }

    void OnDrawGizmosSelected()
    {
        Camera camera = GetComponent<Camera>();
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 0.1F);
    }
}
