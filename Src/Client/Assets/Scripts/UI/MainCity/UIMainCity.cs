using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UIMainCity : MonoSingleton<UIMainCity>
{
    public Text avatarName;
    public Text avatarLevel;
    public Button back_Btn, test_Btn, bag_Btn, charEquip_Btn, quest_Btn, friends_Btn, guild_Btn;
    public UITeam TeamWindow;

    protected override void OnStart()
    {
        UpdateAvatar();
        back_Btn.onClick.AddListener(BackToSelect);
        bag_Btn.onClick.AddListener(OnClickBag);
        charEquip_Btn.onClick.AddListener(OnClickCharEquip);
        quest_Btn.onClick.AddListener(OnClickQeust);
        friends_Btn.onClick.AddListener(OnClickFriend);
        guild_Btn.onClick.AddListener(OnCLickGuild);
    }

    private void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    private void BackToSelect()
    {
        SceneManager.Instance.LoadScene("Start");
        UserServices.Instance.SendLeaveGame();
    }

    private void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();   
    }

    private void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();   
    }

    private void OnClickQeust()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }

    private void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    private void OnCLickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }
}
