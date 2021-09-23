using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Managers;

public class UIPopCharMenu : UIWindow, IDeselectHandler
{
    public int targetId;
    public string targetName;
    public Button chat_btn, addFriend_btn, inviteTeam_btn;

    private void Start()
    {
        chat_btn.onClick.AddListener(OnChat);
        addFriend_btn.onClick.AddListener(OnAddFriend);
        inviteTeam_btn.onClick.AddListener(OnInviteTeam);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        var ed = eventData as PointerEventData;
        if (ed.hovered.Contains(this.gameObject))
        {
            return;
        }
        this.Close(WindowResult.None);
    }

    private void OnEnable()
    {
        this.GetComponent<Selectable>().Select();
        this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0);
    }

    public void OnChat() 
    {
        ChatManager.Instance.StartPrivateChat(targetId, targetName);
        this.Close(WindowResult.No);
    }

    public void OnAddFriend()
    {
        this.Close(WindowResult.No);
    }

    public void OnInviteTeam()
    {
        this.Close(WindowResult.No);
    }
}
