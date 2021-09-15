using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using Models;
using Services;
using SkillBridge.Message;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;
    public Button close_btn, expel_btn, applies_btn, promote_btn, depose_btn, transfer_btn;
    public GameObject panelAdmin;
    public GameObject panelLeader;
    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
        close_btn.onClick.AddListener(OnCloseClick);
        expel_btn.onClick.AddListener(OnClickKickout);
        applies_btn.onClick.AddListener(OnClickAppliesList);
        promote_btn.onClick.AddListener(OnClickPromote);
        depose_btn.onClick.AddListener(OnClickDepose);
        transfer_btn.onClick.AddListener(OnClickTransfer);
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    private void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
        ClearList();
        InitItems();
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }

    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();    
    }

    /// <summary>
    /// 点击打开申请列表
    /// </summary>
    private void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();      
    }

    /// <summary>
    /// 移除成员
    /// </summary>
    private void OnClickKickout()
    {
        return;
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }

        MessageBox.Show(string.Format("要将【{0}】移除公会嘛？", this.selectedItem.Info.Info.Name), "移出公会", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
        };  
    }

    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要提拔的成员！");
        }
        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方身份很尊贵了！");
            return;
        }
        MessageBox.Show(string.Format("要晋升【{0}】为公会副会长嘛？", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方无职罢免");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("会长不是你能动的");
            return;
        }
        MessageBox.Show(string.Format("要罢免【{0}】的公会职务嘛？", this.selectedItem.Info.Info.Name), "罢免职务", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要转让会长的成员");
            return;
        }
        MessageBox.Show(string.Format("要转让会长给【{0}】嘛？", this.selectedItem.Info.Info.Name), "转让会长", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }
}
