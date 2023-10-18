using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAnimation : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSleep()
    {
        animator.SetBool("SleepEnd", true);  //일어나는 애니메이션 true
        animator.SetBool("Move", false);
        animator.SetBool("SleepStart", false);

        // animator.SetBool("SleepStart", false);//----------------
    }

    public void OnMove()
    {
        animator.SetBool("Move", true);  //움직이는 애니메이션 true

        animator.SetBool("Dag", false);

    }

    public void OnIdle()
    {
        animator.SetBool("Move", false);  
        animator.SetBool("Idle", true);
    }
    public void NotOnIdle()
    {
        animator.SetBool("SleepStart", true);  
        animator.SetBool("Idle", false);
    }

   


    public void OnSleepStart()
    {
        animator.SetBool("SleepStart", false); //잠에 드는 동작 True
        animator.SetBool("Idle", false);
        //   animator.SetBool("Move", false);
        //   animator.SetBool("SleepEnd", false);  //------------------------------------------여기


    }

    public void OnReSleep()
    {
        animator.SetBool("Sleep", true); //다시 잠드는 동작 true
        animator.SetBool("SleepEnd", false);

        //2번째 깨울때->
    }

    public void OnAttack()
    {
        animator.SetBool("Attack",true);
        animator.SetBool("Move", false);
    }

    public void OnNotAttack()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Move", true);

        //animator.SetBool("Attack", false);----------
        //animator.SetBool("Move", true);-----------
        
    }


    public void Dmg()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Dag",true );
      //  animator.SetBool("Move", false);//--------------------
    }

    public void NotDmg()
    {
        animator.SetBool("Move", false); // ((((((((((((( True)임 , Animator도 True))))
        animator.SetBool("Dag", false);  
       // animator.SetBool("Attack", false);//
    }

}
