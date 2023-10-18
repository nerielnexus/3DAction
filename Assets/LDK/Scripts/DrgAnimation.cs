using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrgAnimation : MonoBehaviour
{
    


    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnIdleAni()
    {
      
        anim.SetBool("DownIdle", true);
        anim.SetBool("Move", false);//-------------

    }

    public void OnMoveAni()
    {
        anim.SetBool("DownIdle", false);
        anim.SetBool("Move", true);
        anim.SetBool("Fire", false);//-----------
    }
    public void NotOnMoveAni()
    {
        anim.SetBool("DownIdle", true);
        anim.SetBool("Move", false);
        anim.SetBool("Fire", false);//-----------

    }

    public void FireBrass()
    {
        anim.SetBool("DownIdle", false);
        anim.SetBool("Fire", true);
        anim.SetBool("Move", false);
    }

    public void NotFireBrass()
    {

        anim.SetBool("DownIdle", false);
        anim.SetBool("Fire", false);
        anim.SetBool("Move", true);
    }

    public void AttackBigAni()
    {
        anim.SetBool("AttackBigMouth", true);
        anim.SetBool("DownIdle", false);
        anim.SetBool("Fire", false);
        anim.SetBool("Move", false);
    }

    public void AttackNotBigAni()
    {
        anim.SetBool("AttackBigMouth", false);
        anim.SetBool("DownIdle", false);
        anim.SetBool("Fire", false);
        anim.SetBool("Move", true);
    }

    public void AttackSmaillAni()
    {
        anim.SetBool("AttackSMMouth", true);
        anim.SetBool("DownIdle", false);
        anim.SetBool("Fire", false);
        anim.SetBool("Move", false);
    }

    public void AttackNotSmaillAni()
    {
        anim.SetBool("AttackSMMouth", false);
        anim.SetBool("DownIdle", false);
        anim.SetBool("Fire", false);
        anim.SetBool("Move", true);
    }

    public void DieAni()
    {
        //죽음 애니메이션
    }



}
