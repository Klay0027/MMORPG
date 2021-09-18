using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using Common;
using Network;
using UnityEngine;
using Models;
using Managers;

namespace Services
{
    class ItemService : Singleton<ItemService>, IDisposable
    {
        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Subscribe<ItemEquipResponse>(this.OnItemEquip);      
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Unsubscribe<ItemEquipResponse>(this.OnItemEquip);
        }

        /// <summary>
        /// 发送购买道具请求
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="shopItemId"></param>
        public void SendBuyItem(int shopId, int shopItemId)
        {
            Debug.Log("SendBuyItem");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopId = shopId;
            message.Request.itemBuy.shopItemId = shopItemId;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到服务器传来的购买道具响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            MessageBox.Show("购买结果:" + message.Result + "\n" + message.Errormsg, "购买完成");
        }


        Item pendingEquip = null;
        bool isEquip;

        /// <summary>
        /// 发送更新玩家身上装备的请求
        /// </summary>
        /// <param name="equip">哪个部位的装备</param>
        /// <param name="isEquip">玩家身上是否有装备</param>
        /// <returns></returns>
        public bool SendEquipItem(Item equip,bool isEquip)
        {
            if (pendingEquip != null)
            {
                return false;
            }
            Debug.Log("SendEquipItem");
            pendingEquip = equip;
            this.isEquip = isEquip;

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.Slot = (int)equip.EquipInfo.slot;
            message.Request.itemEquip.itemId = equip.Id;
            message.Request.itemEquip.isEquip = isEquip;
            NetClient.Instance.SendMessage(message);
            return true;
        }

        private void OnItemEquip(object sender, ItemEquipResponse message)
        {
            if (message.Result == Result.Success)
            {
                if (pendingEquip != null)
                {
                    if (this.isEquip)
                    {
                        EquipManager.Instance.OnEquipItem(pendingEquip);
                    }
                    else
                    {
                        EquipManager.Instance.OnUnEquipItem(pendingEquip.EquipInfo.slot);
                    }
                    pendingEquip = null;
                }
            }
        }
    }
}
