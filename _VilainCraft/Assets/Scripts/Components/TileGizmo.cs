using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR

public class TileGizmo : MonoBehaviour
{

    [HideInInspector]public bool hasBlocker;
    public Transform tileContent;


    private void OnDrawGizmos()
    {

        if (hasBlocker && tileContent.childCount == 0)
        {
            Vector3 sphereOrigin = tileContent.position;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(sphereOrigin, 1);
            Gizmos.DrawWireCube(tileContent.position, new Vector3(4, 0, 4));

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(sphereOrigin, 1);
            Gizmos.DrawLine(sphereOrigin + Vector3.down, tileContent.position);
        }
    }
}

#endif
