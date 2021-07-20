﻿using System;
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
                    User.Instance.CurrentCharacter = cha;
                }

                CharacterManager.Instance.AddCharacter(cha);

                if (CurrentMapId != response.mapId)
                {
                    this.EnterMap(response.mapId);
                    this.CurrentMapId = response.mapId;
                }
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        { 
        
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
            {
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
            }
        }
    }
}
