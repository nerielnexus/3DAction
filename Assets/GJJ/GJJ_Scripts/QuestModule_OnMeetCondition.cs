using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public class QuestModule_OnMeetCondition : MonoBehaviour
    {
        private QuestModule_Master qmm = null;

        public QUEST_OBJECT_TYPE type;

        public void MeetCondition()
        {
            qmm.EditPacketData(this.gameObject);
            Destroy(gameObject);
        }

        private void Awake()
        {
            qmm = GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestModule_Master>()
                ?? throw new System.Exception(nameof(QuestModule_OnMeetCondition) + " - QuestModule_Master is null");
        }

        private void Update()
        {
            // 추후 조건 설정할 것 (몬스터 사망시 패킷 전송)
            if (type == QUEST_OBJECT_TYPE.KILL && Input.GetKeyDown(KeyCode.L))
                MeetCondition();
        }

        private void OnTriggerEnter(Collider other)
        {
            // 특정 위치에 도달해서 거기 있는 콜라이더를 건드리는 경우
            if (type == QUEST_OBJECT_TYPE.TRAVEL && other.gameObject.CompareTag("Player"))
                MeetCondition();
        }
    }
}
