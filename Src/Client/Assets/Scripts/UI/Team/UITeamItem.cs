﻿using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;
public class UITeamItem : ListView.ListViewItem
{
    public Text nickname;
    public Image classIcon;
    public Image leaderIcon;
    public Image background;

    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? true : false;
    }

    public int idx;

    public NCharacterInfo info;

    private void Start()
    {
        this.background.enabled = false;
    }

    public void SetMemberInfo(int idx, NCharacterInfo item, bool isLeader)
    {
        this.idx = idx;
        this.info = item;
        if (this.nickname != null)
        {
            this.nickname.text = this.info.Level.ToString().PadRight(4) + this.info.Name;
        }
        if (this.classIcon != null)
        {
            this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.info.Class - 1];
        }
        if (this.leaderIcon != null)
        {
            this.leaderIcon.gameObject.SetActive(isLeader);
        }
    }
}