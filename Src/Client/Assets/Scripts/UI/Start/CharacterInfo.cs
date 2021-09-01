using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    public SkillBridge.Message.NCharacterInfo info;

    public Text charClass;
    public Text charName;
    public Text charLevel;
    private void Start()
    {
        if (info != null)
        {
            this.charClass.text = this.info.Class.ToString();
            this.charLevel.text = this.info.Level.ToString() + "级";
            this.charName.text = this.info.Name;
            switch ((int)this.info.Class)
            {
                case 2:
                    this.charClass.text = "法师";
                    break;
                case 1:
                    this.charClass.text = "战士";
                    break;
                case 3:
                    this.charClass.text = "射手";
                    break;
                default:
                    this.charClass.text = "冒险家";
                    break;
            }

        }
    }
}
