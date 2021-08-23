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
    public Button back_Btn, test_Btn, bag_Btn, charEquip_Btn;

    protected override void OnStart()
    {
        UpdateAvatar();
        back_Btn.onClick.AddListener(BackToSelect);
        test_Btn.onClick.AddListener(OnClickTest);
        bag_Btn.onClick.AddListener(OnClickBag);
        charEquip_Btn.onClick.AddListener(OnClickCharEquip);
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

    public void OnClickTest()
    {
        UITest test = UIManager.Instance.Show<UITest>();
        test.SetTitle("我是一个提示框标题", "式步枪和结束标签是不加区别苏北v去诉求不俗并且不俗把球传到和去和地区我");
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了：" + result, "对话框响应结果", MessageBoxType.Information);
    }

    private void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();   
    }

    private void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    
    }
}
