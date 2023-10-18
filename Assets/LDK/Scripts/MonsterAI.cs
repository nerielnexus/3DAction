using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MonsterAI : MonoBehaviour
{


    private int MonsterHp = 250;
    private BoxCollider bx;
    private Rigidbody rb;
    // public List<Transform> WayPoints; //순찰지점 저장 변수들
    int WayPoints = 0;
    int LeePoints = 0;
    //[SerializeField] Transform[] Way;
    public List<GameObject> Way;
    public List<GameObject> WingWay;
    public Transform Homepoint;
    public ParticleSystem FireParticle;
 //   public GameObject BloodParticle;
    public ParticleSystem BloodParticle;

    public Transform FirePoint;
   

    private DrgAnimation DrgAnimation;
    private ColliderBoss colliderBoss;
    Animator ani;
 
    private int CheckIDX=0; //Idle 체크변수
   // float damping = 1.0f;

   // bool _Partrolling;// 순찰여부 판단변수


    public Transform PlayerTarget, enemythis;

    public enum State {WingIDLE, IDLE, WALK, MOVE, WINGMOVE, DEATH, ATTACKFIRE, ATTACKARM, ATTACKMONTH, HIT }

    private State state = State.WINGMOVE;  //WingIDLE
    private bool isDead; //죽음체크변수
    private bool isAttack; //공격체크변수
    private bool isFireChk;



    private float fDestroyTime = 4.0f; //방향 딜레이 시간
    private float fTickTime=0.0f;  //딜레이 변수
 //   private float rotateSpeed=100f;
    int idx;


    

    

    public float traceDist = 100.0f;//추적 사거리 
    public float traceMove = 30.0f;//공격 사거리

    private float dist; //거리변수
    public float speed=5f; //이동속도
    public float Rotspeed = 10f;

    bool isWalk=true;
 

    CapsuleCollider cp;

   
    NavMeshAgent nav;



    


    // Start is called before the first frame update
    void Start()
    {
        
        colliderBoss = GetComponentInChildren<ColliderBoss>();
        ani = GetComponent<Animator>();
        
        DrgAnimation = GetComponentInChildren<DrgAnimation>();
        cp = GetComponentInChildren<CapsuleCollider>();  //--cp = GetComponentInChildren<CapsuleCollider>();
        bx = GetComponentInChildren<BoxCollider>();
        rb = GetComponent<Rigidbody>();
       
        //transform.position = Way[WayPoints].transform.position; --- 둘다 처음에 포지션을 이렇게 설정해줘서 충돌이 일어남
        transform.position = WingWay[LeePoints].transform.position;
       // transform.position = Homepoint.transform.position;  -> 아마 나중에 HomePoint 필요함<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        //nav = this.gameObject.GetComponent<NavMeshAgent>();
        nav = GetComponent<NavMeshAgent>();

        PlayerTarget = GameObject.FindWithTag("Player").GetComponent<Transform>();
        enemythis = this.gameObject.GetComponent<Transform>();
        //StartCoroutine(this.CheckStage());
        //StartCoroutine(this.CheckStateAction());
    } 


  
  
    // Update is called once per frame
    void Update()
    {

        //BloodParticle.Play();
        //yield return new WaitForSeconds(0.2f);


        /*switch (state)
        {
            //case State.WingIDLE:


            // Debug.Log("WingMove0");
            // HomeRotate();
            //CheckIDX = 1;
            //  break;
            case State.IDLE:


                FalseFire();
                DrgAnimation.OnIdleAni();
                colliderBoss.OnNotFireCollder();
                nav.speed = 5;


                FireParticle.Stop();

                CheckIDX = 1;
                DownIdle();
                nav.enabled = false; //이건 false 고정임.. 그래야 자연스럽게 내려옴


                // 가만히서 자고 있는 상태 -> 포효함




                break;


            case State.WALK:





                //OnMove();


                break;


            case State.MOVE:

                FalseFire();
                DrgAnimation.OnMoveAni();
                colliderBoss.OnNotFireCollder();
                Debug.Log("Move");
                CheckIDX = 1;
                //Move에서 회전 각도 조절
                FireParticle.Stop();




                nav.enabled = true;
                dist = Vector3.Distance(PlayerTarget.position, enemythis.position);
                nav.destination = PlayerTarget.position;
                nav.speed = 10.0f;

                //Debug.Log(nav.speed+"MOVE");


                nav.isStopped = false;

                if (dist < 25)
                {
                    // nav.isStopped = true;
                    state = State.ATTACKFIRE;
                    FireParticle.Play();



                    //범위에서 벗어나면 다시 Idle상태로
                }
                else if (dist > 45)  //if (dist<=traceMove) 45
                {
                    isWalk = true;
                    state = State.IDLE;

                    DrgAnimation.NotOnMoveAni();
                }


                /*else if(dist>25) 
                {
                    state = State.MOVE;
                }
                else
                {
                    state = State.IDLE;
                }

                


                break;

            case State.WINGMOVE:


                OnWingMove();
                CheckIDX = 0;


                break;

            case State.HIT:

                break;

            case State.ATTACKFIRE:

                FalseFire();
                DrgAnimation.FireBrass();
                colliderBoss.OnFireCollder();
                CheckIDX = 1;

                idx = Random.Range(1, 3);
                //Debug.Log(idx);

                nav.isStopped = true;

                transform.LookAt(PlayerTarget);
                isAttack = true; //공격 상태 체크 변수
                                 //isWalk = false; //걷기 상태 체크 변수

                //  Debug.Log(transform.position);
                if (dist >= 25)
                {

                    //  nav.isStopped = false;
                    isAttack = false;
                    state = State.MOVE;
                    DrgAnimation.NotFireBrass();
                }
                //***** else if****문 원래

                if (dist <= 8 && idx == 1)
                {

                    state = State.ATTACKARM;
                    FireParticle.Stop();


                }
                else if (dist <= 8 && idx == 2)
                {
                    state = State.ATTACKMONTH;
                    FireParticle.Stop();

                }





                break;

            case State.ATTACKARM:

                // colliderBoss.OnBoxCollder();
                colliderBoss.OnNotFireCollder();

                CheckIDX = 1;
                Debug.Log("1");
                AttackRotate();
                isAttack = true;
                DrgAnimation.AttackSmaillAni();


                if (dist >= 25)
                {
                    isAttack = false;
                    state = State.MOVE;
                    DrgAnimation.AttackNotSmaillAni();
                    idx = 0;
                }
                break;

            case State.ATTACKMONTH:

                // colliderBoss.OnBoxCollder();
                colliderBoss.OnNotFireCollder();
                CheckIDX = 1;
                Debug.Log("2");
                AttackRotate();
                isAttack = true;
                DrgAnimation.AttackBigAni();

                if (dist >= 25)
                {

                    isAttack = false;
                    state = State.MOVE;
                    DrgAnimation.AttackNotBigAni();
                    idx = 0;
                }


                break;


            case State.DEATH:

                isDead = true;
                ani.SetTrigger("Die"); //죽음 애니메이션 발동  
                Debug.Log("죽음");
                // StopAllCoroutines();






                //3초후 사라지기


                break;
        }
    */


    }
    IEnumerator CheckStateAction()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.00000000000001f);

            switch (state)
            {
                //case State.WingIDLE:


                // Debug.Log("WingMove0");
                // HomeRotate();
                //CheckIDX = 1;
                //  break;
                case State.IDLE:


                    FalseFire();
                    DrgAnimation.OnIdleAni();
                    colliderBoss.OnNotFireCollder();
                    nav.speed = 5;


                    FireParticle.Stop();

                    CheckIDX = 1;
                    DownIdle();
                    nav.enabled = false; //이건 false 고정임.. 그래야 자연스럽게 내려옴


                    // 가만히서 자고 있는 상태 -> 포효함




                    break;


                case State.WALK:





                    //OnMove();


                    break;


                case State.MOVE:

                    FalseFire();
                    DrgAnimation.OnMoveAni();
                    colliderBoss.OnNotFireCollder();
                    Debug.Log("Move");
                    CheckIDX = 1;
                    //Move에서 회전 각도 조절
                    FireParticle.Stop();




                    nav.enabled = true;
                    dist = Vector3.Distance(PlayerTarget.position, enemythis.position);
                    nav.destination = PlayerTarget.position;
                    nav.speed = 10.0f;

                    //Debug.Log(nav.speed+"MOVE");


                    nav.isStopped = false;

                    if (dist < 35)
                    {
                        // nav.isStopped = true;
                        state = State.ATTACKFIRE;
                        FireParticle.Play();



                        //범위에서 벗어나면 다시 Idle상태로
                    }
                    else if (dist > 55)  //if (dist<=traceMove) 45
                    {
                        isWalk = true;
                        state = State.IDLE;

                        DrgAnimation.NotOnMoveAni();
                    }


                    /*else if(dist>25) 
                    {
                        state = State.MOVE;
                    }
                    else
                    {
                        state = State.IDLE;
                    }

                    */


                    break;

                case State.WINGMOVE:


                    OnWingMove();
                    CheckIDX = 0;


                    break;

                case State.HIT:

                    break;

                case State.ATTACKFIRE:

                    FalseFire();
                    DrgAnimation.FireBrass();
                    colliderBoss.OnFireCollder();
                    CheckIDX = 1;

                    idx = Random.Range(1, 3);
                    //Debug.Log(idx);

                    nav.isStopped = true;

                    transform.LookAt(PlayerTarget);
                    isAttack = true; //공격 상태 체크 변수
                                     //isWalk = false; //걷기 상태 체크 변수

                    //  Debug.Log(transform.position);
                    if (dist >= 30)
                    {

                        //  nav.isStopped = false;
                        isAttack = false;
                        state = State.MOVE;
                        DrgAnimation.NotFireBrass();
                    }
                    //***** else if****문 원래

                    if (dist <= 15 && idx == 1)
                    {

                        state = State.ATTACKARM;
                        FireParticle.Stop();


                    }
                    else if (dist <= 15 && idx == 2)
                    {
                        state = State.ATTACKMONTH;
                        FireParticle.Stop();

                    }





                    break;

                case State.ATTACKARM:

                    // colliderBoss.OnBoxCollder();
                    colliderBoss.OnNotFireCollder();

                    CheckIDX = 1;
                    Debug.Log("1");
                    AttackRotate();
                    isAttack = true;
                    DrgAnimation.AttackSmaillAni();


                    if (dist >= 30)
                    {
                        isAttack = false;
                        state = State.MOVE;
                        DrgAnimation.AttackNotSmaillAni();
                        idx = 0;
                    }
                    break;

                case State.ATTACKMONTH:

                    // colliderBoss.OnBoxCollder();
                    colliderBoss.OnNotFireCollder();
                    CheckIDX = 1;
                    Debug.Log("2");
                    AttackRotate();
                    isAttack = true;
                    DrgAnimation.AttackBigAni();

                    if (dist >= 30)
                    {

                        isAttack = false;
                        state = State.MOVE;
                        DrgAnimation.AttackNotBigAni();
                        idx = 0;
                    }


                    break;


                case State.DEATH:

                    isDead = true;
                    ani.SetTrigger("Die"); //죽음 애니메이션 발동  
                    Debug.Log("죽음");
                    StopAllCoroutines();






                    //3초후 사라지기


                    break;
            }
        }
       
    }
    //*/

    //Wing0,Wing1이 같이 나옴.. 한개만 되야하는데... ,
    IEnumerator CheckStage()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.15f);  //0.000000000001f = 0.2f

            if (state==State.DEATH) { yield break; }
            

            dist = Vector3.Distance(PlayerTarget.position, enemythis.position);

            if (CheckIDX == 0)
            {
                state = State.WINGMOVE;
            }
            

            if (dist <= traceDist) // 내려오는 동작 , 산책을 시작함
            {

                if (isWalk)
                {
                    state = State.IDLE;
                }


            }
           if (dist<=traceMove&&!isAttack)
            {
                    isWalk = false;
                    state = State.MOVE;  
                    
            }
           

        }
    }

   private void OnEnable()
    {
       StartCoroutine(CheckStage());
       StartCoroutine(CheckStateAction());
       
    }

    public void DownIdle()  //Waypoint0,1,2(y)값 -5.1
    {


        
        cp.enabled = true;
        if (rb.useGravity == false)  //userGriaviy가 flase가 맞다면 (안에 문장 실행)
        {

            //Debug.Log("리지드바디");
            rb.useGravity = true;
        }
            //처음에는 False로 해서 (하늘에 둥둥) -> true로 하면 (땅으로 내려앉음)  Nav게이션자리
            //범위 안에 들어와있을때 중력이 발동되야 하므로 ->true


           // Debug.Log("DownIdle");
            
       transform.position = Vector3.MoveTowards(transform.position, Way[WayPoints].transform.position, speed * Time.deltaTime);
        Rotate1();
        if (transform.position == Way[WayPoints].transform.position)
            {
                WayPoints++;

                if (WayPoints == 3)
                {

                    WayPoints = 0;
                }
            }
        
        

    }

    public void OnMove()
    {

        Debug.Log("OnMove");
        transform.position = Vector3.MoveTowards(transform.position, Way[WayPoints].transform.position, speed * Time.deltaTime); //speed*Time.deltaTime
        //Debug.Log(Vector3.Distance(transform.position, Way[WayPoints].transform.position));
        if (Vector3.Distance(transform.position, Way[WayPoints].transform.position) < 2.0f) //1.5->
         {
             WayPoints++;
             if (WayPoints == 3)
             {
                 WayPoints = 0;

             }

         }
      

    }

    //이 상태로 할려면 1. Ridbody-Use Gravity=false, 2. nav.enabled=false; 네비 컴포넌트 자체를 비활성화
    //3. traceDist 사거리: 100에서 안으로 들어가야함
    //이러면 회전(방향)을 Lookat함수로 조정해야할듯...
    public void OnWingMove()
    {    
        Rotate();
        transform.position=Vector3.MoveTowards(transform.position, WingWay[LeePoints].transform.position, speed * Time.deltaTime);

        if (transform.position == WingWay[LeePoints].transform.position)
        {
            LeePoints++;

            if (LeePoints == 5)
            {
              
                LeePoints = 0;
            }
        }
      
        //Debug.Log(Vector3.Distance(transform.position, Way[WayPoints].transform.position));

    }

    public void AttackRotate()
    {
     
        fTickTime += Time.deltaTime;
        
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        // transform.LookAt(PlayerTarget);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * Rotspeed);

        if (fTickTime >= fDestroyTime) 
        {
            Vector3 dir = PlayerTarget.transform.position - transform.position;
             transform.rotation = Quaternion.LookRotation(dir).normalized;
           // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime*Rotspeed);

            //transform.LookAt(PlayerTarget);
            //rb.useGravity = true;
            fTickTime = 0;
            
        }
    
        


    }


    public void Rotate()
    {
      
        Vector3 dir = WingWay[LeePoints].transform.position - transform.position;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed); 원래 이거임
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * Rotspeed);
    }

    public void Rotate1()
    {

        Vector3 dir1 = Way[WayPoints].transform.position - transform.position;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir1), Time.deltaTime * Rotspeed); 원래 이거임
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir1), Time.deltaTime * Rotspeed);
    }

    public void HomeRotate()
    {
      
        transform.position = Vector3.MoveTowards(transform.position, Homepoint.transform.position, speed * Time.deltaTime);
        Vector3 dir = Homepoint.transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
    }

    public void OnMinusHp()  //체력 감소 함수 - > Update에서 호출해주기
    {

    }

    public void OnBoxCollder()
    {
        isFireChk = true;
        bx.enabled = true;

    }

    public void OnBoxCollderNot()
    {
        //isFireChk = false;
        bx.enabled = false;
        
    }

    public void FalseFire()
    {
        isFireChk = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" &&isFireChk)  // if (other.tag == "Player" &&bx.enabled==true)
        {
           
            //Debug.Log("깨물기");
          

            //---------------이러면 불이 안됨
        }

        if (other.tag == "Player"&& !isFireChk)
        {
            
           // Debug.Log("불이야");
            
        }
       


    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDead) //죽음상태가 아닐때
        {
            if (other.tag == "Melee")
            {
            
                BloodParticle.Play();
                Debug.Log("망치");
                MonsterHp -= 10;// 
                Invoke("Blood", 1.5f);

                //피 효과 안나오게 해야함 / 피 효과 조절
                if (MonsterHp <= 0)
                {
                    state = State.DEATH;

                }

            }
        }


        if (!isDead) //죽음상태가 아닐때
        {
            if (other.tag == "Bullet")
            {

                BloodParticle.Play();
                Debug.Log("망치");
                MonsterHp -= 10;// 
                Invoke("Blood", 1.5f);

                //피 효과 안나오게 해야함 / 피 효과 조절
                if (MonsterHp <= 0)
                {
                    state = State.DEATH;

                }

            }
        }

    }
    public void Blood()
    {
        BloodParticle.Stop();
    }
   
}
