using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public Transform targetTransform;

    public float dist = 7.0f;
    public float height = 2.0f;
    public float dampTrace = 20.0f;

    private Transform transform;


    void Start()
    {
        transform= GetComponent<Transform>();
    }


    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position
                                           - (targetTransform.forward * dist) + (Vector3.up * height)
                                           , Time.deltaTime * dampTrace);
        transform.LookAt(targetTransform.position);
    }
}
