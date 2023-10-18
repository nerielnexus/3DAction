using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public class MonsterDamageTrigger : MonoBehaviour
    {
        [Header("Monster")]
        public MonsterBehaviors mb = null;
        public MonsterAwarenessCollider mac = null;
        public LayerMask layerToDamage;

        [Header("Gun Attack")]
        public Transform barrelend = null;
        public GameObject bullet = null;
        public GameObject muzzleflashEffect = null;

        [Header("Damage Settings")]
        public float swordDamage;
        public float gunDamageMultiplier;

        public void DamageToPlayer(GameObject other)
        {
            if (mb.isUsingGun)
            {
                Debug.Log("monster dealt " + swordDamage * gunDamageMultiplier + " GUN damage to player");
                other.GetComponent<Player>().health -= (int)(swordDamage * gunDamageMultiplier);
            }
            else
            {
                Debug.Log("monster dealt " + swordDamage + " SWORD damage to player");
                other.GetComponent<Player>().health -= (int)swordDamage;
            }
        }

        private void Awake()
        {
            swordDamage = mb.swordDamage;
            gunDamageMultiplier = mb.gunDamageMultiplier;
            layerToDamage = mac.layerToDetect;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            if (!(1 << other.gameObject.layer).Equals(layerToDamage.value))
                return;

            DamageToPlayer(other.gameObject);
        }
    } 
}
