using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;
using Common.Data;
using System.Collections.Generic;
using UnityEngine;

public class MapTools 
{
    [MenuItem("Map Tools/Export Teleporters")]
    public static void ExportTeleporters()
    {
        DataManager.Instance.Load();

        //获取当前场景是哪一个
        Scene current = EditorSceneManager.GetActiveScene();
        string currentScene = current.name;
        //检查场景有没有保存 
        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "确定");
            return;
        }

        List<TeleporterObject> allTeleporters = new List<TeleporterObject>();

        foreach (var map in DataManager.Instance.Maps)
        {
            //获取到目录下每个场景的原始路径
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("Scene {0} not existed!", sceneFile);
                continue;
            }
            //使用编辑器中的场景管理 打开当前场景
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);
            //获取到当前场景中的所有传送点
            TeleporterObject[] teleporters = GameObject.FindObjectsOfType<TeleporterObject>();
            foreach (var teleporter in teleporters)
            {
                //检查当前传送点是否和配置表中的传送点一致
                if (!DataManager.Instance.Teleporters.ContainsKey(teleporter.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图：{0} 中配置的 Teleporter:[{1}] 中不存在", map.Value.Resource, teleporter.ID), "确定");
                    return;
                }

                TeleporterDefine def = DataManager.Instance.Teleporters[teleporter.ID];
                //检查地图ID是否一致
                if (def.MapID != map.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图：{0} 中配置的 Teleporter:[{1}] MapID:{2} 错误", map.Value.Resource, teleporter.ID, def.MapID), "确定");
                    return;
                }
                //赋值方向和位置
                def.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                def.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
            }
        }
        //将修改后的传送点配置表信息保存
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完成", "确定");
    }
}
