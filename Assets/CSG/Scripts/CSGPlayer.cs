using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGPlayer : MonoBehaviour
{
    public float speed = 20.0f; //이동 속도

    private float hAxis;
    private float vAxis;

    bool wDown;
    bool jDown;
    bool iDown;

    bool isJump;
    bool isDodge;


    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;
    Rigidbody rb;

    GameObject nearObgect;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
    }

    void Move() //이동
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(isDodge)
        {
            moveVec = dodgeVec;
        }
        if (wDown)
        {
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        }
        else
        {
            transform.position += moveVec * speed * Time.deltaTime;
        }

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn() //회전
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump() //점프
    {
        if(jDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            rb.AddForce(Vector3.up * 17, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

        }
    }

    void Dodge() //회피
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }


    void OnCollisionEnter(Collision collision) //바닥 충돌 검사
    {
        if(collision.gameObject.tag == "Ground")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

}
