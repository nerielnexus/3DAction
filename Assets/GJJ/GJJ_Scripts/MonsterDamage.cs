using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public class MonsterDamage : MonoBehaviour
    {
        [Header("Monster")]
        public MonsterBehaviors mb = null;
        public LayerMask layerToDamage;

        [Header("Using Sword")]
        public Collider swordCollider = null;

        [Header("Using Gun")]
        public Transform barrelend = null;
        public GameObject bullet = null;
        public GameObject muzzleflash = null;

        [Header("Damage Settings")]
        private float swordDamage;
        private float gumDamageMultiplier;

        private void Awake()
        {
            mb = GetComponent<MonsterBehaviors>();
            swordDamage = mb.swordDamage;
            gumDamageMultiplier = mb.gunDamageMultiplier;
            layerToDamage = GetComponent<MonsterAwarenessCollider>().layerToDetect;
        }

        public void DamageToPlayer()
        {
            if(mb.isUsingGun)
            {
                Debug.Log("monster dealt GUN damage to player");
            }
            else
            {
                Debug.Log("monster dealt SWORD damage to player");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(gameObject.name + " enter ontriggerenter");

            if (!other.gameObject.CompareTag("Player"))
                return;

            Debug.Log(other.gameObject.tag + " ok");

            if (!(1 << other.gameObject.layer).Equals(layerToDamage.value))
                return;

            Debug.Log(other.gameObject.layer + " ok");

            DamageToPlayer();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(gameObject.name + " enter ontriggerenter");

            if (!collision.gameObject.CompareTag("Player"))
                return;

            Debug.Log(collision.gameObject.tag + " ok");

            if (!(1 << collision.gameObject.layer).Equals(layerToDamage.value))
                return;

            Debug.Log(collision.gameObject.layer + " ok");

            DamageToPlayer();
        }
    } 
}
