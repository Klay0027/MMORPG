using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using UnityEngine;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }

        //设置角色信息
        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        //当前角色
        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

        /// <summary>
        /// 当前地图的信息
        /// </summary>
        public MapDefine CurrentMapData { get; set; }

        /// <summary>
        /// 当前角色的游戏对象
        /// </summary>
        public GameObject CurrentCharacterObject { get; set; }
    }
}
