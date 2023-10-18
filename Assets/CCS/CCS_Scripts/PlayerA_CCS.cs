using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerA_CCS : MonoBehaviour
{
    private CharacterController _charController;
    private Animator _animator;

    private Transform meshPlayer;

    private float inputX;
    private float inputZ;
    private Vector3 v_movement;
    private float moveSpeed;
    private float gravity;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 0.1f;
        gravity = 0.5f;
        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        meshPlayer = tempPlayer.transform.GetChild(0);
        _charController = tempPlayer.GetComponent<CharacterController>();
        _animator = meshPlayer.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        
        if (inputX == 0 && inputZ== 0)
        {
            _animator.SetBool("isRun", false);
            _animator.SetBool("idle", true);

        }
        else
        {
            _animator.SetBool("isRun", true);
        }
    }

    private void FixedUpdate()
    {
        // 중력
        if (_charController.isGrounded)
        {
            v_movement.y = 0f;
        }
        else
        {
            v_movement.y -= gravity * Time.deltaTime;
        }

        // 이동
        v_movement = new Vector3(inputX * moveSpeed, v_movement.y, inputZ * moveSpeed);
        _charController.Move(v_movement);

        // 각도
        if (inputX != 0 || inputZ != 0)
        {
            Vector3 lookDir = new Vector3(v_movement.x, 0, v_movement.z);
            meshPlayer.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}
