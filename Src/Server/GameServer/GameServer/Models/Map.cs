using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using Common;
using Common.Data;
using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        //地图中的角色 以角色ID为Key
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();

        SpawnManager SpawnManager = new SpawnManager();
        public MonsterManager MonsterManager = new MonsterManager();

        internal Map(MapDefine define)
        {
            this.Define = define;
            this.SpawnManager.Init(this);
            this.MonsterManager.Init(this);
        }

        //在当前地图中同步状态
        internal void UpdateEntity(NEntitySync entity)
        {
            foreach (var item in this.MapCharacters)
            {
                if (item.Value.character.entityId == entity.Id)
                {
                    item.Value.character.Position = entity.Entity.Position;
                    item.Value.character.Speed = entity.Entity.Speed;
                    item.Value.character.Direction = entity.Entity.Direction;
                }
                else
                {
                    MapService.Instance.SendEntityUpdate(item.Value.connection, entity);
                }
            }
        }

        internal void Update()
        {
            SpawnManager.Update();
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1} characterName:{2}", this.Define.ID, character.Id, character.Info.Name);

            character.Info.mapId = this.ID;
            this.MapCharacters[character.Id] = new MapCharacter(conn, character);

            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;

            conn.Session.Response.mapCharacterEnter.Characters.Add(character.Info);

            foreach (var kv in this.MapCharacters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                if (kv.Value.character != character)
                {
                    this.AddCharacterEnterMap(kv.Value.connection, character.Info);
                }               
            }

            foreach (var kv in this.MonsterManager.Monsters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.Info);
            }           

            conn.SendResponse();
        }

        /// <summary>
        /// 发送通知其他玩家有新玩家进入
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="character"></param>
        void AddCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            if (conn.Session.Response.mapCharacterEnter == null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }
            conn.Session.Response.mapCharacterEnter.Characters.Add(character);
            conn.SendResponse();
        }

        /// <summary>
        /// 角色离开地图
        /// </summary>
        /// <param name="nCharacter"></param>
        internal void CharacterLeave(Character Character)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}", this.Define.ID, Character.Id);

            //通知其他在线玩家 有角色离开
            foreach (var kv in this.MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, Character);
            }
            this.MapCharacters.Remove(Character.Id);//从当前地图中 管理所有角色的字典中 移除要离开的角色
        }

        /// <summary>
        /// 发送通知其他玩家有新玩家离开
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="character"></param>
        private void SendCharacterLeaveMap(NetConnection<NetSession> conn, Character character)
        {
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = character.entityId;
            conn.SendResponse();
        }

        /// <summary>
        /// 怪物进入地图
        /// </summary>
        /// <param name="monster"></param>
        public void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("MonsterEnter：Map:{0} monsterId:{1}", this.Define.ID, monster.Id);
            foreach (var kv in this.MapCharacters)
            {
                this.AddCharacterEnterMap(kv.Value.connection, monster.Info);
            }
        }
    }
}
