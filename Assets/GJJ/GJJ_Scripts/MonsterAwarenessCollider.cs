using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public class MonsterAwarenessCollider : MonoBehaviour
    {
        [Header("Awareness Collider Informations")]
        public MonsterBehaviors mb = null;
        public LayerMask layerToDetect;
        public GameObject detectedTarget = null;
        public float purseInterval = 0f;
        public float purseDistance = 0f;

        private bool loopFlag = false;
        private Collider[] pursedObject = null;

        [Header("TEST")]
        public float _dist;

        private IEnumerator ScanPurse()
        {
            if (!loopFlag)
            {
                loopFlag = true;

                pursedObject = Physics.OverlapSphere(transform.position, purseDistance, layerToDetect);

                if (pursedObject != null)
                    foreach (Collider col in pursedObject)
                        detectedTarget = IdentifyTarget(col.gameObject);

                yield return new WaitForSeconds(purseInterval);
                loopFlag = false;
            }
        }

        private GameObject IdentifyTarget(GameObject other)
        {
            if (!other.CompareTag("Player")) return null;

            if (!(1<<other.layer).Equals(layerToDetect.value)) return null;

            return other;
        }

        private void Awake()
        {
            mb = GetComponent<MonsterBehaviors>();
            purseDistance = mb.distanceAware;
        }

        private void Update()
        {
            if(detectedTarget != null)
                _dist = Vector3.Distance(transform.position, detectedTarget.transform.position);

            StartCoroutine(ScanPurse());

            mb.targetPlayer = detectedTarget;
        }
    } 
}
