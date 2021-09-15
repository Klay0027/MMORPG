using Common.Data;
using Common.Utils;
using Managers;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIGuildApplyItem : ListView.ListViewItem
{
    public Text nickname, @class, level;
    public NGuildApplyInfo Info;
    public Button yes_btn, no_btn;

    public void SetItemInfo(NGuildApplyInfo item)
    {
        this.Info = item;
        if (this.nickname != null) this.nickname.text = this.Info.Name;
        if (this.@class != null) this.@class.text = this.Info.Class.ToString();
        if (this.nickname != null) this.nickname.text = this.Info.Name;
        yes_btn.onClick.AddListener(OnAccept);
        no_btn.onClick.AddListener(OnDecline);
    }

    public void OnAccept()
    { 
        MessageBox.Show(string.Format("要通过{0}的公会申请嘛？",this.Info.Name), "审批申请", MessageBoxType.Confirm, "同意加入", "取消").OnYes = () => 
        {
            GuildService.Instance.SendGuildJoinApply(true, this.Info);
        };
    }
    public void OnDecline()
    {
        MessageBox.Show(string.Format("要拒绝{0}的公会申请嘛？", this.Info.Name), "审批申请", MessageBoxType.Confirm, "拒绝加入", "取消").OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(false, this.Info);
        };
    }
}
