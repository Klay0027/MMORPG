using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Models;
using SkillBridge.Message;
using UnityEngine.Events;

namespace Managers
{
    public enum NpcQuestStatus
    { 
        None = 0,
        Complete,
        Available,
        Incomplete,
    }

    public class QuestManager : Singleton<QuestManager>
    {
        public List<NQuestInfo> questInfos; //所有有效任务
        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
        //分NPC的状态任务
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();
        public UnityAction<Quest> onQuestStatusChanged;

        /// <summary>
        /// 任务管理器初始化
        /// </summary>
        /// <param name="quests"></param>
        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuests.Clear();
            this.npcQuests.Clear();
            InitQuests();
        }

        /// <summary>
        /// 初始化所有任务信息
        /// </summary>
        private void InitQuests()
        {
            //初始化已接任务
            foreach (var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.allQuests[quest.Info.QuestId] = quest;
            }

            this.CheckAvailableQuests();

            foreach (var kv in this.allQuests)
            {
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
        }

        /// <summary>
        /// 初始化可接任务
        /// </summary>
        private void CheckAvailableQuests()
        {
            //根据当前玩家的角色类型和等级 初始化可接任务
            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
                {
                    continue; //表示这项任务不符合玩家的角色类型
                }

                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                {
                    continue; //表示这项任务 玩家等级不够
                }

                if (this.allQuests.ContainsKey(kv.Key))
                {
                    continue; //表示此项任务玩家已做过
                }

                //如果可解任务里面包含有前置任务的任务时
                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    //获取此项任务的前置任务
                    if (this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest))
                    {
                        if (preQuest.Info == null)
                        {
                            continue; //前置任务未接取
                        }
                        if (preQuest.Info.Status != QuestStatus.Finished)
                        {
                            continue; //前置任务未完成
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                Quest quest = new Quest(kv.Value);
                this.allQuests[quest.Define.ID] = quest;
            }
        }

        /// <summary>
        /// 根据Npc类型拆分所有任务到Npc身上
        /// </summary>
        /// <param name="npcId"></param>
        /// <param name="quest"></param>
        private void AddNpcQuest(int npcId, Quest quest)
        {
            if (!this.npcQuests.ContainsKey(npcId))
            {
                this.npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
            }

            List<Quest> availables;
            List<Quest> complates;
            List<Quest> incomplates;

            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Available] = availables;
            }

            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Complete, out complates))
            {
                complates = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Complete] = complates;
            }

            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incomplates))
            {
                incomplates = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Incomplete] = incomplates;
            }

            if (quest.Info == null)
            {
                if (npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                {
                    this.npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.Complated)
                {
                    if (!this.npcQuests[npcId][NpcQuestStatus.Complete].Contains(quest))
                    {
                        this.npcQuests[npcId][NpcQuestStatus.Complete].Add(quest);
                    }
                }
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!this.npcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                    {
                        this.npcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                    }
                }
            }
        }

        /// <summary>
        /// 获取NPC任务状态
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public NpcQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (this.npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                {
                    return NpcQuestStatus.Complete;
                }

                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return NpcQuestStatus.Available;
                }

                if (status[NpcQuestStatus.Incomplete].Count > 0)
                {
                    return NpcQuestStatus.Incomplete;
                }
            }
            return NpcQuestStatus.None;
        }

        /// <summary>
        /// 打开NPC任务
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();

            if (this.npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Complete].First());
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Available].First());
                }
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
                }
            }
            return false;
        }

        /// <summary>
        /// 显示任务对话框
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        private bool ShowQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dlg = UIManager.Instance.Show<UIQuestDialog>();
                dlg.SetQuest(quest);
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DiaLogIncomplete))
                {
                    MessageBox.Show(quest.Define.DiaLogIncomplete);
                }
            }
            return true;
        }

        /// <summary>
        /// 当对话框关闭时 根据点击按钮的结果判断时发送提交、接取或拒绝任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="result"></param>
        private void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindow.WindowResult.Yes)
            {
                if (dlg.quest.Info == null)
                {
                    QuestService.Instance.SendQuestAccept(dlg.quest);
                }
                else if(dlg.quest.Info.Status == QuestStatus.Complated)
                {
                    QuestService.Instance.SendQuestSubmit(dlg.quest);
                }
            }
            else if (result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DiaLogDeny);
            }
        }

        private Quest RefreshQuestStatus(NQuestInfo quest)
        {
            this.npcQuests.Clear();
            Quest result;
            if (this.allQuests.ContainsKey(quest.QuestId))
            {
                this.allQuests[quest.QuestId].Info = quest;
                result = this.allQuests[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                this.allQuests[quest.QuestId] = result;
            }

            CheckAvailableQuests();

            foreach (var kv in this.allQuests)
            {
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }

            if (onQuestStatusChanged != null)
            {
                onQuestStatusChanged(result);
            }

            return result;
        }

        public void OnQuestAccepted(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DiaLogAccept);
        }

        public void OnQuestSubmited(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DiaLogFinish);
        }
    }
}
