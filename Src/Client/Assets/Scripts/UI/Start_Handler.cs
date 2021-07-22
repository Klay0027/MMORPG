using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using Models;
using SkillBridge.Message;
using System.Linq;

public class Start_Handler : MonoBehaviour
{
    CharacterClass characterClass;//角色类型
    public GameObject[] charactersPrefabs;//3种角色预制体
    public GameObject createPanel, selectPanel;
    private List<GameObject> uiChars = new List<GameObject>();
    private int selectCharacterIdx = -1;
    //create character panel
    public Button createBtn, back2Create;
    public InputField nickNameInput;//角色昵称输入框
    public Image[] occupationImages;//角色职业显示图片
    public Button[] occupationBtns;// 创建角色--切换职业按钮
    public Text characterDescText;//角色描述文本
    //select character panel
    public Button back2CreateBtn, startBtn;
    public Text nickNameShowText;
    public GameObject characterGroup; //当前用户的所有角色容器
    public GameObject characterPrefabs; //角色信息预制体
    private void Start()
    {
        InitOccupationBtns();
        InitSelectCharacter(true);
        createBtn.onClick.AddListener(OnClickCreate);
        startBtn.onClick.AddListener(OnClickStart);
        back2Create.onClick.AddListener(OnBack2Create);
        UserServices.Instance.OnCreateChar = OnCharacterCreate;
        UserServices.Instance.OnGameEnter = OnPlayerEnterGame;
    }

    /// <summary>
    /// 初始化选择的角色
    /// </summary>
    /// <param name="isInit"></param>
    private void InitSelectCharacter(bool isInit)
    {
        createPanel.SetActive(false);
        selectPanel.SetActive(true);

        if (isInit)
        {
            foreach (var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();

            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
                GameObject game = Instantiate(characterPrefabs, characterGroup.transform);
                CharacterInfo charInfo = game.GetComponent<CharacterInfo>();
                charInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = game.GetComponent<Button>();
                int index = i;
                button.onClick.AddListener(() => { OnSelectCharacter(index); });
                uiChars.Add(game);
            }
        }

        //OnSelectCharacter(0);
    }

    /// <summary>
    /// 跳转到创建界面
    /// </summary>
    private void OnBack2Create()
    {
        SkipPanel(selectPanel, createPanel);
    }

    /// <summary>
    /// 点击选择角色
    /// </summary>
    /// <param name="idx"></param>
    private void OnSelectCharacter(int idx)
    {
        this.selectCharacterIdx = idx;
        User.Instance.CurrentCharacter = User.Instance.Info.Player.Characters[idx];
        int charIndex = (int)User.Instance.CurrentCharacter.Class;
        nickNameShowText.text = User.Instance.CurrentCharacter.Name;
        for (int i = 0; i < charactersPrefabs.Length; i++)
        {
            if (i == charIndex - 1)
            {
                charactersPrefabs[i].SetActive(true);
            }
            else
            {
                charactersPrefabs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 初始化选择职业按钮
    /// </summary>
    private void InitOccupationBtns()
    {
        for (int i = 0; i < occupationBtns.Length; i++)
        {
            Button btn = occupationBtns[i];
            btn.GetComponent<Button>().onClick.AddListener(delegate () { this.OnSelectOccupation(btn); });
        }
    }

    /// <summary>
    /// 点击选择职业
    /// </summary>
    /// <param name="btnIndex"></param>
    private void OnSelectOccupation(Button btnIndex)
    {
        int index = occupationBtns.ToList().IndexOf(btnIndex);

        this.characterClass = (CharacterClass)(index + 1);
        for (int i = 0; i < charactersPrefabs.Length; i++)
        {
            if (i == index)
            {
                charactersPrefabs[i].SetActive(true);
                occupationImages[i].gameObject.SetActive(true);
                characterDescText.text = DataManager.Instance.Characters[index + 1].Description;

            }
            else
            {
                charactersPrefabs[i].SetActive(false);
                occupationImages[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 点击创建角色
    /// </summary>
    private void OnClickCreate()
    {
        if (string.IsNullOrEmpty(nickNameInput.text))
        {
            MessageBox.Show("请输入您的昵称，冒险家");
            return;
        }
        UserServices.Instance.SendCreateCharacter(this.nickNameInput.text, this.characterClass);
    }

    /// <summary>
    /// 收到服务器创建角色的回复
    /// </summary>
    /// <param name="result">创建结果</param>
    /// <param name="message">提示或错误信息</param>
    private void OnCharacterCreate(Result result, string message)
    {
        if (result == Result.Success)
        {
            MessageBox.Show("创建角色成功！");
            //跳转到选择角色界面
            InitSelectCharacter(true);
            SkipPanel(createPanel, selectPanel);
        }
        else
        {
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
    }

    /// <summary>
    /// 收到服务器传回 是否可以进入游戏的结果
    /// </summary>
    /// <param name="result"></param>
    /// <param name="message"></param>
    private void OnPlayerEnterGame(Result result, string message)
    {
        if (result == Result.Success)
        {
            SceneManager.Instance.LoadScene("MainCity");
        }
        else
        {
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
    }

    /// <summary>
    /// 点击进入主城 开始游戏
    /// </summary>
    private void OnClickStart()
    {
        //MessageBox.Show("进入游戏", "进入游戏", MessageBoxType.Confirm);
        //selectPanel.SetActive(false);
        if (selectCharacterIdx >= 0)
        {
            UserServices.Instance.SendEnterGame(selectCharacterIdx);
        }
    }

    /// <summary>
    /// 跳转界面
    /// </summary>
    /// <param name="currentPanel"></param>
    /// <param name="nextPanel"></param>
    private void SkipPanel(GameObject currentPanel, GameObject nextPanel)
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
    }

}
