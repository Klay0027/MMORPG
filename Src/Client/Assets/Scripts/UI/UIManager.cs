using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    class UIElement
    {
        public string Resources;
        public bool Cache;
        public GameObject Instance;

    }

    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

    public UIManager()
    {
        this.UIResources.Add(typeof(UITest), new UIElement() { Resources = "PreFabs/UI_Test", Cache = true });
        this.UIResources.Add(typeof(UIBag), new UIElement() { Resources = "PreFabs/Bag/UI_Bag", Cache = false });
        this.UIResources.Add(typeof(UIShop), new UIElement() { Resources = "PreFabs/Shop/UI_Shop", Cache = false });
        this.UIResources.Add(typeof(UICharEquip), new UIElement() { Resources = "PreFabs/Equip/UI_ChatEquip", Cache = false });
        this.UIResources.Add(typeof(UIQuestSystem), new UIElement() { Resources = "PreFabs/Quest/UI_QuestSystem", Cache = false });
        this.UIResources.Add(typeof(UIQuestDialog), new UIElement() { Resources = "PreFabs/Quest/UIQuestDialog", Cache = false });
    }

    ~UIManager()
    { 
    
    }

    public T Show<T>()
    {
        //SoundManager.Instance.PlaySound("ui_open");
        Type type = typeof(T); //当执行show方法时，取得调用脚本的类型 type
        //根据 type 判断是否存在字典中 
        if (this.UIResources.ContainsKey(type))
        {
            //根据 type 获取到这个UI元素的信息
            UIElement info = this.UIResources[type];
            
            //如果 这个UI元素已经被实例化一次 则直接打开
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                //动态加载资源
                UnityEngine.Object prefab = Resources.Load(info.Resources);
                //如果资源不存在 返回默认类型
                if (prefab == null)
                {
                    return default(T);
                }
                //如果资源存在 加载出资源
                info.Instance = (GameObject)GameObject.Instantiate(prefab);
            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="type"></param>
    public void Close(Type type)
    {
        //SoundManager.Instance.PlaySound("ui_close");
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }
}
