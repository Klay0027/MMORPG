using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading_Handler : MonoBehaviour
{
    public GameObject Loading_Panel, Login_Panel;
    public GameObject Start_Bg, TipsGame_Bg, Loading_Group;
    public Image Loading_Fill;
    private bool isLoading;

    private void Start()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        StartCoroutine(DataManager.Instance.LoadData());//读取json信息

        Loading_Fill.fillAmount = 0;
        isLoading = false;
        Start_Bg.SetActive(true);
        TipsGame_Bg.SetActive(false);
        Loading_Group.SetActive(false);

        StartCoroutine(Load());

    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(3.5f);
        TipsGame_Bg.SetActive(false);
        Loading_Group.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        isLoading = true;

        //Init basic services
        UserServices.Instance.Init();
        MapService.Instance.Init();

        TestManager.Instance.Init();
    }

    private void Update()
    {
        CountDown();
    }

    private void CountDown()
    {
        if (isLoading)
        {
            if (Loading_Fill.fillAmount < 1)
            {
                Loading_Fill.fillAmount += Time.deltaTime / 2.5f;
            }
            else
            {              
                Loading_Panel.SetActive(false);
                Login_Panel.SetActive(true);
            }
        }
    }
}
