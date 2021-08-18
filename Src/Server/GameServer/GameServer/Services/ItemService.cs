﻿using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class ItemService : Singleton<ItemService>
    {
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
        }

        private void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemBuy: character : {0}, shop: {1}, shopItem: {2}", character.Id, request.shopId, request.shopItemId);
            //var result = ShopManager
        }

        public void Init()
        { 
        
        }


    }
}
