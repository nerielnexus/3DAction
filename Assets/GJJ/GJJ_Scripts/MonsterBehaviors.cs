using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public enum MONSTERTYPE
    {
        NONE = 0,
        AIROBOT,
        CHIMERA,
        TENTACLE
    };

    public enum MONSTERBEHAVIORSTATE
    {
        NONE = 0,
        IDLE,
        WALK,
        RUN,
        ATTACK,
        GUN,
        SWORD,
        HIT,
        DEAD,
        OUTOFRANGE
    };

    public enum MONSTERCHASESTATE
    {
        IDLE = 0,
        ROAM,
        AWARE,
        CHASE,
        ATTACK
    };

    public partial class MonsterBehaviors : MonoBehaviour
    {
        [Header("Monster State Machine")]
        public MONSTERTYPE enumMonsterType = MONSTERTYPE.NONE;
        public MONSTERBEHAVIORSTATE enumActionType = MONSTERBEHAVIORSTATE.NONE;
        public MONSTERCHASESTATE enumChaseType = MONSTERCHASESTATE.IDLE;
        public GameObject targetPlayer = null;
        public GameObject initialSpawnLocation = null;
        public int chanceToRoam = 45;
        public float roamMultiplier = 4.5f;
        public float roamTime = 3.0f;
        private bool loopflag_RoamPoint = false;
        private bool loopflag_ChanceToRoam = false;
        public GameObject roamPoint = null;
        private Animator animCon = null;
        private Rigidbody rb = null;
        private bool hasReached = true;
        public string groundTag = "Ground";
        public bool isMonsterDead = false;
        private bool isSpawnpointSetted = false;


        [Header("Monster Status")]
        public int health = 100;
        public float swordDamage = 10;

        [Header("Variables for Monster-Using-Gun")]
        public float gunDamageMultiplier = 1.5f;
        public int chanceToUseGun = 35;
        public bool isUsingGun = false;
        public Transform muzzleflashPosition = null;
        public GameObject muzzleflashEffect = null;
        private bool loopflag_PlayMuzzleFlashEffect = false;
        public float animRuntimeMuzzleFlashEffect = 1.4f;

        [Header("Monster Specs")]
        public float moveSpeed = 4f;
        public float moveSpeedRunModifier = 1.3f;
        public float rotateSpeed = 100f;
        public float gravityValue = 14f;
        public float distanceAware = 40f;
        public float distanceChase = 10f;   // may use for get distance of gun shoot
        public float distanceSword = 3f;
        public float distanceMinimum = 0.1f;

        [Header("Check isGrounded")]
        private Ray groundedRay;
        private RaycastHit hit;
        private float distIsGrounded = 1.0f;
        public bool isGrounded = false;

        [Header("Monster Damage Trigger")]
        public MonsterDamageTrigger mdt = null;

        [Header("TEST")]
        public KeyCode ImitateMonsterDead = KeyCode.L;
    } 
}
