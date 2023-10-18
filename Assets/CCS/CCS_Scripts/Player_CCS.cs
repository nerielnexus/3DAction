using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CCS : MonoBehaviour
{
    public float speed;
    public float jumpPow;
    public float gravity;

    private Vector3 moveDir;
    private bool jumpButtonPressed;
    private bool flyingMode;
    

    public CharacterController selectPlayer;
    void Start()
    {
        speed = 10.0f;
        gravity = 5.0f;
        moveDir = Vector3.zero;
        jumpPow = 5.0f;
        jumpButtonPressed = false;
        flyingMode = false;
    }

    void Update()
    {
        if (selectPlayer == null) return;

        if (selectPlayer.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDir = selectPlayer.transform.TransformDirection(moveDir);
            moveDir *= speed;

            if(jumpButtonPressed == false && Input.GetButton("Jump"))
            {
                jumpButtonPressed = true;
                moveDir.y = jumpPow;
            }
        }

        else
        {
            if (moveDir.y < 0 && jumpButtonPressed == false && Input.GetButton("Jump"))
            {
                flyingMode = true;
            }

            if(flyingMode)
            {
                jumpButtonPressed = true;

                moveDir.y *= 0.95f;

                if (moveDir.y > -1) moveDir.y = -1;

                moveDir.x = Input.GetAxis("Horizontal");
                moveDir.z = Input.GetAxis("Vertical");

            }

            else
            {
                moveDir.y -= gravity * Time.deltaTime;
            }
        }

        if(!Input.GetButton("Jump"))
        {
            jumpButtonPressed = false;
            flyingMode = false;
        }

        selectPlayer.Move(moveDir * Time.deltaTime);
    }

}
