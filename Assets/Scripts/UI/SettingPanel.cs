using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : UIBase
{

    private Button ArchiveBtn;
    private Button BackBtn;
    private Button CopyCodeBtn;

	void Awake()
	{
        Bind(UIEvent.UI_HIDE_SETTING_MENU, UIEvent.UI_SHOW_SETTING_MENU);

        ArchiveBtn = this.transform.Find("ArchiveBtn").GetComponent<Button>();
        BackBtn = this.transform.Find("BackBtn").GetComponent<Button>();
        CopyCodeBtn = this.transform.Find("CopyCodeBtn").GetComponent<Button>();
        ArchiveBtn.onClick.AddListener(ClearArchive);
        BackBtn.onClick.AddListener(Back);
        CopyCodeBtn.onClick.AddListener(CopyCode);
        SettingSetActive(false);
    }

    private void CopyCode()
    {
        GUIUtility.systemCopyBuffer = PlayerPrefs.GetString("ID");
        ShowToast.MakeToast("已复制到剪贴板");
    }

    void Start () 
	{
		
	}

	void Update () 
	{
		
	}

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_SHOW_SETTING_MENU:
                Debug.Log(message);
                SettingSetActive((bool)message);
                break;
            case UIEvent.UI_HIDE_SETTING_MENU:
                SettingSetActive((bool)message);
                break;
            default:
                break;
        }
    }

    private void Back()
    {
        SettingSetActive(false);
        Dispatch(AreaCode.UI, UIEvent.UI_SHOW_MAIN_MENU,true);
    }

    private void ClearArchive()
    {
        PlayerPrefs.SetString("ID", "0");
        Restart(500);
    }
    
    private void SettingSetActive(bool active)
    {
        ArchiveBtn.gameObject.SetActive(active);
        BackBtn.gameObject.SetActive(active);
        CopyCodeBtn.gameObject.SetActive(active);
    }

    public static void Restart(int delay)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        mainActivity.Call("doRestart", delay);
        jc.Dispose();
        mainActivity.Dispose();
    }

}
