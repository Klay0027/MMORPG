using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public Sprite activeImage;
    private Sprite normalImage;
    private Image tabImage;

    public TabView tabView;
    public int tabIndex;
    public bool selected = false;

    private void Start()
    {
        tabImage = this.GetComponent<Image>();

        normalImage = tabImage.sprite;

        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Select(bool select)
    {
        tabImage.overrideSprite = select ? activeImage : normalImage;
    }

    private void OnClick()
    {
        this.tabView.SelectTab(this.tabIndex);
    }
}
