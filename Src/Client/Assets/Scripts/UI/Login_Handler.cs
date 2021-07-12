using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class Login_Handler : MonoBehaviour
{
    public GameObject Login_Panel, Register_Panel;
    public Button Register_Btn, Login_Btn;
    public Toggle remberNumber_Tog, tips_Tog;
    public InputField Number_Input, Password_Input;

    private string userName, password;

    private void Start()
    {
        Login_Btn.onClick.AddListener(OnLoginClick);
        Register_Btn.onClick.AddListener(OnRegisterClick);

        UserServices.Instance.OnLogin = this.OnLogin;

    }

    private void OnEnable()
    {
        Clear();
    }

    private void OnLogin(SkillBridge.Message.Result result, string msg)
    {
        MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));  
    }

    /// <summary>
    /// 点击登录按钮
    /// </summary>
    private void OnLoginClick()
    {
        userName = Number_Input.text;
        password = Password_Input.text;
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
            UserServices.Instance.SendLogin(userName, password);
        }
        else 
        {
            MessageBox.Show("请确认您已阅读过用户协议！");
            Debug.Log("请确认您已阅读过用户协议");
            return;
        }
        Clear();
    }

    /// <summary>
    /// 点击注册按钮
    /// </summary>
    private void OnRegisterClick()
    {
        Login_Panel.SetActive(false);
        Register_Panel.SetActive(true);
    }


    /// <summary>
    /// 清除输入框、复选框内容
    /// </summary>
    private void Clear()
    {
        Number_Input.text = "";
        Password_Input.text = "";
        remberNumber_Tog.isOn = false;
        tips_Tog.isOn = false;
    }
}
