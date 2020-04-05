using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommunicationProtocol.Dto;
using CommunicationProtocol.Code;

public class MainPanel : UIBase
{
    private Image playerExpImg;
    private Image PlayerIconImg;
    private Text playerNameText;
    private Text playerIDText;
    private Button ChangeIconBtn;
    private Button ChangeNameBtn;
    private Button MatchBtn;
    private Button ModelBtn;
    private Button SettingBtn;
    private Button ExitBtn;
    private Image TitleImg;
    private InputField playerNameInput;

	void Awake()
	{
        playerExpImg = this.transform.Find("PlayerExpImg").GetComponent<Image>();
        PlayerIconImg = this.transform.Find("PlayerIconImg").GetComponent<Image>();
        playerNameText = this.transform.Find("PalyerNameText").GetComponent<Text>();
        playerIDText = this.transform.Find("PalyerIDText").GetComponent<Text>();
        ChangeIconBtn = this.transform.Find("ChangeIconBtn").GetComponent<Button>();
        ChangeNameBtn = this.transform.Find("ChangeNameBtn").GetComponent<Button>();
        MatchBtn = this.transform.Find("MatchBtn").GetComponent<Button>();
        ModelBtn = this.transform.Find("ModelBtn").GetComponent<Button>();
        SettingBtn = this.transform.Find("SettingBtn").GetComponent<Button>();
        ExitBtn = this.transform.Find("ExitBtn").GetComponent<Button>();
        TitleImg = this.transform.Find("TitleImg").GetComponent<Image>();
        playerNameInput = this.transform.Find("PlayerNameInput").GetComponent<InputField>();

        Bind(UIEvent.UI_CHANGE_ID, UIEvent.UI_HIDE_MAIN_MENU,
            UIEvent.UI_SHOW_MAIN_MENU,UIEvent.UI_CHANGE_ICON,
            UIEvent.UI_REFRESH);
	}
	
	void Start () 
	{
       // InitMain();
        ChangeIconBtn.onClick.AddListener(OpenIconPanel);
        ChangeNameBtn.onClick.AddListener(ChangeName);
        MatchBtn.onClick.AddListener(StartMtch);
        ModelBtn.onClick.AddListener(Model);
        SettingBtn.onClick.AddListener(Settings);
        ExitBtn.onClick.AddListener(Exit);

        playerNameInput.onEndEdit.AddListener(delegate {
            if (playerNameInput.textComponent.text!=string.Empty&& playerNameInput.textComponent.text.Length<=8)
            {
                playerNameText.text = playerNameInput.textComponent.text;
                playerNameInput.gameObject.SetActive(false);
                ChangeNameBtn.gameObject.SetActive(true);
                PlayerPrefs.SetString("Name", playerNameText.text);
            }
            else
            {
                playerNameInput.gameObject.SetActive(false);
                ChangeNameBtn.gameObject.SetActive(true);
                playerNameText.text = PlayerPrefs.GetString("Name");
            }
        });
        
    }

