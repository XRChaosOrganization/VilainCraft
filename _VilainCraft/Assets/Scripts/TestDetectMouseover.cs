using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDetectMouseover : MonoBehaviour
{
    public bool isActive { get; set; }
    public LayerMask mouseColliderLayerMask;
    SpriteRenderer sr0 = null;

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, mouseColliderLayerMask))
            {
                SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
                if (sr && sr != sr0)
                {
                    if (sr0)
                        sr0.enabled = false;
                    sr.enabled = true;
                    sr0 = sr;
                }
            }
            else 
            {
                if (sr0)
                {
                    sr0.enabled = false;
                    sr0 = null;
                }
                    
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            isActive = false;
            sr0.enabled = false;
            sr0 = null;
        }
    }
}
