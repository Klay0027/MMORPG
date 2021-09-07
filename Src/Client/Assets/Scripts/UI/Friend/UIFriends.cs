using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Models;
using Services;
using System;
using UnityEngine.UI;

public class UIFriends : UIWindow
{
    public GameObject itemPrefab;
    public ListView list;
    public Transform itemRoot;
    public UIFriendItem selectedItem;
    public Button addFriend_Btn, delFriend_Btn, invite_Btn, chat_Btn, close_Btn;

    private void Start()
    {
        FriendService.Instance.OnFriendUpdate = RefreshUI;
        this.list.onItemSelected += this.OnFriendSelected;
        RefreshUI();
        addFriend_Btn.onClick.AddListener(OnClickFriendAdd);
        delFriend_Btn.onClick.AddListener(OnClickFriendRemove);
        close_Btn.onClick.AddListener(OnCloseClick);
        invite_Btn.onClick.AddListener(OnClickFriendTeamInvite);
    }

    public void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;
    }

    public void OnClickFriendAdd()
    {
        InputBox.Show("输入要添加好友的昵称或ID", "添加好友").OnSubmit += OnFriendAddSubmit;    
    }

    private bool OnFriendAddSubmit(string input, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        if (!int.TryParse(input, out friendId))
        {
            friendName = input;
        }
        if (friendId == User.Instance.CurrentCharacter.Id || friendName == User.Instance.CurrentCharacter.Name)
        {
            tips = "开玩笑嘛！不能添加自己噢！";
            return false;
        }

        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }

    public void OnClickFriendChat()
    { 
    
    }

    public void OnClickFriendTeamInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要邀请的好友！");
            return;
        }
        if (selectedItem.info.Status == 0)
        {
            MessageBox.Show("请选择在线的好友！");
            return;
        }
        MessageBox.Show(string.Format("确定要邀请好友【{0}】吗？", selectedItem.info.friendInfo.Name), "邀请好友组队", MessageBoxType.Confirm, "删除", "取消").OnYes = () => {
            TeamService.Instance.SendTeamInviteRequest(this.selectedItem.info.Id, this.selectedItem.info.friendInfo.Name);
        };
    }

    public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要删除的好友！");
            return;
        }
        MessageBox.Show(string.Format("确定要删除好友【{0}】吗？", selectedItem.info.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes = () => { 
        FriendService.Instance.SendFriendRemoveRequest(this.selectedItem.info.Id, this.selectedItem.info.friendInfo.Id);
        };
    }

    private void RefreshUI()
    {
        ClearFriendList();
        InitFriendItems();
    }

    /// <summary>
    /// 初始化所有的好友列表
    /// </summary>
    private void InitFriendItems()
    {
        foreach (var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefab, this.list.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.list.AddItem(ui);
        }
    }

    private void ClearFriendList()
    {
        this.list.RemoveAll();
    }
}
