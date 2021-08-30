using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class ItemManager
    {
        Character Owner;

        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        /// <summary>
        /// 初始化当前角色的道具及数量
        /// </summary>
        /// <param name="owner"></param>
        public ItemManager(Character owner)
        {
            this.Owner = owner;

            foreach (var item in owner.Data.Items)
            {
                this.Items.Add(item.ItemID, new Item(item));
            }
        }

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool UseItem(int itemId, int count = 1)
        {
            Log.InfoFormat("[{0}]UserItem[{1}:{2}]", this.Owner.Data.ID, itemId, count);
            Item item = null;
            if (this.Items.TryGetValue(itemId, out item))
            {
                if (item.Count < count)
                {
                    return false;
                }

                item.Remove(count);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断道具是否存在
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool HasItem(int itemId)
        {
            Item item = null;

            if (this.Items.TryGetValue(itemId, out item))
            {
                return item.Count > 0;
            }

            return false;
        }

        /// <summary>
        /// 获取道具
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Item GetItem(int itemId)
        {
            Item item = null;
            this.Items.TryGetValue(itemId, out item);
            Log.InfoFormat("[{0}]GetItem[{1}:{2}]", this.Owner.Data.ID, itemId, item);
            return item;
        }

        /// <summary>
        /// 添加道具
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool AddItem(int itemId, int count)
        {
            Item item = null;

            if (this.Items.TryGetValue(itemId, out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItems dbItem = new TCharacterItems();
                dbItem.TCharacterID = Owner.Data.ID;
                dbItem.Owner = Owner.Data;
                dbItem.ItemID = itemId;
                dbItem.ItemCount = count;
                Owner.Data.Items.Add(dbItem);
                item = new Item(dbItem);
                this.Items.Add(itemId, item);
            }
            this.Owner.StatusManager.AddItemChange(itemId, count, StatusAction.Add);
            Log.InfoFormat("[{0}]AddItem[{1}] addCount:{2}", this.Owner.Data.ID, item, count);
            //DBService.Instance.Save();
            return true;
        }

        /// <summary>
        /// 移除道具
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool RemoveItem(int itemId, int count)
        {
            if (!this.Items.ContainsKey(itemId))
            {
                return false;
            }
            Item item = this.Items[itemId];
            if (item.Count < count)
            {
                return false;
            }
            item.Remove(count);
            this.Owner.StatusManager.AddItemChange(itemId, count, StatusAction.Delete);
            Log.InfoFormat("[{0}]RemoveItem[{1}] removeCount:{2}", this.Owner.Data.ID, item, count);
            //DBService.Instance.Save();
            return true;
        }

        public void GetItemInfos(List<NItemInfo> list)
        {
            foreach (var item in this.Items)
            {
                list.Add(new NItemInfo() { Id = item.Value.ItemID, Count = item.Value.Count });
            }
        }

    }
}
