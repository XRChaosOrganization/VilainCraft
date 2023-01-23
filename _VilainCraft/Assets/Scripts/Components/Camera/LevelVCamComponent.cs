using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class LevelVCamComponent : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Transform focusPoint;
    public CinemachineTrackedDolly dolly;

    public BoundsInt cameraBounds;
    public Bounds zoomedBounds;
    public float farthestFOV;
    [HideInInspector] public static float closestFOV = 3f;

    public float moveSpeed;
    public AnimationCurve rotationLerp;
    public float lerpSpeed;
    public float zoomStep;

    [HideInInspector] public Vector3 cameraDirUp;
    [HideInInspector] public Vector3 cameraDirRight;
    [HideInInspector] public Vector3 moveInput;
    



    private void Start()
    {
        dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
        GetCameraDir(Mathf.RoundToInt(dolly.m_PathPosition));
    }

    private void Update()
    {
        HandleMovement();
    }



    #region Public Methods
    public void Rotate(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        int i = Mathf.RoundToInt(dolly.m_PathPosition + value) % 4;
        if (i < 0)
            i += 4;
        GetCameraDir(i);

        StartCoroutine(RotateCoroutine(value > 0 ? 1 : -1));
    }

    public void Zoom(float z)
    {

        cam.m_Lens.FieldOfView = Mathf.Clamp(cam.m_Lens.FieldOfView + z * zoomStep * Time.deltaTime, closestFOV, farthestFOV);
        float f = (farthestFOV - cam.m_Lens.FieldOfView) / (farthestFOV - closestFOV);
        zoomedBounds = new Bounds(cameraBounds.center, (Vector3)cameraBounds.size * f);
        HandleMovement();
    }

    #endregion

    #region Private Methods
    void HandleMovement()
    {
        Debug.Log(moveInput);
        Vector3 dir = (cameraDirUp * moveInput.y + cameraDirRight * moveInput.x).normalized;
        Vector3 newPos = focusPoint.position + dir * moveSpeed * Time.deltaTime;
        Vector3 clampedPos = new Vector3(Mathf.Clamp(newPos.x, zoomedBounds.min.x, zoomedBounds.max.x), 0f, Mathf.Clamp(newPos.z, zoomedBounds.min.z, zoomedBounds.max.z));
        focusPoint.position = clampedPos;
    }


    void GetCameraDir(int i)
    {
        Vector3 pathPos = dolly.m_Path.EvaluatePosition(i);
        cameraDirUp = new Vector3(-pathPos.x / Mathf.Abs(pathPos.x), 0f, -pathPos.z / Mathf.Abs(pathPos.z));
        cameraDirRight = new Vector3(cameraDirUp.z, 0f, -cameraDirUp.x); // Rotate vector 90° Clockwise : (x , y) -> (y , -x)
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

    #endregion

    #region Gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(focusPoint.position, 0.7f);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(focusPoint.position, 0.7f);

        Gizmos.DrawWireCube(cameraBounds.center, cameraBounds.size);


        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(zoomedBounds.center, zoomedBounds.size);
    }

    #endregion
}
