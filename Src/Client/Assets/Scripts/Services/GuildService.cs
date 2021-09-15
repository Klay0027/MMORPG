﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SkillBridge.Message;
using Common;
using Network;
using Models;
using Managers;
using UnityEngine.Events;

namespace Services
{
    public class GuildService : Singleton<GuildService>, IDisposable
    {
        public UnityAction OnGuildUpdate;
        public UnityAction<bool> OnGuildCreateResult;
        public UnityAction<List<NGuildInfo>> OnGuildListResult;

        public void Init()
        { 
        
        }

        public GuildService()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);
            MessageDistributer.Instance.Subscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);
            MessageDistributer.Instance.Unsubscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        /// <summary>
        /// 发送创建公会请求
        /// </summary>
        /// <param name="guildName"></param>
        /// <param name="notice"></param>
        public void SendGuildCreate(string guildName, string notice)
        {
            Debug.Log("SendGuildCreate");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildCreate = new GuildCreateRequest();
            message.Request.guildCreate.GuildName = guildName;
            message.Request.guildCreate.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到创建公会响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void OnGuildCreate(object sender, GuildCreateResponse response)
        {
            Debug.LogFormat("OnGuildCreateResponse", response.Result);
            if (OnGuildCreateResult != null)
            {
                this.OnGuildCreateResult(response.Result == Result.Success);
            }
            if (response.Result == Result.Success)
            {
                GuildManager.Instance.Init(response.guildInfo);
                MessageBox.Show(string.Format("{0}公会创建成功", response.guildInfo.GuildName), "公会创建");
            }
            else
            {
                MessageBox.Show(string.Format("{0}公会创建失败", response.guildInfo.GuildName), "公会创建");
            }
        }

        /// <summary>
        /// 发送加入公会的请求
        /// </summary>
        /// <param name="guildId">公会ID</param>
        public void SendGuildJoinRequest(int guildId)
        {
            Debug.Log("SendGuildJoinRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinReq = new GuildJoinRequest();
            message.Request.guildJoinReq.Apply = new NGuildApplyInfo();
            message.Request.guildJoinReq.Apply.GuildId = guildId;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 会长在线状态 收到加入公会的请求 返回给玩家是否同意加入
        /// </summary>
        /// <param name="guildId">公会ID</param>
        public void SendGuildJoinResponse(bool accept, GuildJoinRequest request)
        {
            Debug.Log("SendGuildJoinResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = request.Apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept :ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 会长在线状态 收到玩家加入公会的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnGuildJoinRequest(object sender, GuildJoinRequest request)
        {
            var confirm = MessageBox.Show(string.Format("[{0}]申请加入公会", request.Apply.Name), "公会申请", MessageBoxType.Confirm, "同意", "拒绝");
            confirm.OnYes = () =>
            {
                this.SendGuildJoinResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendGuildJoinResponse(false, request);
            };
        }

        /// <summary>
        /// 在线玩家 收到加入公会的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnGuildJoinResponse(object sender, GuildJoinResponse response)
        {
            Debug.LogFormat("OnGuildJoinResponse:{0}", response.Result);
            if (response.Result == Result.Success)
            {
                MessageBox.Show( "加入公会成功", "公会申请");
            }
            else
            {
                MessageBox.Show("加入公会失败", "公会申请");
            }
        }

        /// <summary>
        /// 公会信息刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnGuild(object sender, GuildResponse message)
        {
            Debug.LogFormat("OnGuild:{0} {1} {2}", message.Result, message.guildInfo.Id, message.guildInfo.GuildName);
            GuildManager.Instance.Init(message.guildInfo);
            if (this.OnGuildUpdate != null)
            {
                this.OnGuildUpdate();
            }
        }

        public void SendGuildLeaveRequest()
        {
            Debug.Log("SendGuildLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildLeave = new GuildLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                GuildManager.Instance.Init(null);
                MessageBox.Show("离开公会成功", "公会");
            }
            else
            {
                MessageBox.Show("离开公会失败", "公会", MessageBoxType.Error);
            }
        }

        public void SendGuildListRequest()
        {
            Debug.Log("SendGuildListRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildList(object sender, GuildListResponse response)
        {
            if (OnGuildListResult != null)
            {
                this.OnGuildListResult(response.Guilds);
            }
        }

        public void SendGuildJoinApply(bool accept, NGuildApplyInfo apply)
        {
            Debug.Log("SendGuildJoinApply");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        public void SendAdminCommand(GuildAdminCommand command, int characterId)
        {
            Debug.Log("SendAdminCommand");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildAdmin = new GuildAdminRequest();
            message.Request.guildAdmin.Command = command;
            message.Request.guildAdmin.Target = characterId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildAdmin(object sender, GuildAdminResponse message)
        {
            Debug.LogFormat("GuildAdmin : {0} {1}", message.Command, message.Result);
            MessageBox.Show(string.Format("执行操作：{0} 结果： {1} {2}", message.Command, message.Result, message.Errormsg));
        }
    }
}
