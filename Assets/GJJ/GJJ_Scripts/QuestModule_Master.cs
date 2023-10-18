using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace GJJ
{
    public class QuestModule_Master : MonoBehaviour
    {
        [Header("Quest Lists")]
        // 지금 플레이어가 갖고 있는 퀘스트 목록
        public List<QuestModule_Data> questOnBoard = null;

        // 플레이어가 그간 클리어한 퀘스트의 "고유 아이디" 목록
        public List<string> questCleared = null;

        // 게임에서 제공하는 퀘스트 목록
        public List<QuestModule_Data> questAvailableInGame = null;

        // 플레이어가 갖고 있는 퀘스트의 클리어 조건 목록
        public List<QuestDataPacket> questRequirementList = null;

        [Header("ETC")]
        // 플레이어가 가진 퀘스트 목록을 보여주는 창을 띄우는 키
        public KeyCode keycodeCurrentQuestTable = default;

        // 퀘스트 목록 창(quest table) 프리팹
        public GameObject questTableUI = null;

        // 퀘스트를 획득하려는 창(quest gain) 프리팹
        public GameObject questGainUI = null;

        public KeyCode keyDeleteQuest = KeyCode.P;

        // 퀘스트 클리어하면 뜨는 배너
        public GameObject questClearBanner = null;

        [Header("Quest Table UI Elements")]
        public Text questNameText = null;
        public Text questDescriptionText = null;
        public Text questObjectiveText = null;
        private int questListIter = 0;
        public Text questClearBannerText = null;
        private bool loopflag_QuestClearBanner = false;

        // ==================================================

        private void ToggleQuestTable()
        {
            if (Input.GetKeyDown(keycodeCurrentQuestTable))
            {
                if (!questTableUI.activeSelf)
                {
                    questTableUI.SetActive(true);
                }
                else
                {
                    questTableUI.SetActive(false);
                }
            }
        }

        public void AddQuestToOnBoard(QuestModule_Data qd)
        {
            if (questOnBoard.Contains(qd))
            {
                Debug.Log(nameof(AddQuestToOnBoard) + "|| " + qd.questName + " is already in OnBoard List");
                return;
            }

            if (questCleared.Contains(qd.questName))
            {
                Debug.Log(nameof(AddQuestToOnBoard) + "|| " + qd.questName + " is already done");
                return;
            }

            questOnBoard.Add(qd);
            questRequirementList.Add(CreatePacket(qd));
        }

        private void ShowQuestOnBoard()
        {
            if (questOnBoard.Count != 0)
            {
                QuestModule_Data data = questOnBoard[questListIter];

                questNameText.text = data.questName;
                questDescriptionText.text = data.questDescription;
                questObjectiveText.text = data.questObjective;
            }
            else
            {
                questNameText.text = "Not Available";
                questDescriptionText.text = "Not Available";
                questObjectiveText.text = "Not Available";
            }
        }

        public void OnButtonClick_IncreaseIter()
        {
            if (questOnBoard.Count > 1)
            {
                if (questListIter == questOnBoard.Count - 1)
                    questListIter = 0;
                else
                    questListIter++;
            }
        }

        public void OnButtonClick_DecreaseIter()
        {
            if (questOnBoard.Count > 1)
            {
                if (questListIter == 0)
                    questListIter = questOnBoard.Count - 1;
                else
                    questListIter--;
            }
        }

        // 퀘스트 클리어를 했을 때의 처리
        // questOnBoard -> questCleared 로 옮김
        // questRequirementList 에서도 제거
     
        public void OnButtonClick_DeleteQuest()
        {
            if (Input.GetKeyDown(keyDeleteQuest) && questOnBoard.Count > 0)
            {
                QuestModule_Data toDelete = questOnBoard[questListIter];
                questCleared.Add(toDelete.questName);
                questOnBoard.Remove(toDelete);
                questListIter = 0;

                foreach(QuestDataPacket packet in questRequirementList)
                {
                    if (packet.questid == toDelete.questUniqueID)
                        questRequirementList.Remove(packet);
                }
            }
        }

        private QuestDataPacket CreatePacket(QuestModule_Data data)
        {
            QuestDataPacket result = new QuestDataPacket();
            result.questid = data.questUniqueID;
            result.questtype = data.questObject;
            result.questobjective = data.questObjectTarget;
            result.questcurrentcount = 0;
            result.questrequirecount = data.questTargetCount;

            return result;
        }

        public void EditPacketData(QuestDataPacket packet)
        {
            int index = questRequirementList.FindIndex(x => x.questid == packet.questid);

            questRequirementList[index] = packet;
        }

        public void EditPacketData(GameObject obj)
        {
            List<QuestDataPacket> extracted =
                questRequirementList.FindAll(
                        x => obj.name.Contains(x.questobjective.name)
                        );

            if (extracted.Count == 0)
            {
                Debug.LogWarning("no such quest");
                return;
            }

            for (int i = 0; i < extracted.Count; i++)
            {
                QuestDataPacket tmp = extracted[i];
                tmp.questcurrentcount++;
                extracted[i] = tmp;
            }

            for(int i=0; i<questRequirementList.Count; i++)
            {
                if (questRequirementList[i] == extracted.Find(x => x.questid == questRequirementList[i].questid))
                    questRequirementList[i] = extracted.Find(x => x.questid == questRequirementList[i].questid);
            }
        }

        private void CheckQuestClearStatue()
        {
            if (questRequirementList.Count == 0)
                return;

            for(int i=0; i<questRequirementList.Count; i++)
            {
                if (questRequirementList[i].questcurrentcount == questRequirementList[i].questrequirecount)
                {
                    QuestModule_Data data = questOnBoard.Find(x => x.questUniqueID == questRequirementList[i].questid);

                    questClearBannerText.text = data.questName + " Cleared !";
                    StartCoroutine(ShowQuestClearBanner());
                    
                    questCleared.Add(data.questUniqueID);
                    questOnBoard.Remove(data);
                    questRequirementList.Remove(questRequirementList.Find(x=>x.questid == questRequirementList[i].questid));
                }
            }
        }

        private IEnumerator ShowQuestClearBanner()
        {
            if(!loopflag_QuestClearBanner)
            {
                loopflag_QuestClearBanner = true;
                questClearBanner.SetActive(true);
                yield return new WaitForSeconds(2f);
                questClearBanner.SetActive(false);
                loopflag_QuestClearBanner = false;
            }
        }

        // ==================================================

        private void Awake()
        {
            if (questTableUI == null)
                throw new System.Exception("Quest Table UI is null");

            questTableUI.SetActive(false);

            questRequirementList = new List<QuestDataPacket>();
        }

        private void Update()
        {
            /*
            if (questTableUI.activeSelf)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
            */

            ToggleQuestTable();

            ShowQuestOnBoard();

            OnButtonClick_DeleteQuest();

            CheckQuestClearStatue();
        }
    } 
}
