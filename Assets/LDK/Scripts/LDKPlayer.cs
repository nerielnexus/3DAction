using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDKPlayer : MonoBehaviour
{
    MiniMonsterAI miniMonster;
    // Start is called before the first frame update
    public float movespeed = 10.0f;
    public float RotateSpeed = 10.0f;
    private Rigidbody rigidbody;
    private BoxCollider boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        miniMonster = GameObject.Find("GolemPrefab").GetComponent<MiniMonsterAI>();
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Attack();

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 Dir = new Vector3(h, 0, v).normalized;

        // 바라보는 방향으로 회전 후 다시 정면을 바라보는 현상을 막기 위해 설정
        if (!(h == 0 && v == 0))
        {
            // 이동과 회전을 함께 처리
            transform.position += Dir * movespeed * Time.deltaTime;
            // 회전하는 부분. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Dir), Time.deltaTime * RotateSpeed);
            rigidbody.velocity = (Dir * movespeed);

            transform.LookAt(transform.position + Dir);

        }
    }

    public void Attack()
    {
       // boxCollider.enabled = false;

     /*   Debug.Log("콜라이더");
         if(miniMonster.isPlayerCHK==true) //  if(miniMonster.state==MiniMonsterAI.State.HIT)
        {
             boxCollider.enabled = true;
             Debug.Log("콜라이더");
         }
         else if(miniMonster.isPlayerCHK == false)
         {
             boxCollider.enabled = false;
         }
        */
    }
    
}
