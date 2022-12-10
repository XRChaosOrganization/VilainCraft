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



    // Start is called before the first frame update
    void Start()
    {
        camBrain = GetComponent<CinemachineBrain>();
        //activeCam = (CinemachineVirtualCamera) camBrain.ActiveVirtualCamera;
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        if (levelCam && context.started)
            levelCam.Rotate(context);
        else Debug.LogError("Active Camera does not contain a LevelCameraComponent");
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        
        float z = Mathf.Clamp(context.ReadValue<Vector2>().y, -1f, 1f);
        if (levelCam )
            levelCam.Zoom(z);
        else Debug.LogError("Active Camera does not contain a LevelCameraComponent");

    }
}
