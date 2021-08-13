using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElement : MonoBehaviour
{
    public Transform owner;
    public float height;

    private void Start()
    {
        height = 2.05f;
    }

    private void Update()
    {
        if (owner != null)
        {
            this.transform.position = owner.position + Vector3.up * height;
        }
    }
}
