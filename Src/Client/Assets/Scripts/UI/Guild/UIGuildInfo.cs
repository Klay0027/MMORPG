using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Common;

public class UIGuildInfo : MonoBehaviour
{
    public Text guildName, guildID, leader, notice, memberNumber;
    private NGuildInfo info;
    public NGuildInfo Info
    {
        get { return this.info; }
        set { this.info = value; this.UpdateUI(); }
    }

    private void UpdateUI()
    {
        if (this.info == null)
        {
            this.guildName.text = "None";
            this.guildID.text = "None";
            this.leader.text = "None";
            this.notice.text = "None";
            this.memberNumber.text = "None";
        }
        else
        {
            this.guildName.text = this.Info.GuildName;
            this.guildID.text = this.info.Id.ToString();
            this.leader.text = this.info.leaderName;
            this.notice.text = this.Info.Notice;
            this.memberNumber.text = string.Format("成员数量：{0}/{1}",this.info.memberCount, 100);
        }
    }
}
