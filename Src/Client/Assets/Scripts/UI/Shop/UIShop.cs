using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Common.Data;
using Models;
using Managers;

public class UIShop : UIWindow
{
    public Text title, money;
    public GameObject shopItem;
    public Transform[] itemRoot;
    public Button buy_btn, close_btn;
    private ShopDefine shop;

    private void Start()
    {
        buy_btn.onClick.AddListener(OnClickBuy);
        close_btn.onClick.AddListener(OnCloseClick);
        StartCoroutine(InitItems());
    }

    /// <summary>
    /// 初始化商店中的商品
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitItems()
    {
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            //商品的状态大于0说明该商品在销售中
            if (kv.Value.Status > 0)
            {
                GameObject go = Instantiate(shopItem, itemRoot[0]);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.SetShopItem(kv.Key, kv.Value, this);
            }
        }
        yield return null;
    }

    /// <summary>
    /// 初始化打开的是什么类型的商店
    /// </summary>
    /// <param name="shop"></param>
    public void SetShop(ShopDefine shop)
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    private UIShopItem selectedItem;

    public void SelectShopItem(UIShopItem item)
    {
        if (selectedItem != null)
        {
            selectedItem.Selected = false;
        }
        selectedItem = item;
    }

    public void OnClickBuy()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要购买的道具", "购买提示");
            return;
        }

        if (!ShopManager.Instance.BuyItem(this.shop.ID, this.selectedItem.ShopItemID))
        {

        }
    }
}
