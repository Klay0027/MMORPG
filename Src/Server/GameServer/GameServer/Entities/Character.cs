using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using GameServer.Models;
using Common;
using Common.Utils;
namespace GameServer.Entities
{
    class Character : CharacterBase, IPostResponser
    {       
        public TCharacter Data;

        public ItemManager ItemManager;

        public QuestManager QuestManager;

        public StatusManager StatusManager;

        public FriendManager FriendManager;

        public Team Team;
        public double TeamUpdateTS;

        public Guild Guild;
        public double GuildUpdateTS;

        public Chat Chat;

        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Id = cha.ID;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;//角色等级;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];

            this.ItemManager = new ItemManager(this);
            this.ItemManager.GetItemInfos(this.Info.Items);

            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Unlocked = this.Data.CharacterBag.Unlocked;
            this.Info.Bag.Items = this.Data.CharacterBag.Items;
            this.Info.Equips = this.Data.Equips;
            this.QuestManager = new QuestManager(this); //任务管理器初始化
            this.QuestManager.GetQuestInfos(this.Info.Quests); //从数据库中获取到当前玩家的任务信息
            this.StatusManager = new StatusManager(this);
            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Friends);

            this.Guild = GuildManager.Instance.GetGuild(this.Data.GuildId);
            this.Chat = new Chat(this);
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set 
            {
                if (this.Data.Gold == value)
                {
                    return;
                }
                this.StatusManager.AddGoldChange((int)(value - this.Data.Gold));
                this.Data.Gold = value;
            }
        }

        public void PostProcess(NetMessageResponse message)
        {
            Log.InfoFormat("PostProcess > Character : characterID:{0} Name:{1}", this.Id, this.Info.Name);
            this.FriendManager.PostProcess(message);

            if (this.Team != null)
            {
                //Log.InfoFormat("PostProcess > Character : characterID:{0}:{1}", this.Id, this.Info.Name);
                if (TeamUpdateTS < this.Team.timestamp)
                {
                    TeamUpdateTS = Time.timestamp;
                    this.Team.PostProcess(message);
                }
            }

            if (this.Guild != null)
            {
                if (this.Info.Guild == null)
                {
                    this.Info.Guild = this.Guild.GuildInfo(this);
                    if (message.mapCharacterEnter != null)
                    {
                        GuildUpdateTS = Guild.timestamp;
                    }
                }
                if (GuildUpdateTS < this.Guild.timestamp && message.mapCharacterEnter == null)
                {
                    GuildUpdateTS = Guild.timestamp;
                    this.Guild.PostProcess(this, message);
                }
            }

            if (this.StatusManager.HasStatus)
            {
                this.StatusManager.PostProcess(message);
            }

            this.Chat.PostProcess(message);
        }

        public void Clear()
        {
            this.FriendManager.OfflineNotify();
        }

        public NCharacterInfo GetBasicInfo()
        {
            return new NCharacterInfo()
            {
                Id = this.Id,
                Name = this.Info.Name,
                Class = this.Info.Class,
                Level = this.Info.Level
            };
        }
    }
}
