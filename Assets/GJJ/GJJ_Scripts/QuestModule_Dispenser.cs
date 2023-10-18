using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GJJ
{
    public class QuestModule_Dispenser : MonoBehaviour
    {
        [Header("Quest Module Items")]
        // 퀘스트 데이터베이스 (매니저 유형)
        [SerializeField] public QuestModule_Master questMaster = null;
        public string questMasterTag = null;

        // 실제로 제공할 퀘스트 Scriptable Object
        public QuestModule_Data currentQuest = null;

        // 플레이어를 탐지할 콜라이더
        public Collider detectionArea = null;
        public float detectionRadius = 3f;

        [Header("Quest Accept UI Elements")]
        // 퀘스트 수락 창 ui
        public GameObject questAcceptObject = null;
        public GameObject questAcceptPrefab = null;

        // 퀘스트 수락 창 ui 항목들
        public Text currentQuestName = null;
        public Text currentQuestDescription = null;
        public Text currentQuestObject = null;


        private bool CheckIntegrity()
        {
            if (questMaster.questAvailableInGame.Contains(currentQuest))
                return true;
            else
                return false;
        }

        private void QuestAcceptUI()
        {
            if (questAcceptObject.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                return;
            }

            currentQuestName.text = currentQuest.questName;
            currentQuestDescription.text = currentQuest.questDescription;
            currentQuestObject.text = currentQuest.questObjective;
        }

        public void OnButtonClick_QuestAccept()
        {
            Time.timeScale = 1f;
            questMaster.AddQuestToOnBoard(currentQuest);
            Debug.Log("quest added");
            questAcceptObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OnButtonClick_QuestDecline()
        {
            Time.timeScale = 1f;
            Debug.Log("quest declined");
            questAcceptObject.SetActive(false);
        }

        private void InitializeUIElements()
        {
            questAcceptObject = Instantiate(questAcceptPrefab, GameObject.Find("Canvas").transform);

            for (int i = 0; i < questAcceptObject.transform.childCount; i++)
            {
                if (questAcceptObject.transform.GetChild(i).gameObject.name == "quest name")
                    currentQuestName = questAcceptObject.transform.GetChild(i).GetComponent<Text>();

                if (questAcceptObject.transform.GetChild(i).gameObject.name == "quest description")
                    currentQuestDescription = questAcceptObject.transform.GetChild(i).GetComponent<Text>();

                if (questAcceptObject.transform.GetChild(i).gameObject.name == "quest objective")
                    currentQuestObject = questAcceptObject.transform.GetChild(i).GetComponent<Text>();

                if (questAcceptObject.transform.GetChild(i).gameObject.name == "quest accept button")
                    questAcceptObject.transform.GetChild(i).GetComponent<Button>()
                        .onClick.AddListener(() => OnButtonClick_QuestAccept());

                if (questAcceptObject.transform.GetChild(i).gameObject.name == "quest decline button")
                    questAcceptObject.transform.GetChild(i).GetComponent<Button>()
                        .onClick.AddListener(() => OnButtonClick_QuestDecline());
            }

            questAcceptObject.SetActive(false);
        }

        private void Awake()
        {
            questMaster = GameObject.FindGameObjectWithTag(questMasterTag).GetComponent<QuestModule_Master>();
            if (questMaster == null)
                throw new System.Exception(nameof(QuestModule_Dispenser) + " - questMaster is null");

            if (currentQuest == null)
                throw new System.Exception(nameof(QuestModule_Dispenser) + " - currentQuest is null");

            if (!CheckIntegrity())
                throw new System.Exception(nameof(QuestModule_Dispenser) + " - currentQuest is not listed quest");

            detectionArea = GetComponent<Collider>();

            if (detectionArea == null)
                throw new System.Exception(nameof(QuestModule_Dispenser) + " - detectionArea is null");

            (detectionArea as SphereCollider).radius = detectionRadius;

            Debug.LogWarning(this.gameObject.name + " - " + currentQuest.questName);

            InitializeUIElements();
        }

        private void Update()
        {
            QuestAcceptUI();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
                questAcceptObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
                questAcceptObject.SetActive(false);
        }
    } 
}
