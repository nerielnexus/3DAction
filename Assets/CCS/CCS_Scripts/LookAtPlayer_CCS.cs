using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer_CCS : MonoBehaviour
{
    public Transform cam;
    
    void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
