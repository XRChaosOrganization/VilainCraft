using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class LevelCameraComponent : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineTrackedDolly dolly;

    public AnimationCurve rotationLerp;
    public float lerpSpeed;
    public float zoomStep;
    [HideInInspector] public static float zoomClosest = 18;
    [HideInInspector] public float zoomFarthest;

    private void Start()
    {
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
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
    


}
