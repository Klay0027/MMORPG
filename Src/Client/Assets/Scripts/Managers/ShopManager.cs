using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Services;

namespace Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        UIShop uiShop;
        ShopDefine shop;

        public void Init()
        {
            NpcManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeShop, OnOpenShop);
        }

        private bool OnOpenShop(NpcDefine npc)
        {
            this.ShowShop(npc.Param);
            return true;
        }

        /// <summary>
        /// 显示商店界面
        /// </summary>
        /// <param name="shopId"></param>
        public void ShowShop(int shopId)
        {           
            if (DataManager.Instance.Shops.TryGetValue(shopId, out shop))
            {
                uiShop = UIManager.Instance.Show<UIShop>();
                if (uiShop != null)
                {
                    uiShop.SetShop(shop);
                }
            }
        }

        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="shopItemId"></param>
        /// <returns></returns>
        public bool BuyItem(int shopId, int shopItemId)
        {
            ItemService.Instance.SendBuyItem(shopId, shopItemId);
            return false;
        }

    }
}
