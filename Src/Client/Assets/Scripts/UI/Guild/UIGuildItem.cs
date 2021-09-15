using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem
{
    public Text guildId, guildName, memberNum, leader;
    public Image Background;
    public Sprite normalBg, selectedBg;

    public override void onSelected(bool selected)
    {
        this.Background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NGuildInfo info;

    public void SetGuildInfo(NGuildInfo item)
    {
        this.info = item;
        if (this.guildId != null)
        {
            this.guildId.text = this.info.Id.ToString();
        }

        if (this.memberNum != null)
        {
            this.memberNum.text = this.info.memberCount.ToString();
        }

        if (this.guildName != null)
        {
            this.guildName.text = this.info.GuildName;
        }
        if (this.leader != null)
        {
            this.leader.text = this.info.leaderName;
        }
    }
}