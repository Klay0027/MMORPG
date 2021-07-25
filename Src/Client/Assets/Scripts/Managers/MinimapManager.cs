﻿using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {
        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/MainCity/MiniMap/" + User.Instance.CurrentMapData.MiniMap);
        }
    }
}
