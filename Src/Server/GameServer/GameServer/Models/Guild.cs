using Common;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utils;
using GameServer.Services;

namespace GameServer.Models
{
    class Guild
    {
        //Guild 管理本公会自身所有业务
        public double timestamp;
        public TGuild Data;
        public Guild(TGuild guild)
        {
            this.Data = guild;
        }
        public string Name { get { return this.Data.Name; } }
        public int Id { get { return this.Data.Id; } }

        /// <summary>
        /// 加入公会
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        public bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.GuildApplies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null)
            {
                return false;
            }

            var dbApply = DBService.Instance.Entities.GuildApplie.Create();
            dbApply.GuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;

            DBService.Instance.Entities.GuildApplie.Add(dbApply);
            this.Data.GuildApplies.Add(dbApply);

            DBService.Instance.Save();

            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        /// <summary>
        /// 公会审批
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        public bool JoinAppove(NGuildApplyInfo apply)
        {
            //从数据库中查询是否有申请列表 如果查不到申请列表返回false
            var oldApply = this.Data.GuildApplies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if (oldApply == null)
            {
                return false;
            }

            oldApply.Result = (int)apply.Result;

            if (apply.Result == ApplyResult.Accept)
            {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
            }

            DBService.Instance.Save();

            this.timestamp = TimeUtil.timestamp;
            return true;
        }


        /// <summary>
        /// 添加公会成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="class"></param>
        /// <param name="level"></param>
        /// <param name="title"></param>
        public void AddMember(int characterId, string name, int @class, int level, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember()
            {
                CharacterId = characterId,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
                LastTime = now
            };
            this.Data.GuildMember.Add(dbMember);
            var character = CharacterManager.Instance.GetCharacter(characterId);
            if (character != null)
            {
                character.Data.GuildId = this.Id;
            }
            else
            {
                TCharacter dbChar = DBService.Instance.Entities.Characters.SingleOrDefault(c => c.ID == characterId);
                dbChar.GuildId = this.Id;
            }
        }

        /// <summary>
        /// 离开公会
        /// </summary>
        /// <param name="member"></param>
        public void Leave(Character member)
        {

        }

        
        public void PostProcess(Character from, NetMessageResponse message)
        {
            if (message.Guild == null)
            {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = this.GuildInfo(from);
            }
        }

        public NGuildInfo GuildInfo(Character from)
        {
            NGuildInfo info = new NGuildInfo()
            {
                Id = this.Id,
                GuildName = this.Name,
                Notice = this.Data.Notice,
                leaderId = this.Data.LeaderID,
                leaderName = this.Data.LeaderName,
                createTime = (long)TimeUtil.GetTimestamp(this.Data.CreateTime),
                memberCount = this.Data.GuildMember.Count
            };

            if (from != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if (from.Id == this.Data.LeaderID)
                {
                    info.Applies.AddRange(GetApplyInfos());
                }
            }
            return info;
        }
        private List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();
            foreach (var member in this.Data.GuildMember)
            {
                var memberInfo = new NGuildMemberInfo()
                {
                    Id = member.Id,
                    characterId = member.CharacterId,
                    Title = (GuildTitle)member.Title,
                    joinTime = (long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime),
                };
                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Level = character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                }
                else
                {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                }

                members.Add(memberInfo);
            }
            return members;
        }

        private NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo()
            {
                Id = member.CharacterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }

        /// <summary>
        /// 获取公会申请列表
        /// </summary>
        /// <returns></returns>
        private List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach (var apply in this.Data.GuildApplies)
            {
                if (apply.Result != (int)ApplyResult.None)
                {
                    continue;
                }
                applies.Add(new NGuildApplyInfo()
                {
                    characterId = apply.CharacterId,
                    GuildId = apply.GuildId,
                    Class = apply.Class,
                    Level = apply.Level,
                    Name = apply.Name,
                    Result = (ApplyResult)apply.Result
                });
            }
            return applies;
        }

        /// <summary>
        /// 查找公会成员
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        private TGuildMember GetDBMember(int characterId)
        {
            foreach (var member in this.Data.GuildMember)
            {
                if (member.CharacterId == characterId)
                {
                    return member;
                }
            }
            return null;
        }

        /// <summary>
        /// 执行管理的方法
        /// </summary>
        /// <param name="command">管理 做的操作</param>
        /// <param name="targetId">执行目标</param>
        /// <param name="sourceId">执行者</param>
        public void ExcuteAdmin(GuildAdminCommand command, int targetId, int sourceId)
        {
            var target = GetDBMember(targetId);
            var source = GetDBMember(sourceId);
            switch (command)
            {
                case GuildAdminCommand.Kickout:
                    //踢人
                    break;
                case GuildAdminCommand.Promote:
                    //提拔
                    target.Title = (int)GuildTitle.VicePresident;
                    break;
                case GuildAdminCommand.Depost:
                    //罢免 降职
                    target.Title = (int)GuildTitle.None;
                    break;
                case GuildAdminCommand.Transfer:
                    //转让会长
                    target.Title = (int)GuildTitle.President;
                    source.Title = (int)GuildTitle.None;
                    this.Data.LeaderID = targetId;
                    this.Data.LeaderName = target.Name;
                    break;
            }
            DBService.Instance.Save();
            timestamp = TimeUtil.timestamp;
        }
    }
}
