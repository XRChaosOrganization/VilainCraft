using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class LevelCameraController : MonoBehaviour
{

    public CinemachineBrain camBrain;
    public CinemachineVirtualCamera activeCam => (CinemachineVirtualCamera)camBrain.ActiveVirtualCamera;
    public LevelVCamComponent levelCam => activeCam.GetComponentInParent<LevelVCamComponent>();



    void Start()
    {
        camBrain = GetComponent<CinemachineBrain>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if (levelCam && context.started)
            levelCam.Rotate(context);
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        float z = Mathf.Clamp(context.ReadValue<Vector2>().y, -1f, 1f);
        if (levelCam )
            levelCam.Zoom(z);
    }

    public void Move(InputAction.CallbackContext context)
    {
        levelCam.moveInput = context.ReadValue<Vector2>();
    }
}
