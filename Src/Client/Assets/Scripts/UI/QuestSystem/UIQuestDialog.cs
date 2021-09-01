using UnityEngine;
using UnityEngine.UI;
using Models;
using System.Collections;
using System.Collections.Generic;
public class UIQuestDialog : UIWindow
{
    public UIQuestInfo questInfo;
    public Quest quest;
    public GameObject openButtons;
    public GameObject submitButtons;

    private Button accept_Btn, deny_Btn, submit_Btn, close_Btn;
    private void Start()
    {
        accept_Btn = openButtons.transform.GetChild(0).gameObject.GetComponent<Button>();
        deny_Btn = openButtons.transform.GetChild(1).gameObject.GetComponent<Button>();
        submit_Btn = submitButtons.transform.GetChild(0).gameObject.GetComponent<Button>();

        accept_Btn.onClick.AddListener(OnYesClick);
        deny_Btn.onClick.AddListener(OnNoClick);
        submit_Btn.onClick.AddListener(OnYesClick);
        close_Btn.onClick.AddListener(OnCloseClick);
    }
    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuest();
        if (this.quest.Info == null)
        {
            openButtons.SetActive(true);
            submitButtons.SetActive(false);
        }
        else
        {
            if (this.quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                openButtons.SetActive(false);
                submitButtons.SetActive(true);
            }
            else
            {
                openButtons.SetActive(false);
                submitButtons.SetActive(false);
            }
        }
    }

    private void UpdateQuest()
    {
        if (this.quest != null)
        {
            if (this.questInfo != null)
            {
                this.questInfo.SetQuestInfo(quest);
            }
        }
    }
}
