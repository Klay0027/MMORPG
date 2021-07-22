using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Services;
using SkillBridge.Message;
using Models;
using Managers;

public class GameObjectManager : MonoBehaviour 
{
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    private void Start()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter; //添加角色进入事件
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter = null;
    }

    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }


    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.Info.Id) || Characters[character.Info.Id] == null) //当前编号的角色不存在或者角色对象为空时才可以创建
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource); //character.Define.Resource 读取配置表中的资源路径

            if (obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.", character.Define.TID, character.Define.Resource);
                return;
            }

            GameObject go = (GameObject)Instantiate(obj); //实例化角色对象
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name; //为角色对象添加名字
            go.transform.position = GameObjectTool.LogicToWorld(character.position); //转换为世界坐标
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction); 

            Characters[character.Info.Id] = go;

            EntityController ec = go.GetComponent<EntityController>(); //获取当前角色的实体控制脚本

            if (ec != null) //如果实体脚本不为空
            {
                ec.entity = character; //将当前角色 赋值 给实体脚本中的角色
                ec.isPlayer = character.IsPlayer; //
            }

            PlayerInputController pc = go.GetComponent<PlayerInputController>(); //获取当前角色的控制脚本

            if (pc != null)
            {
                if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id) //如果是当前选择的对象
                {
                    User.Instance.CurrentCharacterObject = go;
                    MainPlayerCamera.Instance.player = go;
                    pc.enabled = true;
                    pc.character = character;
                    pc.entityController = ec;
                }
                else
                {
                    pc.enabled = false;
                }
            }
            //添加显示角色名称和等级的UI
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
    
    }
}
