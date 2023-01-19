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

    [Tooltip("In % of screen size")]
    [Range(0f,0.3f)] public float edgeScrollSize;



    void Start()
    {
        camBrain = GetComponent<CinemachineBrain>();
    }

    private void LateUpdate()
    {
        EdgeScroll();
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


    void EdgeScroll()
    {
        Vector2 edgeScroll = Vector2.zero;

        if (Input.mousePosition.x <= Screen.width * edgeScrollSize / 2)
            edgeScroll.x = -1;
        if (Input.mousePosition.x >= Screen.width * (1 - edgeScrollSize / 2))
            edgeScroll.x = 1;
        if (Input.mousePosition.y <= Screen.height * edgeScrollSize / 2)
            edgeScroll.y = -1;
        if (Input.mousePosition.y >= Screen.height * (1 - edgeScrollSize / 2))
            edgeScroll.y = 1;

        levelCam.moveInput = edgeScroll;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        
        Vector3 pos = transform.position + transform.forward * activeCam.m_Lens.NearClipPlane;
        Vector3[] frustum = new Vector3[4];
        Vector3[] corners = new Vector3[4];

        Camera.main.CalculateFrustumCorners(new Rect(0, 0, 1, 1), activeCam.m_Lens.NearClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustum);
        Vector2 size = new Vector2((frustum[3].x - frustum[0].x) / 2, (frustum[1].y - frustum[0].y) / 2);

        corners[0] = pos - transform.right * size.x * (1 - edgeScrollSize) - transform.up * size.y * (1 - edgeScrollSize);
        corners[1] = pos - transform.right * size.x * (1 - edgeScrollSize) + transform.up * size.y * (1 - edgeScrollSize);
        corners[2] = pos + transform.right * size.x * (1 - edgeScrollSize) + transform.up * size.y * (1 - edgeScrollSize);
        corners[3] = pos + transform.right * size.x * (1 - edgeScrollSize) - transform.up * size.y * (1 - edgeScrollSize);

        for (int i = 0; i < corners.Length; i++)
            Gizmos.DrawLine(corners[i], corners[(i+1)%4]);


    }



}



