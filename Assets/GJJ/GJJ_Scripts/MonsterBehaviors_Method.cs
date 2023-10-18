using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public partial class MonsterBehaviors : MonoBehaviour
    {
        private void Chimera_SetAnimatorTreeArguments()
        {
            // anim params list
            // AnimIdle AnimRun AnimWalk AnimGun AnimSword AnimDead OutOfRange

            if (enumActionType == MONSTERBEHAVIORSTATE.IDLE)
            {
                animCon.SetBool("AnimIdle", true);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimRun", false);
                animCon.SetBool("AnimSword", false);
                animCon.SetBool("AnimGun", false);
                animCon.SetBool("AnimDead", false);
                animCon.SetBool("OutOfRange", false);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
            {
                if (animCon.GetBool("AnimIdle")) animCon.SetBool("AnimIdle", false);

                if (animCon.GetBool("AnimRun")) animCon.SetBool("AnimRun", false);

                if (animCon.GetBool("AnimSword") || animCon.GetBool("AnimGun"))
                {
                    animCon.SetBool("AnimSword", false);
                    animCon.SetBool("AnimGun", false);
                }

                animCon.SetBool("AnimWalk", true);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.RUN)
            {
                if (animCon.GetBool("AnimIdle")) animCon.SetBool("AnimIdle", false);

                if (animCon.GetBool("AnimWalk")) animCon.SetBool("AnimWalk", false);

                animCon.SetBool("AnimRun", true);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.SWORD)
            {
                animCon.SetBool("AnimIdle", false);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimRun", false);
                animCon.SetBool("AnimSword", true);
                animCon.SetBool("AnimGun", false);
                animCon.SetBool("AnimDead", false);
                animCon.SetBool("OutOfRange", false);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.GUN)
            {
                animCon.SetBool("AnimIdle", false);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimRun", false);
                animCon.SetBool("AnimSword", false);
                animCon.SetBool("AnimGun", true);
                animCon.SetBool("AnimDead", false);
                animCon.SetBool("OutOfRange", false);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
            {
                animCon.SetBool("AnimIdle", false);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimRun", false);
                animCon.SetBool("AnimSword", false);
                animCon.SetBool("AnimGun", false);
                animCon.SetBool("AnimDead", true);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.OUTOFRANGE)
            {
                if (animCon.GetBool("AnimSword") || animCon.GetBool("AnimGun"))
                {
                    animCon.SetBool("AnimSword", false);
                    animCon.SetBool("AnimGun", false);
                }

                animCon.SetBool("OutOfRange", true);
            }
        }

        private void Chimera_SetBehaviorState()
        {
            if (targetPlayer == null)
            {
                StartCoroutine(ChanceToRoam());
                return;
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
                return;

            float _dist = Vector3.Distance(transform.position, targetPlayer.transform.position);

            if (_dist > distanceAware)
                StartCoroutine(ChanceToRoam());

            if (_dist <= distanceAware && _dist > distanceChase)
                enumChaseType = MONSTERCHASESTATE.AWARE;

            if (_dist <= distanceChase && _dist > distanceSword)
                if (!isUsingGun)
                    enumChaseType = MONSTERCHASESTATE.CHASE;
                else
                    enumChaseType = MONSTERCHASESTATE.ATTACK;

            if (_dist <= distanceSword && !isUsingGun)
                enumChaseType = MONSTERCHASESTATE.ATTACK;
        }

        private void Chimera_Behaviors()
        {
            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
                return;

            if (enumChaseType == MONSTERCHASESTATE.IDLE)
            {
                float _dist = Vector3.Distance(transform.position, initialSpawnLocation.transform.position);
                Vector3 moveDirection = initialSpawnLocation.transform.position - transform.position;

                transform.LookAt(initialSpawnLocation.transform.position);

                if (_dist <= distanceMinimum)
                    enumActionType = MONSTERBEHAVIORSTATE.IDLE;
                else
                    enumActionType = MONSTERBEHAVIORSTATE.WALK;

                if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                    rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);

                StartCoroutine(ChanceToRoam());
            }

            if (enumChaseType == MONSTERCHASESTATE.ROAM)
            {
                StartCoroutine(ChangeRoamPoint());

                if (roamPoint != null)
                {
                    float _dist = Vector2.Distance(transform.position, roamPoint.transform.position);
                    Vector3 moveDirection = roamPoint.transform.position - transform.position;

                    // transform.LookAt(roamPoint.transform.position);

                    transform.LookAt(new Vector3(
                        roamPoint.transform.position.x,
                        transform.position.y,
                        roamPoint.transform.position.z
                        ));

                    if (_dist <= distanceMinimum)
                    {
                        enumActionType = MONSTERBEHAVIORSTATE.IDLE;
                        hasReached = true;
                    }
                    else
                        enumActionType = MONSTERBEHAVIORSTATE.WALK;

                    if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                        rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
                }

            }

            if (enumChaseType == MONSTERCHASESTATE.AWARE)
            {
                float _dist = Vector3.Distance(transform.position, targetPlayer.transform.position);
                Vector3 moveDirection = targetPlayer.transform.position - transform.position;

                transform.LookAt(new Vector3(targetPlayer.transform.position.x,
                    transform.position.y,
                    targetPlayer.transform.position.z));

                if (_dist <= distanceAware && _dist > distanceChase)
                    enumActionType = MONSTERBEHAVIORSTATE.WALK;

                if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                    rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);

                if (targetPlayer == null)
                    enumChaseType = MONSTERCHASESTATE.IDLE;
            }

            if (enumChaseType == MONSTERCHASESTATE.CHASE)
            {
                Vector3 moveDirection = targetPlayer.transform.position - transform.position;

                transform.LookAt(new Vector3(targetPlayer.transform.position.x,
                    transform.position.y,
                    targetPlayer.transform.position.z));

                isUsingGun = (Random.Range(0, 101) <= chanceToUseGun);

                if (!isUsingGun)
                {
                    enumActionType = MONSTERBEHAVIORSTATE.RUN;
                    rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * moveSpeedRunModifier * Time.fixedDeltaTime);
                }
                else
                    enumChaseType = MONSTERCHASESTATE.ATTACK;

                if (targetPlayer == null)
                    enumChaseType = MONSTERCHASESTATE.IDLE;
            }

            if (enumChaseType == MONSTERCHASESTATE.ATTACK)
            {
                transform.LookAt(new Vector3(targetPlayer.transform.position.x,
                    transform.position.y,
                    targetPlayer.transform.position.z));

                if (!isUsingGun)
                    enumActionType = MONSTERBEHAVIORSTATE.SWORD;
                else
                {
                    enumActionType = MONSTERBEHAVIORSTATE.GUN;
                    StartCoroutine(PlayMuzzleFlashEffect());
                }

                if (targetPlayer == null)
                    enumChaseType = MONSTERCHASESTATE.IDLE;
            }
        }

        private void Tentacle_SetAnimatorTreeArguments()
        {
            // anim-param: idle walk run hit attack dead

            if (enumActionType == MONSTERBEHAVIORSTATE.IDLE)
            {
                animCon.SetBool("AnimIdle", true);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimRun", false);
                animCon.SetBool("AnimAttack", false);
                animCon.SetBool("AnimHit", false);
                animCon.SetBool("AnimDead", false);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
            {
                if (animCon.GetBool("AnimIdle")) animCon.SetBool("AnimIdle", false);

                if (animCon.GetBool("AnimRun")) animCon.SetBool("AnimRun", false);

                if (animCon.GetBool("AnimAttack")) animCon.SetBool("AnimAttack", false);

                animCon.SetBool("AnimWalk", true);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.RUN)
            {
                if (animCon.GetBool("AnimIdle")) animCon.SetBool("AnimIdle", false);

                if (animCon.GetBool("AnimWalk")) animCon.SetBool("AnimWalk", false);

                animCon.SetBool("AnimRun", true);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.ATTACK)
            {
                animCon.SetBool("AnimIdle", false);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimRun", false);
                animCon.SetBool("AnimAttack", true);
                animCon.SetBool("AnimHit", false);
                animCon.SetBool("AnimDead", false);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.HIT)
            {
                animCon.SetBool("AnimIdle", false);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimRun", false);
                animCon.SetBool("AnimAttack", false);
                animCon.SetBool("AnimHit", true);
                animCon.SetBool("AnimDead", false);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
            {
                animCon.SetBool("AnimIdle", false);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimRun", false);
                animCon.SetBool("AnimAttack", false);
                animCon.SetBool("AnimHit", false);
                animCon.SetBool("AnimDead", true);
            }
        }

        private void Tentacle_SetBehaviorState()
        {
            if (targetPlayer == null)
            {
                StartCoroutine(ChanceToRoam());
                return;
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
                return;

            float _dist = Vector3.Distance(transform.position, targetPlayer.transform.position);

            if (_dist > distanceAware)
                StartCoroutine(ChanceToRoam());

            if (_dist <= distanceAware && _dist > distanceChase)
                enumChaseType = MONSTERCHASESTATE.AWARE;

            if (_dist <= distanceChase && _dist > distanceSword)
                if (!isUsingGun)
                    enumChaseType = MONSTERCHASESTATE.CHASE;
                else
                    enumChaseType = MONSTERCHASESTATE.ATTACK;

            if (_dist <= distanceSword && !isUsingGun)
                enumChaseType = MONSTERCHASESTATE.ATTACK;
        }

        private void Tentacle_Behaviors()
        {
            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
                return;

            if (enumChaseType == MONSTERCHASESTATE.IDLE)
            {
                float _dist = Vector3.Distance(transform.position, initialSpawnLocation.transform.position);
                Vector3 moveDirection = initialSpawnLocation.transform.position - transform.position;

                transform.LookAt(initialSpawnLocation.transform.position);

                if (_dist <= distanceMinimum)
                    enumActionType = MONSTERBEHAVIORSTATE.IDLE;
                else
                    enumActionType = MONSTERBEHAVIORSTATE.WALK;

                if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                    rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);

                StartCoroutine(ChanceToRoam());
            }

            if (enumChaseType == MONSTERCHASESTATE.ROAM)
            {
                StartCoroutine(ChangeRoamPoint());

                if (roamPoint != null)
                {
                    float _dist = Vector2.Distance(transform.position, roamPoint.transform.position);
                    Vector3 moveDirection = roamPoint.transform.position - transform.position;

                    // transform.LookAt(roamPoint.transform.position);

                    transform.LookAt(new Vector3(
                        roamPoint.transform.position.x,
                        transform.position.y,
                        roamPoint.transform.position.z
                        ));

                    if (_dist <= distanceMinimum)
                    {
                        enumActionType = MONSTERBEHAVIORSTATE.IDLE;
                        hasReached = true;
                    }
                    else
                        enumActionType = MONSTERBEHAVIORSTATE.WALK;

                    if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                        rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
                }

            }

            if (enumChaseType == MONSTERCHASESTATE.AWARE)
            {
                float _dist = Vector3.Distance(transform.position, targetPlayer.transform.position);
                Vector3 moveDirection = targetPlayer.transform.position - transform.position;

                transform.LookAt(new Vector3(targetPlayer.transform.position.x,
                    transform.position.y,
                    targetPlayer.transform.position.z));

                if (_dist <= distanceAware && _dist > distanceChase)
                    enumActionType = MONSTERBEHAVIORSTATE.WALK;

                if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                    rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);

                if (targetPlayer == null)
                    enumChaseType = MONSTERCHASESTATE.IDLE;
            }

            if (enumChaseType == MONSTERCHASESTATE.CHASE)
            {
                Vector3 moveDirection = targetPlayer.transform.position - transform.position;

                transform.LookAt(new Vector3(targetPlayer.transform.position.x,
                    transform.position.y,
                    targetPlayer.transform.position.z));

                enumActionType = MONSTERBEHAVIORSTATE.RUN;
                rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * moveSpeedRunModifier * Time.fixedDeltaTime);

                if (targetPlayer == null)
                    enumChaseType = MONSTERCHASESTATE.IDLE;
            }

            if (enumChaseType == MONSTERCHASESTATE.ATTACK)
            {
                transform.LookAt(new Vector3(targetPlayer.transform.position.x,
                    transform.position.y,
                    targetPlayer.transform.position.z));

                enumActionType = MONSTERBEHAVIORSTATE.ATTACK;

                if (targetPlayer == null)
                    enumChaseType = MONSTERCHASESTATE.IDLE;
            }
        }

        private void Robot_SetAnimatorTreArguments()
        {
            // anim-param: idle walk hit dead

            if (enumActionType == MONSTERBEHAVIORSTATE.IDLE)
            {
                animCon.SetBool("AnimIdle", true);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimHit", false);
                animCon.SetBool("AnimDead", false);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
            {
                if (animCon.GetBool("AnimIdle")) animCon.SetBool("AnimIdle", false);

                animCon.SetBool("AnimWalk", true);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.HIT)
            {
                animCon.SetBool("AnimIdle", false);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimHit", true);
                animCon.SetBool("AnimDead", false);
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
            {
                animCon.SetBool("AnimIdle", false);
                animCon.SetBool("AnimWalk", false);
                animCon.SetBool("AnimHit", false);
                animCon.SetBool("AnimDead", true);
            }
        }

        private void Robot_SetBehaviorState()
        {
            if (targetPlayer == null)
            {
                StartCoroutine(ChanceToRoam());
                return;
            }

            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
                return;

            float _dist = Vector3.Distance(transform.position, targetPlayer.transform.position);

            /*  robot 의 거리 재기
             *  (a) _dist 값을 구함
             *  (b) (targetPlayer.transform.position - transform.position).normalize 를 구함
             *      이건 몬스터가 플레이어를 향해 가는 것
             *  (c) b 의 벡터를 뒤집어서 도망가는 방향을 구함
             *          - y axis 까지 뒤집을 필요는 없음
             *          - 결국 y axis 를 기준으로 xz벡터를 180 degree 돌리는거네?
             *  (d) c 에 (1-a) 의 값을 곱해 도망가는 방향과 힘을 구함
             *  (e) d 를 따라 몬스터를 움직이게 함 (= 도망가는 모션)
             *  (f) e 에 적당한 변형을 가해 평생 도망가서 못잡는 상황을 방지할 것
             *          - moveSpeedRunModifier 를 사용해서 데바데 데드하드 쓰는 느낌으로 일정 시간 속도 부스팅해주기
             *          - 대신 moveSpeed 값은 다른 몬스터보다 낮게
             */

            if (_dist > distanceAware)
                StartCoroutine(ChanceToRoam());

            if (_dist <= distanceAware && _dist > distanceChase)
                enumChaseType = MONSTERCHASESTATE.AWARE;

            if (_dist <= distanceChase)
                enumChaseType = MONSTERCHASESTATE.CHASE;
        }

        private void Robot_Behaviors()
        {
            if (enumActionType == MONSTERBEHAVIORSTATE.DEAD)
                return;

            if(enumChaseType == MONSTERCHASESTATE.IDLE)
            {
                float _dist = Vector3.Distance(transform.position, initialSpawnLocation.transform.position);
                Vector3 moveDirection = initialSpawnLocation.transform.position - transform.position;

                transform.LookAt(initialSpawnLocation.transform.position);

                if (_dist <= distanceMinimum)
                    enumActionType = MONSTERBEHAVIORSTATE.IDLE;
                else
                    enumActionType = MONSTERBEHAVIORSTATE.WALK;

                if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                    rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);

                StartCoroutine(ChanceToRoam());
            }

            if(enumChaseType == MONSTERCHASESTATE.ROAM)
            {
                StartCoroutine(ChangeRoamPoint());

                if (roamPoint != null)
                {
                    float _dist = Vector2.Distance(transform.position, roamPoint.transform.position);
                    Vector3 moveDirection = roamPoint.transform.position - transform.position;

                    // transform.LookAt(roamPoint.transform.position);

                    transform.LookAt(new Vector3(
                        roamPoint.transform.position.x,
                        transform.position.y,
                        roamPoint.transform.position.z
                        ));

                    if (_dist <= distanceMinimum)
                    {
                        enumActionType = MONSTERBEHAVIORSTATE.IDLE;
                        hasReached = true;
                    }
                    else
                        enumActionType = MONSTERBEHAVIORSTATE.WALK;

                    if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                        rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
                }
            }

            if(enumChaseType == MONSTERCHASESTATE.AWARE)
            {
                float _dist = Vector3.Distance(transform.position, targetPlayer.transform.position);
                // Vector3 moveDirection = targetPlayer.transform.position - transform.position;
                Vector3 moveDirection = transform.position - targetPlayer.transform.position;

                transform.LookAt(new Vector3(targetPlayer.transform.position.x * -1f,
                    transform.position.y,
                    targetPlayer.transform.position.z * -1f));

                if (_dist <= distanceAware && _dist > distanceChase)
                    enumActionType = MONSTERBEHAVIORSTATE.WALK;

                if (enumActionType == MONSTERBEHAVIORSTATE.WALK)
                    rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);

                if (targetPlayer == null)
                    enumChaseType = MONSTERCHASESTATE.IDLE;
            }

            if(enumChaseType == MONSTERCHASESTATE.CHASE)
            {
                // Vector3 moveDirection = targetPlayer.transform.position - transform.position;
                Vector3 moveDirection = transform.position - targetPlayer.transform.position;

                transform.LookAt(new Vector3(targetPlayer.transform.position.x * -1f,
                    transform.position.y,
                    targetPlayer.transform.position.z * -1f));

                enumActionType = MONSTERBEHAVIORSTATE.WALK;
                rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * moveSpeedRunModifier * Time.fixedDeltaTime);

                if (targetPlayer == null)
                    enumChaseType = MONSTERCHASESTATE.IDLE;
            }
        }

        // =================================================================

        private IEnumerator ChanceToRoam()
        {
            if(!loopflag_ChanceToRoam)
            {
                loopflag_ChanceToRoam = true;

                int chance = Random.Range(0, 101);

                if(chance <= chanceToRoam)
                    enumChaseType = MONSTERCHASESTATE.ROAM;
                else
                    enumChaseType = MONSTERCHASESTATE.IDLE;

                yield return new WaitForSeconds(roamTime);

                loopflag_ChanceToRoam = false;
            }
        }

        private IEnumerator ChangeRoamPoint()
        {
            if(!loopflag_RoamPoint)
            {
                loopflag_RoamPoint = true;

                if (isGrounded && hasReached && initialSpawnLocation != null)
                {
                    if (roamPoint == null)
                        roamPoint = new GameObject(gameObject.name + " roam point");

                    roamPoint.transform.position = GetRoamPointVector();

                    hasReached = false;

                    yield return new WaitForSeconds(roamTime);
                }
                else if(isGrounded && !hasReached && initialSpawnLocation != null)
                {
                    roamPoint.transform.position = GetRoamPointVector();
                    yield return new WaitForSeconds(roamTime);
                }
                else
                    yield return null;

                loopflag_RoamPoint = false;
            }
        }

        private Vector3 GetRoamPointVector()
        {
            Vector3 result = Vector3.zero;
            float modifier = Random.Range(-roamMultiplier, roamMultiplier);

            if (initialSpawnLocation.transform.position.x == 0f)
                result += new Vector3(1, 0, 0);

            if (initialSpawnLocation.transform.position.z == 0f)
                result += new Vector3(0, 0, 1);

            result += initialSpawnLocation.transform.position;
            result *= modifier;
            result.y = transform.position.y + 1f;

            // Debug.Log(result + ", " + modifier);

            return result;
        }

        private bool DrawRayToCheckGrounded()
        {
            groundedRay.origin = transform.position;
            groundedRay.direction = transform.up * -1f;

            Physics.Raycast(groundedRay, out hit, distIsGrounded);
            bool result = false;

            try
            {
                if (hit.collider.gameObject.CompareTag(groundTag))
                    result = true;
                else
                    result = false;
            }
            catch (System.Exception)
            {

            }

            return result;
        }

        private void SetInitialSpawnpoint()
        {
            if (!isGrounded)
                return;

            if (isSpawnpointSetted)
                return;

            try
            {
                if (initialSpawnLocation == null)
                {
                    initialSpawnLocation = new GameObject(gameObject.name + " initial spawn point");
                    initialSpawnLocation.transform.position = transform.position;
                }
                else
                {
                    Vector3 tmp = new Vector3(
                        initialSpawnLocation.transform.position.x,
                        transform.position.y,
                        initialSpawnLocation.transform.position.z
                        );

                    initialSpawnLocation.transform.position = tmp;
                }
            }
            catch (System.Exception)
            {

            }

            isSpawnpointSetted = true;

            return;
        }

        private IEnumerator PlayMuzzleFlashEffect()
        {
            if(!loopflag_PlayMuzzleFlashEffect)
            {
                loopflag_PlayMuzzleFlashEffect = true;

                Destroy(Instantiate(muzzleflashEffect, muzzleflashPosition.position, muzzleflashPosition.transform.localRotation),animRuntimeMuzzleFlashEffect);
                mdt.DamageToPlayer(targetPlayer);
                yield return new WaitForSeconds(animRuntimeMuzzleFlashEffect);

                loopflag_PlayMuzzleFlashEffect = false;
            }
        }

        // ============================================================
        private void Awake()
        {
            if (enumMonsterType == MONSTERTYPE.NONE)
                throw new System.Exception("Current Monster Type is NONE, please select monster type.");

            animCon = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            groundedRay = new Ray(transform.position, transform.up * -1f);

            isUsingGun = (enumMonsterType == MONSTERTYPE.CHIMERA) && Random.Range(1, 101) <= chanceToUseGun * 100;

            if (mdt == null && enumMonsterType != MONSTERTYPE.AIROBOT)
                throw new System.Exception("[GJJ Exception] MonsterDamageTrigger component is null");
        }

        private void Start()
        {

        }

        private void Update()
        {
            if(!isSpawnpointSetted)
                SetInitialSpawnpoint();

            if (health <= 0)
            {
                isMonsterDead = true;
                enumActionType = MONSTERBEHAVIORSTATE.DEAD;
                GetComponent<QuestModule_OnMeetCondition>().MeetCondition();
                Destroy(gameObject, 2f);
            }
                

            if (enumMonsterType == MONSTERTYPE.CHIMERA)
            {
                Chimera_SetAnimatorTreeArguments();
                Chimera_SetBehaviorState();
                Chimera_Behaviors();
            }

            if (enumMonsterType == MONSTERTYPE.TENTACLE)
            {
                Tentacle_SetAnimatorTreeArguments();
                Tentacle_SetBehaviorState();
                Tentacle_Behaviors();
            }

            if(enumMonsterType == MONSTERTYPE.AIROBOT)
            {
                Robot_SetAnimatorTreArguments();
                Robot_SetBehaviorState();
                Robot_Behaviors();
            }
        }

        private void FixedUpdate()
        {
            isGrounded = DrawRayToCheckGrounded();

            Debug.DrawRay(transform.position, transform.forward * distanceAware, Color.red);
            Debug.DrawRay(groundedRay.origin, groundedRay.direction, Color.red);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bullet"))
                health -= other.gameObject.GetComponent<Bullet>().damage;

            if (other.gameObject.CompareTag("Melee"))
                health -= other.gameObject.GetComponent<Weapon>().damage;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
                isGrounded = true;
            else
                isGrounded = false;
        }

        private void OnTriggerExit(Collider other)
        {
        }
    } 
}
