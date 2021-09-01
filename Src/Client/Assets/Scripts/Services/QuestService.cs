using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using Common;
using Network;
using UnityEngine;
using Models;
using Managers;


public class QuestService : Singleton<QuestService>, IDisposable
{
    public QuestService()
    {
        MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(this.OnQuestAccept);
        MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(this.OnQuestSubmit);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<QuestAcceptResponse>(this.OnQuestAccept);
        MessageDistributer.Instance.Unsubscribe<QuestSubmitResponse>(this.OnQuestSubmit);
    }

    /// <summary>
    /// 发送接受任务
    /// </summary>
    /// <param name="quest">接受的哪个任务</param>
    /// <returns></returns>
    public bool SendQuestAccept(Quest quest)
    {
        Debug.Log("SendQuestAccept");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.questAccept = new QuestAcceptRequest();
        message.Request.questAccept.QuestId = quest.Define.ID;
        NetClient.Instance.SendMessage(message);
        return true;
    }

    /// <summary>
    /// 收到接受任务的响应
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnQuestAccept(object sender, QuestAcceptResponse message)
    {
        Debug.LogFormat("OnQuestAccept:{0}, ERR:{1}", message.Result, message.Errormsg);
        if (message.Result == Result.Success)
        {
            QuestManager.Instance.OnQuestAccepted(message.Quest);
        }
        else
        {
            MessageBox.Show("任务接受失败", "错误", MessageBoxType.Error);
        }
    }

    /// <summary>
    /// 发送提交任务
    /// </summary>
    /// <param name="quest">提交的任务</param>
    /// <returns></returns>
    public bool SendQuestSubmit(Quest quest)
    {
        Debug.Log("SendQuestSubmit");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.questSubmit = new QuestSubmitRequest();
        message.Request.questSubmit.QuestId = quest.Define.ID;
        NetClient.Instance.SendMessage(message);
        return false;
    }

    /// <summary>
    /// 收到提交任务的响应
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnQuestSubmit(object sender, QuestSubmitResponse message)
    {
        Debug.LogFormat("OnQuestSubmit:{0}, ERR:{1}", message.Result, message.Errormsg);
        if (message.Result == Result.Success)
        {
            QuestManager.Instance.OnQuestSubmited(message.Quest);
        }
        else
        {
            MessageBox.Show("任务完成失败", "错误", MessageBoxType.Error);
        }
    }
}
