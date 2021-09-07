using GameServer.Entities;
using GameServer.Models;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class MonsterManager
    {
        private Map Map;

        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();

        public void Init(Map map)
        {
            this.Map = map;
        }

        /// <summary>
        /// 创建一个怪物
        /// </summary>
        /// <param name="spawnMonID"></param>
        /// <param name="spawnLevel"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        internal Monster Create(int spawnMonID, int spawnLevel, NVector3 position, NVector3 direction)
        {
            Monster monster = new Monster(spawnMonID, spawnLevel, position, direction);
            EntityManager.Instance.AddEntity(this.Map.ID, monster);
            monster.Id = monster.entityId;
            monster.Info.EntityId = monster.entityId;
            monster.Info.mapId = this.Map.ID;
            Monsters[monster.Id] = monster;

            this.Map.MonsterEnter(monster);
            return monster;
        }
    }
}
