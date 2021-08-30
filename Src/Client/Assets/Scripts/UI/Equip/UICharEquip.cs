using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Models;

public class UICharEquip : UIWindow
{
    public Button close_btn;
    public Text title, money;
    public GameObject itemPrefab, itemEquipedPrefab;
    public Transform itemListRoot;
    public List<Transform> slots;

    private void Start()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChanged += RefreshUI;
        close_btn.onClick.AddListener(OnCloseClick);
    }

    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipedList();
        InitEquipedItems();
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    /// <summary>
    /// 初始化所有待装备道具
    /// </summary>
    private void InitAllEquipItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class)
            {
                if (EquipManager.Instance.Contains(kv.Key))
                {
                    continue;
                }
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }  
    }

    /// <summary>
    /// 清除左边待装备列表
    /// </summary>
    private void ClearAllEquipList()
    {
        foreach (var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }  
    }

    /// <summary>
    /// 清除玩家身上的装备
    /// </summary>
    private void ClearEquipedList()
    {
        foreach (var item in slots)
        {
            if (item.childCount > 1)
            {
                Destroy(item.GetChild(1).gameObject);
            }
        }
    }

    /// <summary>
    /// 初始化已经装备的列表
    /// </summary>
    private void InitEquipedItems()
    {
        for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
            var item = EquipManager.Instance.Equips[i];
            {
                if (item != null)
                {
                    GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }
            }
        }  
    }

    /// <summary>
    /// 穿装备
    /// </summary>
    /// <param name="item"></param>
    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }

    /// <summary>
    /// 脱装备
    /// </summary>
    /// <param name="item"></param>
    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }
}
