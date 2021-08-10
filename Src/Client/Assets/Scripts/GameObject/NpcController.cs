using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public int npcID;

    SkinnedMeshRenderer renderer;

    Animator animator;

    Color orignColor;

    private bool inInteractive = false;

    NpcDefine npc;

    private void Start()
    {
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        animator = this.gameObject.GetComponentInChildren<Animator>();
        orignColor = renderer.sharedMaterial.color;
        npc = NpcManager.Instance.GetNpcDefine(this.npcID);
        this.StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
            this.Relax();
        }
    
    }

    private void Relax()
    {
        animator.SetTrigger("Relax");
    }

    private void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(npc))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }


    private void OnMouseDown()
    {
        Interactive();
        Debug.Log("点我干嘛！我是" + this.gameObject.name);
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    private void Highlight(bool highlight)
    {
        if (highlight)
        {
            Debug.Log(renderer.sharedMaterial.color);
            if (renderer.sharedMaterial.color != Color.white)
            {
                renderer.sharedMaterial.color = Color.white;
            }
        }
        else
        {
            if (renderer.sharedMaterial.color != orignColor)
            {
                renderer.sharedMaterial.color = orignColor;
            }
        }
    }
}
