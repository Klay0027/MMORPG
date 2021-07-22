using System;
using Common.Data;
using SkillBridge.Message;
using Models;
using Managers;
using Network;
using UnityEngine;

namespace Services
{
    public class MapService : Singleton<MapService>, IDisposable
    {
        public int CurrentMapId = 0;

        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        public void Init()
        { 
        
        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", response.mapId, response.Characters.Count);

            foreach (var cha in response.Characters)
            {
                if (User.Instance.CurrentCharacter.Id == cha.Id)
                {
                    User.Instance.CurrentCharacter = cha; //刷新一遍角色数据
                }

                CharacterManager.Instance.AddCharacter(cha); //遍历当前地图所有角色 交给角色管理器

                if (CurrentMapId != response.mapId)
                {
                    this.EnterMap(response.mapId); //进入新地图
                    this.CurrentMapId = response.mapId; //更新当前所在地图的地图编号
                }
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        { 
        
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId)) //判断传来的地图编号是否存在
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource); //跳转场景
            }
            else
            {
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
            }
        }
    }
}
