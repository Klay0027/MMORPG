using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class UIInputBox : MonoBehaviour
{
    public Text title, message, tips;
    public Button yes_btn, no_btn;
    public InputField input;
    public Text yes_btn_title, no_btn_title;

    public delegate bool SubmitHandler(string inputText, out string tips);
    public event SubmitHandler OnSubmit;
    public UnityAction OnCancel;

    public string emptyTips;

    public void Init(string title, string message, string btnOK = "", string btnCancel = "", string emptyTips = "")
    {
        if (!string.IsNullOrEmpty(title))
        {
            this.title.text = title;
        }
        this.message.text = message;
        this.tips.text = null;
        this.OnSubmit = null;
        this.emptyTips = emptyTips;

        if (!string.IsNullOrEmpty(btnOK))
        {
            this.yes_btn_title.text = title;
        }
        if (!string.IsNullOrEmpty(btnCancel))
        {
            this.no_btn_title.text = title;
        }
        this.yes_btn.onClick.AddListener(OnClickYes);
        this.no_btn.onClick.AddListener(OnClickNo);
    }

    private void OnClickYes()
    {
        this.tips.text = "";
        if (string.IsNullOrEmpty(input.text))
        {
            this.tips.text = this.emptyTips;
            return;
        }
        if (OnSubmit != null)
        {
            string tips;
            if (!OnSubmit(this.input.text, out tips))
            {
                this.tips.text = tips;
                return;
            }
        }
        Destroy(this.gameObject);
    }
    private void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnCancel != null)
        {
            this.OnCancel();
        }
    }

}
