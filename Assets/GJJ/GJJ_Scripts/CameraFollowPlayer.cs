using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        public Transform target = null;
        public UnityEngine.Camera myCamera = null;

        private void Awake()
        {
            if (target == null)
                throw new System.Exception(nameof(CameraFollowPlayer) + " - target is null");

            if (myCamera == null)
                throw new System.Exception(nameof(CameraFollowPlayer) + " - camera is null");
        }

        private void Update()
        {
            Vector3 cameraAnchor = new Vector3(
                target.position.x,
                target.position.y + 6,
                target.position.z - 10
                );

            myCamera.gameObject.transform.position = cameraAnchor;

            myCamera.gameObject.transform.RotateAround(target.position, target.up, target.rotation.z);
            myCamera.gameObject.transform.LookAt(target);
        }
    } 
}
