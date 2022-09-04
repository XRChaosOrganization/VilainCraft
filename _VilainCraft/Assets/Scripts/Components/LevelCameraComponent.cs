using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class LevelCameraComponent : MonoBehaviour
{
    CinemachineBrain camBrain;
    CinemachineVirtualCamera activeCam;
    CinemachineTrackedDolly dolly;

    public AnimationCurve rotationLerp;
    public float lerpSpeed;

    private void Start()
    {
        camBrain = GetComponent<CinemachineBrain>();
        activeCam = camBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        dolly = activeCam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            float value = context.ReadValue<float>();
            StartCoroutine(RotateCoroutine(value > 0 ? 1 : -1));
        }
        
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