	void Update () 
	{
		
	}

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_CHANGE_ID:
                string id = message.ToString();
                ChangeID(id);
                break;
            case UIEvent.UI_SHOW_MAIN_MENU:
                MainSetActive((bool)message);
                break;
            case UIEvent.UI_HIDE_MAIN_MENU:
                MainSetActive((bool)message);
                break;
            case UIEvent.UI_CHANGE_ICON:
                ChangeIcon((int)message);
                break;
            case UIEvent.UI_REFRESH:
                Refresh(message as UserDto);
                break;
            default: break;
        }
    }
    /// <summary>
    /// 根据数据刷新主菜单视图
    /// </summary>
    private void Refresh(UserDto dto)
    {
        playerNameText.text = dto.Name;
        Debug.LogWarning(dto.Name);
        playerExpImg.transform.Find("Text").GetComponent<Text>().text = "级别：" + dto.Lv;
        ChangeIcon(dto.IconID);
        ChangeModel(dto.ModelID);
        SaveUserData(dto);
    }

    private void SaveUserData(UserDto dto)
    {
        PlayerPrefs.SetString("Name", dto.Name);
        PlayerPrefs.SetInt("LV", dto.Lv);
        PlayerPrefs.SetInt("IconID", dto.IconID);
        PlayerPrefs.SetInt("ModelID", dto.ModelID);
    }

    private void ChangeModel(int index)
    {
        ShowModel.Instance.ShowPlayerModel(index);
    }

    private void OpenIconPanel()
    {
        //打开头像选择面板
        Dispatch(AreaCode.UI,UIEvent.UI_SHOW_ICON,true);
    }

    private void ChangeIcon(int id)
    {
        //Debug.Log("主界面改变ICON"+id);
        Sprite sprite = Resources.Load("Icon/Icon" + id, typeof(Sprite)) as Sprite;
        PlayerIconImg.sprite = sprite; 
        
    }

    private void ChangeName()
    {      
            playerNameInput.gameObject.SetActive(true);
        ChangeNameBtn.gameObject.SetActive(false);
    }

    private void ChangeID(string id)
    {
        playerIDText.text = "ID:" + id;
        PlayerPrefs.SetString("ID", id);
    }

    private void StartMtch()
    {
        Dispatch(AreaCode.UI, UIEvent.UI_SHOW_MODEL_VIEW, false);
        MainSetActive(false);
        UpLoadUserInfo();
        Dispatch(AreaCode.UI,UIEvent.UI_SHOW_MATCH_MENU,true);
        
    }

    /// <summary>
    /// 向服务器上传本地玩家数据
    /// </summary>
    private void UpLoadUserInfo()
    {
        UserDto userDto = new UserDto();
        userDto.Account = PlayerPrefs.GetString("ID");
        userDto.Name = PlayerPrefs.GetString("Name");
        userDto.IconID = PlayerPrefs.GetInt("IconID");
        userDto.ModelID = PlayerPrefs.GetInt("ModelID");
        userDto.Lv = PlayerPrefs.GetInt("Kill")/2;
        SocketMessage smg = new SocketMessage(OpCode.USER, UserCode.USER_UPLOADINFO_CREQ, userDto);
        Dispatch(AreaCode.NET, 0, smg);
    }

    bool isModelSelect = false;
    private void Model()
    {
        if (!isModelSelect)
        {
            ModelBtn.transform.Find("Text").GetComponent<Text>().text = "我是确认按钮";
            Dispatch(AreaCode.UI, UIEvent.UI_SHOW_MODEL_ARROW, null);
            isModelSelect = true;
        }
        else
        {
            ModelBtn.transform.Find("Text").GetComponent<Text>().text = "角色选择";
            Dispatch(AreaCode.UI, UIEvent.UI_SELECTED_MODEL, null);
            isModelSelect = false;
        }

    }

    private void Settings()
    {
        Debug.Log("SettingPanle");
        MainSetActive(false);
        Dispatch(AreaCode.UI, UIEvent.UI_SHOW_SETTING_MENU, true);
    }

    private bool isExit;
    private void Exit()
    {
        if (isExit)
        {
            Application.Quit();
            NetManager.Instance.OffLine();
        }
        else
        {
            ShowToast.MakeToast("再点一次就退出了");
            isExit = true;
            StartCoroutine(CountDown());
        }
    }

    IEnumerator CountDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            isExit = false;
        }
    }

    private void MainSetActive(bool active)
    {
        playerExpImg.gameObject.SetActive(active);
        PlayerIconImg.gameObject.SetActive(active);
        playerNameText.gameObject.SetActive(active);
        playerIDText.gameObject.SetActive(active);
        ChangeIconBtn.gameObject.SetActive(active);
        ChangeNameBtn.gameObject.SetActive(active);
        MatchBtn.gameObject.SetActive(active);
        ModelBtn.gameObject.SetActive(active);
        SettingBtn.gameObject.SetActive(active);
        ExitBtn.gameObject.SetActive(active);
        TitleImg.gameObject.SetActive(active);
       
    }
    /// <summary>
    /// 初始化玩家数据
    /// </summary>
    private void InitMain()
    {
        if (PlayerPrefs.HasKey("Name"))
        {
            playerNameText.text = PlayerPrefs.GetString("Name");
        }
        else
        {
            PlayerPrefs.SetString("Name", "起一个名字真费劲");
            playerNameText.text = PlayerPrefs.GetString("Name");
        }

        if (PlayerPrefs.HasKey("Kill"))
        {
            playerExpImg.transform.Find("Text").GetComponent<Text>().text = "级别：" + (PlayerPrefs.GetInt("Kill")/2).ToString();
        }
        else
        {
            PlayerPrefs.SetInt("Kill", 0);
            playerExpImg.transform.Find("Text").GetComponent<Text>().text = "级别："+ (PlayerPrefs.GetInt("Kill") / 2).ToString();
        }

        if (PlayerPrefs.HasKey("IconID"))
        {
            ChangeIcon(PlayerPrefs.GetInt("IconID"));
        }
        else
        {
            //Debug.Log("init头像");
            ChangeIcon(1);
            PlayerPrefs.SetInt("IconID", 1);
        }

        if (!PlayerPrefs.HasKey("ModelID"))
        {
            PlayerPrefs.SetInt("ModelID", 0);
        }

    }

}
