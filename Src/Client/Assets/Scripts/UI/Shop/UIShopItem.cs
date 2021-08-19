using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour, ISelectHandler
{
    public Image icon, background;
    public Text title, price, count, limitClass;
    //public Sprite normalBg, selectedBg;

    private bool selected;

    public bool Selected
    {
        get 
        {
            return selected;
        }   
        set
        {
            selected = value;
            //this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
    }
    public int ShopItemID { get; set; }
    private UIShop shop;
    private ItemDefine item;
    private ShopItemDefine ShopItem { get; set; }

    public void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
    {
        this.shop = owner;
        this.ShopItemID = id;
        this.ShopItem = shopItem;
        this.item = DataManager.Instance.Items[this.ShopItem.ItemID];

        this.title.text = this.item.Name;
        this.count.text = "X" + ShopItem.Count.ToString();
        this.price.text = ShopItem.Price.ToString();
        this.limitClass.text = this.item.LimitClass.ToString();
        this.icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);
    }

    /// <summary>
    /// 被选择
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        this.shop.SelectShopItem(this);
    }
}
