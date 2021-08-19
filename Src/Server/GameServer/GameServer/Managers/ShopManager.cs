using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using GameServer.Services;
namespace GameServer.Managers
{
    class ShopManager : Singleton<ShopManager>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">发送者 代表谁来买</param>
        /// <param name="shopId">商店ID 代表哪一个类型的商店</param>
        /// <param name="shopItemId">在该商店购买的道具的ID</param>
        /// <returns></returns>
        public Result BuyItem(NetConnection<NetSession> sender, int shopId, int shopItemId)
        {
            if (!DataManager.Instance.Shops.ContainsKey(shopId))
            {
                return Result.Failed;
            }
            ShopItemDefine shopItem;

            if (DataManager.Instance.ShopItems[shopId].TryGetValue(shopItemId, out shopItem))
            {
                Log.InfoFormat("BuyItem : character:{0} Item:{1} Count:{2} Price:{3}", sender.Session.Character.Id, shopItem.ItemID, shopItem.Count, shopItem.Price);
                if (sender.Session.Character.Gold >= shopItem.Price)
                {
                    sender.Session.Character.ItemManager.AddItem(shopItem.ItemID, shopItem.Count);
                    sender.Session.Character.Gold -= shopItem.Price;
                    DBService.Instance.Save();
                    return Result.Success;
                }
            }
            return Result.Failed;
        }
    }
}
