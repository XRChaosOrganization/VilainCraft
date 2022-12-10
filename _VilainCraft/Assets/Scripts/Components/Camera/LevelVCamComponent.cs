using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class LevelVCamComponent : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Transform focusPoint;
    CinemachineTrackedDolly dolly;

    public BoundsInt cameraBounds;
    public float farthestFOV;

    public AnimationCurve rotationLerp;
    public float lerpSpeed;
    public float zoomStep;
    [HideInInspector] public static float zoomClosest = 18;
    [HideInInspector] public float zoomFarthest;



    private void Start()
    {
        dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        StartCoroutine(RotateCoroutine(value > 0 ? 1 : -1));
    }

    public void Zoom(float z)
    {
        cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize + z * zoomStep, zoomClosest, zoomFarthest);
    }

    IEnumerator RotateCoroutine(int i)
    {
        float time = 0;
        float a = dolly.m_PathPosition;
        while (time < 1)
        {
            dolly.m_PathPosition = Mathf.Lerp(a, a + i, rotationLerp.Evaluate(time));
            time += Time.deltaTime * lerpSpeed;
            yield return null;
        }
        dolly.m_PathPosition = Mathf.RoundToInt(dolly.m_PathPosition) % 4;
        if (dolly.m_PathPosition < 0)
            dolly.m_PathPosition += 4;
        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.7f);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.7f);

        //Vector3 size = new Vector3(cameraBounds.xMax - cameraBounds.xMin, 0, cameraBounds.zMax - cameraBounds.zMin);

        //Gizmos.DrawWireCube(transform.position, size);
        Gizmos.DrawWireCube(cameraBounds.center, cameraBounds.size);
    }

    //public void SetBounds(BoundsInt tilemapBounds)
    //{
    //    cameraBounds.xMin = 
    //}


}
