using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster_CCS : MonoBehaviour
{ 
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;
    public Rigidbody rigid;
    public SkinnedMeshRenderer[] meshs;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    void Update()
    {
        healthBar.value = HP;
    }
    void TakeDamage()
    {
        if (HP <= 0)
        {
            //Play Death Animation
            animator.SetTrigger("die");
        }
        else
        {
            //Play Get Hit Animation
            animator.SetTrigger("damage");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            HP -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec, false));
        }
        else if (other.tag == "Bullet")
        {
            EnemyBullet bullet = other.GetComponent<EnemyBullet>();
            HP -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {

        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            TakeDamage();
            mesh.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }
        if (HP > 0)
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        else
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;

           
            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
            }

                Destroy(gameObject, 4);

        }
    }
}
