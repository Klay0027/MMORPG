using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour
{
    //public Image avatar;

    public Text avaverName, avaverLevel;

    public Character character;

    private void Start()
    {
        if (this.character != null)
        {
            if (character.Info.Type == SkillBridge.Message.CharacterType.Monster)
            {
                //this.avatar.gameObject.SetActive(false);
            }
            else
            {
                //this.avatar.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        this.UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name;
            string level = " Lv." + this.character.Info.Level;
            if (name != this.avaverName.text)
            {
                this.avaverName.text = name;
                this.avaverLevel.text = level;
            }
        }
    
    }
}

