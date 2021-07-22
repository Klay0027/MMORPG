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

namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            //MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapCharacterEnterRequest>(this.OnMapCharacterEnter);

        }

        public void Init()
        {
            MapManager.Instance.Init();
        }

    }
}
