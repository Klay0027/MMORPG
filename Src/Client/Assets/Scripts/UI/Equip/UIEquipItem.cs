using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Common.Data;
using Managers;
using SkillBridge.Message;

public class UIEquipItem : MonoBehaviour, IPointerClickHandler
{
    public Image icon, background;
    public Text title, level, limitClass, limitCategory;
    public Sprite normalBg, selectedBg;

    private bool selected;
    public bool Selected
    {
        get { return selected; }
        set 
        {
            selected = value;
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
    }

    public int index { get; set; }
    private UICharEquip owner;
    private Item item;

    private bool isEquiped = false; //代表是装备列表还是非装备列表



    public void SetEquipItem(int index, Item item, UICharEquip owner, bool isEquiped)
    {
        this.owner = owner;
        this.index = index;
        this.item = item;
        this.isEquiped = isEquiped;

        if (this.title != null)
        {
            this.title.text = this.item.Define.Name;
        }

        if (this.level != null)
        {
            this.level.text = item.Define.Level.ToString();
        }

        if (this.limitClass != null)
        {
            this.limitClass.text = item.Define.LimitClass.ToString();
        }

        if (this.limitCategory != null)
        {
            this.limitCategory.text = item.Define.Category;
        }

        if (this.icon != null)
        {
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEquiped)
        {
            UnEquip();
        }
        else
        {
            if (this.selected)
            {
                DoEquip();
                this.Selected = false;
            }
            else
            {
                this.Selected = true;
            }
        }
    }

    private void DoEquip()
    {
        var message = MessageBox.Show(string.Format("要装备：{0}嘛？", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        message.OnYes = () =>
        {
            var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.slot);
            if (oldEquip != null)
            {
                var newmsg = MessageBox.Show(string.Format("要替换装备：{0}嘛？", oldEquip.Define.Name), "确认", MessageBoxType.Confirm);
                newmsg.OnYes = () =>
                {
                    this.owner.DoEquip(this.item);
                };
            }
            else
            {
                this.owner.DoEquip(this.item);
            }
        };
    }

    private void UnEquip()
    {
        var message = MessageBox.Show(string.Format("要取下装备：{0}嘛？", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        message.OnYes = () =>
        {
            this.owner.UnEquip(this.item);
        };
    }
}
