using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Data;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] targets;
    public Text description;
    //public UIIconItem rewardItems;
    public Text rewardMoney, rewardExp;

    /// <summary>
    /// 设置任务信息
    /// </summary>
    /// <param name="quest"></param>
    public void SetQuestInfo(Quest quest)
    {
        //设置任务标题
        if (quest.Define.Type == QuestType.Main)
        {
            this.title.text ="[主线]" + quest.Define.Name;
        }
        else
        {
            this.title.text = "[支线]" + quest.Define.Name;
        }
        //this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        this.targets[0].text = quest.Define.Overview;
        //设置任务描述
        if (quest.Info == null)
        {
            this.description.text = quest.Define.Overview;
        }
        else
        {
            if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.description.text = quest.Define.DiaLogFinish;
            }
        }

        this.rewardMoney.text = "金币：" + quest.Define.RewardGold.ToString();
        this.rewardExp.text = "经验：" + quest.Define.RewardExp.ToString();

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
        MessageBox.Show("不要放弃呀！");
    }
}
