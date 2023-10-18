using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJJ
{
    public enum QUEST_OBJECT_TYPE
    {
        TRAVEL,
        SEARCH,
        KILL
    }

    [Serializable]
    public class QuestDataPacket
    {
        public string questid;
        public QUEST_OBJECT_TYPE questtype;
        public GameObject questobjective;
        public int questcurrentcount;
        public int questrequirecount;

        public override bool Equals(object obj)
        {
            return obj is QuestDataPacket packet &&
                   questid == packet.questid;
        }

        public override int GetHashCode()
        {
            return 1106445000 + EqualityComparer<string>.Default.GetHashCode(questid);
        }

        public static bool operator ==(QuestDataPacket left, QuestDataPacket right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(QuestDataPacket left, QuestDataPacket right)
        {
            return !(left == right);
        }

        public QuestDataPacket Clone()
        {
            QuestDataPacket result = new QuestDataPacket();

            result.questid = this.questid;
            result.questtype = this.questtype;
            result.questobjective = this.questobjective;
            result.questcurrentcount = this.questcurrentcount;
            result.questrequirecount = this.questrequirecount;

            return result;
        }
    }

    [CreateAssetMenu(fileName = "Quest Data", menuName = "Quest/Quest Data")]
    public class QuestModule_Data : ScriptableObject
    {
        // 퀘스트의 고유 아이디
        [SerializeField] public string questUniqueID;

        // 유저가 보는 퀘스트 이름
        [SerializeField] public string questName;

        // 유저가 보는 퀘스트 설명
        [SerializeField] public string questDescription;

        // 유저가 보는 퀘스트 목표, 텍스트로 정리한 내용
        [SerializeField] public string questObjective;

        // 유저가 보는 '이 퀘스트' 의 선행 퀘스트 = 선행 조건
        [SerializeField] public string questBefore;

        // 이 퀘스트의 목적
        // 어디로 이동하기(travel), 무언가를 찾기(search), 적을 죽이기(kill)
        [SerializeField] public QUEST_OBJECT_TYPE questObject;

        // 퀘스트의 목적 대상 (이동할 지점, 찾을 물건, 죽일 적 등)
        [SerializeField] public GameObject questObjectTarget;

        // 퀘스트의 목적 카운트 (ex. questTargetCount 만큼의 적을 죽이시오)
        [SerializeField] public int questTargetCount;
    } 
}
