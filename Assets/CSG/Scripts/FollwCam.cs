using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollwCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;


    void Update()
    {
        transform.position = target.position + offset;
    }
}
