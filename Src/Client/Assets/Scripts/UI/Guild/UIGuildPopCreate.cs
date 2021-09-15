﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UIGuildPopCreate : UIWindow
{
    public InputField inputName;
    public InputField inputNotice;
    public Button create_btn;
    public Button close_btn;

    private void Start()
    {
        create_btn.onClick.AddListener(OnYesClick);
        close_btn.onClick.AddListener(OnCloseClick);
        GuildService.Instance.OnGuildCreateResult = OnGuildCreated;
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildCreateResult = null;
    }

    public override void OnYesClick()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            MessageBox.Show("请输入公会名称", "错误", MessageBoxType.Error);
            return;
        }
        if (inputName.text.Length < 4 || inputName.text.Length > 10)
        {
            MessageBox.Show("公会名称超出限制，请重新输入！", "错误", MessageBoxType.Error);
            return;
        }
        if (string.IsNullOrEmpty(inputNotice.text))
        {
            MessageBox.Show("请输入公会宣言", "错误", MessageBoxType.Error);
            return;
        }
        if (inputNotice.text.Length < 5 || inputName.text.Length > 50)
        {
            MessageBox.Show("公会宣言超出限制，请重新输入！", "错误", MessageBoxType.Error);
            return;
        }
        GuildService.Instance.SendGuildCreate(inputName.text, inputNotice.text);
    }

    private void OnGuildCreated(bool result)
    {
        if (result)
        {
            this.Close(WindowResult.Yes);
        }
    }
}
