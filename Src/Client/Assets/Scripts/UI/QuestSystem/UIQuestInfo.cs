using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] targets;
    public Text description;
    public UIIconItem rewardItems;
    public Text rewardMoney, rewardExp;

    /// <summary>
    /// 设置任务信息
    /// </summary>
    /// <param name="quest"></param>
    public void SetQuestInfo(Quest quest)
    {
        //设置任务标题
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        //设置任务描述
        if (quest.Info == null)
        {
            this.description.text = quest.Define.DiaLog;
        }
        else
        {
            if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.description.text = quest.Define.DiaLogFinish;
            }
        }

        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    /// <summary>
    /// 点击放弃任务
    /// </summary>
    public void OnClickAbandon()
    { 
    
    }
}
