using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : UIWindow
{
    public Text title_Text, content_Text;
    public Button close_Btn, determine_Btn;
    private void Start()
    {
        close_Btn.onClick.AddListener(OnCloseClick);
        determine_Btn.onClick.AddListener(OnYesClick);
        this.gameObject.transform.localPosition = Vector3.zero;
    }

    public void SetTitle(string titleText, string contentText)
    {
        title_Text.text = titleText;
        content_Text.text = contentText;
    }
}
