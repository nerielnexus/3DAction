using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniMonsterAI : MonoBehaviour
{

    Animator ani;    
    MiniAnimation miniAnimation;
    private int CountChk = 0; //잠에서 깨어나는 횟수 5
    private int MonsterHp = 100; //체력
    private bool isSleep;
    //------------------------------
    private Rigidbody rb;
     //public Transform Way; //순찰지점 저장 변수들
    int WayPoints = 0;
 
  //  [SerializeField] Transform[] Way;

    public Transform Homepoint;




    public Transform PlayerTarget, enemythis;

    public enum State { SlEEP, SLEEPEND,HOMP, SLEEPSTART, MOVE, DEATH, ATTACK, HIT }

    public State state = State.SlEEP;  //WingIDLE
    private bool isDead; //죽음체크변수
    private bool isPlayerAttack; //Move, Attack 차이 내는 변수
    //private bool isHit;
    public ParticleSystem BloodParticle;
  

    public float traceMove = 15.0f;//공격 사거리

    private float dist; //거리변수
    public float speed = 5f; //이동속도
    public float RotSpeed = 10.0f; //회전속도


    BoxCollider bx;
    NavMeshAgent nav;
   


   


    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();
        miniAnimation = GetComponentInChildren<MiniAnimation>();
        bx = GetComponentInChildren<BoxCollider>();
        rb = GetComponent<Rigidbody>();
      
        // transform.position = WingWay[LeePoints].transform.position;
        transform.position = Homepoint.transform.position;

        //nav = this.gameObject.GetComponent<NavMeshAgent>();
        nav = GetComponent<NavMeshAgent>();

        PlayerTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
        enemythis = this.gameObject.GetComponent<Transform>();

    }



    //Wing0,Wing1이 같이 나옴.. 한개만 되야하는데... , DownIDle였다가 벗어나면 땅밑으로 추락...(개선해야됨)
    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(PlayerTarget.position, enemythis.position);

    }
    IEnumerator CheckStateAction()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.00000000000001f);
            switch (state)
            {
                case State.SlEEP:

                    bx.enabled = false;
                    //CountChk = 0;
                    //애니메이션
                    //miniAnimation.OnReSleep(); 원래이거
                    miniAnimation.OnSleepStart();
                    miniAnimation.NotOnIdle();
                 
                    Debug.Log("Sleep");



                    break;
                case State.SLEEPEND:
                    //일어나는 애니메이션 -> 포효하는 애니메이션
                  
                    bx.enabled = false;
                    Dist();
                    miniAnimation.OnSleep();
                    Debug.Log("SleepEnd");
                    break;


                case State.HOMP:
                   // isPlayerAttack = false;
                    bx.enabled = false;
                 
                    //집으로 돌아가는 MOVE
                    CountChk = 0;

                    nav.speed = 5.0f;
                    nav.isStopped = true;
                    HomeRotate();
                    Debug.Log("집");


                    if (dist <= 5) // 5< s <25
                    {
                        state = State.MOVE;
                        nav.isStopped = false;
                    }


                    break;


                case State.SLEEPSTART:

                    //잠드는 애니메이션



                    break;


                case State.MOVE:

                  
                    //isPlayerAttack = false;
                    nav.isStopped = false;
                    bx.enabled = false;
                    miniAnimation.OnMove();

                    isSleep = false;
                    Debug.Log("Move");
                    nav.destination = PlayerTarget.position;
                    nav.speed = 10.0f;




                    if (dist <=4 &&!isPlayerAttack )  //    if (dist <= 4 && !isPlayerAttack) ------------------------------------
                    {
                        state = State.ATTACK;
                    }
                    else if (dist >= 20)
                    {
                        //제자리로 돌아가는 함수(HomePoint)
                        //상태변환 SleepStart->Sleep->SleePEnd

                        state = State.HOMP;

                    }
                    //
                    // Dir();



                    break;


                case State.HIT:

                    bx.enabled = false;
                    nav.isStopped = true;
                    Debug.Log("히트");
                  

                    //히트 애니메이션 실행
                    miniAnimation.Dmg();
                    isPlayerAttack = true;




                    if (dist >= 5)
                    {

                        state = State.MOVE;
                        miniAnimation.NotDmg();
                        isPlayerAttack = false;
                    }
                    else if (dist >= 20)
                    {
                        //제자리로 돌아가는 함수(HomePoint)
                        //상태변환 SleepStart->Sleep->SleePEnd

                        state = State.HOMP;
                        miniAnimation.NotDmg();
                        isPlayerAttack = false;

                    }

                 

                    break;


                case State.ATTACK:


                    Invoke("Box", 1.4f);
                    //isPlayerAttack = false;
                   // bx.enabled = true;
                  
                    Debug.Log("공격");
                    nav.isStopped = true;
                    miniAnimation.OnAttack();
                    OnAttack();
                    //공격애니메이션 실행
                    // //Move= <=10 | Home >=25  

                    if (dist >=5)
                    {
                        
                        nav.destination = PlayerTarget.position;
                        state = State.MOVE;
                        miniAnimation.OnNotAttack();

                    }
                    else if (dist >= 20)
                    {
                        //제자리로 돌아가는 함수(HomePoint)
                        //상태변환 SleepStart->Sleep->SleePEnd

                        state = State.HOMP;
                        miniAnimation.OnNotAttack();

                    }
                    /*else
                    {
                        nav.isStopped = true;
                    }*/



                    break;



                case State.DEATH:


                    bx.enabled = false;
                    isDead = true;
                    ani.SetTrigger("Die"); //죽음 애니메이션 발동  
                    Debug.Log("죽음");
                    StopAllCoroutines();

                    break;
            }
        }
    }


    IEnumerator CheckStage()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.15f);

            if (state == State.DEATH) { yield break; }
            dist = Vector3.Distance(PlayerTarget.position, enemythis.position);
           


            //sleepStart=0
            if (isSleep&&CountChk>4) //player-monster=거리<20
            {                    // OnWingMove();  ....1
                
                state = State.SLEEPEND;


            }

        }
    }

    public void Box()
    {
        bx.enabled = true;
    }
    public void Dist()
    {
        dist = Vector3.Distance(PlayerTarget.position, enemythis.position);

        if (dist >= 5 || dist<=20)
        {
            //불변수로 공격과 MOVE 차이를 줘야될것 같다.
            state = State.MOVE;
         
        }
    }


    public void Dir()
    {
        // Vector3 dir = transform.position - PlayerTarget.position;
        float dir = Vector3.Distance(this.transform.position, PlayerTarget.transform.position);

        if (dir < 15)
        {
            nav.isStopped = true;
        }
        else
        {
            nav.isStopped = false;
        }
        //한번 traceMove= Move로 돌입하면 계속 nav가 쫒아옴.. 이걸 원래 (HomePoint)로 돌아가게해줘야함..

    }

    private void OnEnable()
    {
        StartCoroutine(CheckStage());
        StartCoroutine(CheckStateAction());

    }



    public void OnMove()
    {

        /*   transform.position = Vector3.MoveTowards(transform.position, Way[WayPoints].transform.position, speed * Time.deltaTime); //speed*Time.deltaTime
                                                                                                                                    //Debug.Log(Vector3.Distance(transform.position, Way[WayPoints].transform.position));
           if (Vector3.Distance(transform.position, Way[WayPoints].transform.position) < 10)
           {
               WayPoints++;
               if (WayPoints == 3)
               {
                   WayPoints = 0;

               }

           }
        */
        Debug.Log("{0}" + WayPoints);

    }

    //이 상태로 할려면 1. Ridbody-Use Gravity=false, 2. nav.enabled=false; 네비 컴포넌트 자체를 비활성화
    //3. traceDist 사거리: 100에서 안으로 들어가야함
    //이러면 회전(방향)을 Lookat함수로 조정해야할듯...
    public void OnAttack()
    {
        transform.LookAt(PlayerTarget);
     /*   dist = Vector3.Distance(PlayerTarget.position, enemythis.position);
        if (dist >= 5)
        {
            nav.destination = PlayerTarget.position;
            state = State.MOVE;
            miniAnimation.OnNotAttack();
        }
        if (dist >= 25)
        {
            miniAnimation.OnNotAttack();
            state = State.HOMP;
        }
        else
        {
            nav.isStopped = true;
        }
       */


    }

 

    public void Rotate()
    {

        //   Vector3 dir = WingWay[LeePoints].transform.position - transform.position;
        //  transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
    }
    public void HomeRotate()
    {

        transform.position = Vector3.MoveTowards(transform.position, Homepoint.transform.position, speed * Time.deltaTime);
        Vector3 dir = Homepoint.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * RotSpeed);

        if(transform.position==Homepoint.transform.position)
        {
            //miniAnimation.OnSleepStart();
            miniAnimation.OnIdle();
            //잠드는 애니메이션
            state = State.SlEEP;
           
        }
      
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isDead) //죽음상태가 아닐때  / if (!isDead && !isPlayerAttack)
        {
            if (!isSleep)
            {
                if (other.transform.tag == "Melee")
                {

                    state = State.HIT;

                    BloodParticle.Play();

                    MonsterHp -= 8;// 
                    Invoke("Blood", 1.5f);

                    //피 효과 안나오게 해야함 / 피 효과 조절
                    if (MonsterHp <= 0)
                    {
                        state = State.DEATH;

                    }
                }
            }
        }


        if (!isDead) //죽음상태가 아닐때
        {
            if (!isSleep)
            {
                if (other.transform.tag == "Bullet")
                {

                    state = State.HIT;

                    BloodParticle.Play();

                    MonsterHp -= 8;// 
                    Invoke("Blood", 1.5f);

                    //피 효과 안나오게 해야함 / 피 효과 조절
                    if (MonsterHp <= 0)
                    {
                        state = State.DEATH;

                    }
                }
            }
        }
    }
  


    public void Blood()
    {
        BloodParticle.Stop();
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            CountChk++;
            isSleep = true;           
        }

    }
  
    
    

}
