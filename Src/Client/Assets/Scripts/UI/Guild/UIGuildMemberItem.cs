using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Common.Data;
using Common.Utils;
using Managers;
using Models;
using SkillBridge.Message;

public class UIGuildMemberItem : ListView.ListViewItem
{
    public Text nickname, @class, level, title, joinTime, status;
    public Image Background;
    public Sprite normalBg, selectedBg;
    public override void onSelected(bool selected)
    {
        this.Background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NGuildMemberInfo Info;

    public void SetGuildMemberInfo(NGuildMemberInfo item)
    {
        this.Info = item;
        if (this.nickname != null) this.nickname.text = this.Info.Info.Name;
        if (this.@class != null) this.@class.text = this.Info.Info.Class.ToString();
        if (this.level != null) this.level.text = this.Info.Info.Level.ToString();
        if (this.title != null) this.title.text = this.Info.Title.ToString();
        if (this.joinTime != null) this.joinTime.text = TimeUtil.GetTime(this.Info.joinTime).ToShortDateString();
        if (this.status != null) this.status.text = this.Info.Status == 1 ? "在线" : "离线";

    }
}
