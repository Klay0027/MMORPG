using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {
        public MiniMap minimap;

        private Collider minimapBoundingBox;

        public Collider MinimapBoundingBox
        {
            get 
            {
                return minimapBoundingBox;
            }
        }
        public Transform PlayerTransform
        {
            get 
            {
                if (User.Instance.CurrentCharacterObject == null)
                {
                    return null;
                }
                return User.Instance.CurrentCharacterObject.transform;
            }
        
        }

        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/MainCity/MiniMap/" + User.Instance.CurrentMapData.MiniMap);
        }

        public void UpdateMinimap(Collider minimapBoundingBox)
        {
            this.minimapBoundingBox = minimapBoundingBox;
            if (this.minimap != null)
            {
                this.minimap.UpdateMap();
            }

        }
    }
}
