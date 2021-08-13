using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Common.Data;
using Models;

public class UIShop : UIWindow
{
    public Text title, money;
    public GameObject shopItem;
    public Transform[] itemRoot;
    private ShopDefine shop;

    private void Start()
    {
        
    }

    private IEnumerator InitItems()
    {
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if (kv.Value.Status > 0)
            {
                GameObject go = Instantiate(shopItem, itemRoot[0]);
                //UIShopItem ui = go.GetComponent<UIShopItem>();
                //ui.SetShopItem(kv.Key, kv.Value, this);
            }
        }
        yield return null;
    }

    public void SetShop(ShopDefine shop)
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    //private UIShopItem selectedItem;
}
