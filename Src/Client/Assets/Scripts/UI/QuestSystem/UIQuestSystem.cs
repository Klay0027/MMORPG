using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;

public class UIQuestSystem : UIWindow
{
    public Text title;
    public GameObject itemPrefab;
    public TabView Tabs;
    public ListView listMain, listBranch;
    public UIQuestInfo questInfo;

    private bool showAvailableList = false;

    private void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
        this.listBranch.onItemSelected += this.OnQuestSelected;
        this.Tabs.OnTabSelect += OnSelectTab;
        //QuestManager.Instance.onq
    }

    private void OnSelectTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }

    private void RefreshUI()
    {
        ClearAllQuestList();
        InitAllQuestItems();
    }

    private void InitAllQuestItems()
    {
        foreach (var kv in QuestManager.Instance.allQuests)
        {
            if (showAvailableList)
            {
                if (kv.Value.Info != null)
                {
                    continue;
                }
                else
                {
                    if (kv.Value.Info == null)
                    {
                        continue;
                    }
                }

                //GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main)
                GameObject go = Instantiate(itemPrefab);
                UIQuestItem ui = go.GetComponent<UIQuestItem>();
                ui.SetQuestInfo(kv.Value);
                if (kv.Value.Define.Type == QuestType.Main)
                {
                    this.listMain.AddItem(ui as ListView.ListViewItem);
                }
                else
                {
                    this.listBranch.AddItem(ui as ListView.ListViewItem);
                }
            }
        } 
    }

    private void ClearAllQuestList()
    {
        this.listMain.RemoveAll();
        this.listBranch.RemoveAll();   
    }

    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }

    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        this.questInfo.SetQuestInfo(questItem.quest);
    }
}

