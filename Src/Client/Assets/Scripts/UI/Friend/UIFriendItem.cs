using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem
{
    public Text nickname, level, @class, status;
    public Image Background;
    public Sprite normalBg, selectedBg;

    public override void onSelected(bool selected)
    {
        this.Background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NFriendInfo info;
    private bool isEquiped = false;

    public void SetFriendInfo(NFriendInfo item)
    {
        this.info = item;
        if (this.nickname != null)
        {
            this.nickname.text = this.info.friendInfo.Name;
        }

        if (this.@class != null)
        {
            this.@class.text = this.info.friendInfo.Class.ToString();
        }

        if (this.level != null)
        {
            this.level.text = this.info.friendInfo.Level.ToString();
        }
        if (this.status != null)
        {
            this.status.text = this.info.Status == 1 ? "在线" : "离线";
        }
    }
}
