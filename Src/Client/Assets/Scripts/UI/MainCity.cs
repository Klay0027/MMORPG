using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class MainCity : MonoBehaviour
{
    public Text avatarName;
    public Text avatarLevel;
    public Button back_Btn;

    private void Start()
    {
        UpdateAvatar();
        back_Btn.onClick.AddListener(BackToSelect);
        UserServices.Instance.OnGameLeave = OnPlayerLeave;
    }

    private void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    private void BackToSelect()
    {
        UserServices.Instance.SendLeaveGame();
    }

    private void OnPlayerLeave(Result result, string message)
    {
        if (result == Result.Success)
        {
            SceneManager.Instance.LoadScene("Start");
            Debug.Log("已经正常离开游戏");
        }
        else
        {
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }

    }
}
