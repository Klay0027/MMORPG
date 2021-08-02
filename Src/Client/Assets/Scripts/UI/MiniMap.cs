using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class MiniMap : MonoBehaviour
{
    public Image miniMap;
    public Image arrow;
    public Text mapName;

    public Collider MinimapBoundingBox;

    private Transform playerTransfrom;

    private void Start()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;


        if (this.miniMap.overrideSprite == null)
        {
            this.miniMap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        }

        this.miniMap.SetNativeSize();
        this.miniMap.transform.localPosition = Vector3.zero;

    }

    private void Update()
    {
        if (playerTransfrom == null)
        {
            playerTransfrom = MinimapManager.Instance.PlayerTransform;
        }

        if (MinimapBoundingBox == null || playerTransfrom == null)
        {
            return;
        }
        //获取当前地图的宽和高
        float realWidth = MinimapBoundingBox.bounds.size.x;
        float realHeight = MinimapBoundingBox.bounds.size.z;

        //角色在地图中的相对位置
        float relaX = playerTransfrom.position.x - MinimapBoundingBox.bounds.min.x;
        float relaY = playerTransfrom.position.z - MinimapBoundingBox.bounds.min.z;

        //获取中心点的坐标
        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        this.miniMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.miniMap.rectTransform.localPosition = Vector3.zero;

        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransfrom.eulerAngles.y);
    }
}
