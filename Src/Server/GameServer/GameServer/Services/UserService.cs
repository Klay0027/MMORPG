using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class UserService:Singleton<UserService>
    {
        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }
        
        public void Init()
        {

        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                message.Response.userRegister.Result = Result.Failed;
                message.Response.userRegister.Errormsg = "用户已存在！";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player, RegisterDate = DateTime.Now });
                DBService.Instance.Entities.SaveChanges();
                message.Response.userRegister.Result = Result.Success;
                message.Response.userRegister.Errormsg = "注册成功！";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == request.Passward)
                {
                    sender.Session.User = user;
                    message.Response.userLogin.Result = Result.Success;
                    message.Response.userLogin.Errormsg = "登录成功，欢迎来到声闻世界！";
                    message.Response.userLogin.Userinfo = new NUserInfo();
                    message.Response.userLogin.Userinfo.Id = 1;
                    message.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                    message.Response.userLogin.Userinfo.Player.Id = user.Player.ID;

                    foreach (var character in user.Player.Characters)
                    {
                        NCharacterInfo info = new NCharacterInfo();
                        info.Id = character.ID;
                        info.Name = character.Name;
                        info.Class = (CharacterClass)character.Class;
                        message.Response.userLogin.Userinfo.Player.Characters.Add(info);
                    }
                    
                }
                else 
                {
                    message.Response.userLogin.Result = Result.Failed;
                    message.Response.userLogin.Errormsg = "用户名或密码不对！";
                }               
            }
            else
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "用户不存在!请先注册~";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("CreateCharacter: Name:{0}  Class:{1}", request.Name, request.Class);

            //创建角色对象
            TCharacter character = new TCharacter();
            character.Class = (int)request.Class;
            character.Name = request.Name;
            character.TID = (int)request.Class;
            character.MapID = 1;
            character.MapPosX = 5000;
            character.MapPosY = 4000;
            character.MapPosZ = 820;

            //调用数据库服务新增角色存储到数据库
            character = DBService.Instance.Entities.Characters.Add(character);
            sender.Session.User.Player.Characters.Add(character); //添加角色session
            DBService.Instance.Entities.SaveChanges(); //保存数据

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.createChar = new UserCreateCharacterResponse();
            message.Response.createChar.Result = Result.Success;
            message.Response.createChar.Errormsg = "添加角色成功！";
            //遍历当前玩家有哪些角色
            foreach (var item in sender.Session.User.Player.Characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = item.ID;
                info.Name = item.Name;
                info.Class = (CharacterClass)item.Class;
                info.Tid = item.TID;
                message.Response.createChar.Characters.Add(info);
            }
            
            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        /// <summary>
        /// 进入游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);

            Log.InfoFormat("UserGameEnterRequest: characterID:{0}:{1} Map:{2}", dbchar.ID, dbchar.Name, dbchar.MapID);

            Character character = CharacterManager.Instance.AddCharacter(dbchar);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameEnter = new UserGameEnterResponse();
            message.Response.gameEnter.Result = Result.Success;
            message.Response.gameEnter.Errormsg = "Success!";

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
            sender.Session.Character = character;

            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
        }

        /// <summary>
        /// 离开游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGameLeaveRequest: characterID:{0}:{1} Map:{2}", character.Id, character.Info.Name, character.Info.mapId);

            //根据角色ID在角色管理器中从字典Characters移除
            CharacterManager.Instance.RemoveCharacter(character.Id);

            //根据角色信息在地图管理器中从当前地图字典中移除
            MapManager.Instance[character.Info.mapId].CharacterLeave(character.Info);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameLeave = new UserGameLeaveResponse();
            message.Response.gameLeave.Result = Result.Success;
            message.Response.gameLeave.Errormsg = "None";

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }
    }
}
