using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameObjectTool 
{
    /// <summary>
    /// 转世界坐标1
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 LogicToWorld(NVector3 vector) 
    {
        return new Vector3(vector.X / 100f, vector.Z / 100f, vector.Y / 100f);
    }

    /// <summary>
    /// 转世界坐标2
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 LogicToWorld(Vector3Int vector)
    {
        return new Vector3(vector.x / 100f, vector.z / 100f, vector.y / 100f);
    }

    /// <summary>
    /// 转世界坐标3
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static float LogicToWorld(int val)
    {
        return val / 100f;
    }

    /// <summary>
    /// 世界坐标转逻辑坐标
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int WorldToLogic(float val)
    {
        return Mathf.RoundToInt(val * 100f);
    }

    /// <summary>
    /// 世界坐标转逻辑坐标2
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3Int WorldToLogic(Vector3 vector)
    {
        return new Vector3Int()
        {
            x = Mathf.RoundToInt(vector.x * 100),
            y = Mathf.RoundToInt(vector.z * 100),
            z = Mathf.RoundToInt(vector.y * 100)
        };
    }

    /// <summary>
    /// 世界坐标转逻辑坐标3
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static NVector3 WorldToLogicN(Vector3 vector)
    {
        return new NVector3()
        {
            X = Mathf.RoundToInt(vector.x * 100),
            Y = Mathf.RoundToInt(vector.z * 100),
            Z = Mathf.RoundToInt(vector.y * 100)
        };
    }

    public static bool EntityUpdate(NEntity entity, UnityEngine.Vector3 position, Quaternion rotation, float speed)
    {
        NVector3 pos = WorldToLogicN(position);
        NVector3 dir = WorldToLogicN(rotation.eulerAngles);
        int spd = WorldToLogic(speed);
        bool updated = false;
        if (!entity.Position.Equal(pos))
        {
            entity.Position = pos;
            updated = true;
        }
        if (!entity.Direction.Equal(dir))
        {
            entity.Direction = dir;
            updated = true;
        }
        if (entity.Speed != spd)
        {
            entity.Speed = spd;
            updated = true;
        }

        return updated;
    }

}
