﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TabView : MonoBehaviour
{
    public TabButton[] tabButtons;

    public GameObject[] tabPages;

    private IEnumerator Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].tabView = this;
            tabButtons[i].tabIndex = i;
        }

        yield return new WaitForEndOfFrame();

        SelectTab(0);
    }

    public void SelectTab(int index)
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].Select(i == index);
            tabPages[i].SetActive(i == index);
        }
    }
}
