using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraForTest : MonoBehaviour
{
    public GameObject target = null;
    public Vector3 customDist;

    private void Awake()
    {
        transform.position = target.transform.position + customDist;
        transform.SetParent(target.transform);
        transform.LookAt(target.transform);
    }
}
