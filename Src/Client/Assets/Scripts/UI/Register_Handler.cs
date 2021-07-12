using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register_Handler : MonoBehaviour
{
    public GameObject Login_Panel, Register_Panel;
    public Button enterGame_Btn; //, backLogin_Btn;
    public Toggle tips_Tog;
    public InputField Number_Input, Password_Input, ConfirmPassword_Input;

    private string userName, password, confirmPwd;

    private void Start()
    {
        UserServices.Instance.OnRegister = this.OnRegister;
        enterGame_Btn.onClick.AddListener(OnEnterGame);
    }

    private void OnEnable()
    {
        Clear();
    }

    private void OnRegister(SkillBridge.Message.Result result, string msg)
    {
        MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));
    }

    /// <summary>
    /// 点击登录按钮
    /// </summary>
    private void OnEnterGame()
    {
        userName = Number_Input.text;
        password = Password_Input.text;
        confirmPwd = ConfirmPassword_Input.text;
        if (tips_Tog.isOn)
        {
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("请输入账号！");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码！");
                return;
            }
            if (string.IsNullOrEmpty(confirmPwd))
            {
                MessageBox.Show("请再次输入密码！");
                return;
            }
            if (password != confirmPwd)
            {
                MessageBox.Show("两次输入的密码不一致");
                return;
            }
            //调用发送注册请求
            UserServices.Instance.SendRegister(userName, password);
        }
        else
        {
            MessageBox.Show("请先阅读用户协议");
            Debug.Log("注册：请先阅读用户协议！");
            return;
        }
        Clear();
        StartCoroutine(OnBack());
    }

    /// <summary>
    /// 返回到登录
    /// </summary>
    private IEnumerator OnBack()
    {
        yield return new WaitForSeconds(1);
        Login_Panel.SetActive(true);
        Register_Panel.SetActive(false);
    }


    /// <summary>
    /// 清除输入框、复选框内容
    /// </summary>
    private void Clear()
    {
        Number_Input.text = "";
        Password_Input.text = "";
        ConfirmPassword_Input.text = "";
        tips_Tog.isOn = false;
    }
}
