using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 存储按钮信息的类
/// </summary>
public class ButtonInfo
{
    public int id;
    public GameObject obj;
}

public class IconPanel : UIBase
{
    private Image BackGroundImg;
    [SerializeField]
    private Button[] IconBtn=new Button[8];

	void Awake()
	{
        Bind(UIEvent.UI_SHOW_ICON);
        BackGroundImg = this.transform.Find("BackGroundImg").GetComponent<Image>();
        for (int i = 0; i < 8; i++)
        {
            IconBtn[i] = this.transform.Find("IconBtn" + i).GetComponent<Button>();
        }
        for (int i = 0; i < IconBtn.Length; i++) //给按钮动态绑定带参方法
        {
            ButtonInfo bif = new ButtonInfo();
            bif.id = i;
            bif.obj = IconBtn[i].gameObject;
            IconBtn[i].onClick.AddListener(()=> {
                OnIconBtnClick(bif);
            });
        }
	}
	
	void Start () 
	{
        IconSetActive(false);
	}

	void Update () 
	{
		
	}

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_SHOW_ICON:
                IconSetActive((bool)message);
                break;
            default:
                break;
        }
    }

    private void IconSetActive(bool active)
    {
        BackGroundImg.gameObject.SetActive(active);
        foreach(var item in IconBtn)
        {
            item.gameObject.SetActive(active);
        }
    }
    private GameObject tempBtn;
    private void  OnIconBtnClick(ButtonInfo btnInfo)
    {
        if (tempBtn != btnInfo.obj&&tempBtn!=null)
        {
            tempBtn.GetComponent<Outline>().enabled = false;
            tempBtn = btnInfo.obj;
            btnInfo.obj.GetComponent<Outline>().enabled = true;           
        }
        else if(tempBtn==null)
        {
            tempBtn = btnInfo.obj;
            btnInfo.obj.GetComponent<Outline>().enabled = true;
        }
        else
        {
            IconSetActive(false);
            PlayerPrefs.SetInt("IconID", btnInfo.id);
            Debug.Log("选择了头像" + btnInfo.id);
            Dispatch(AreaCode.UI, UIEvent.UI_CHANGE_ICON, btnInfo.id); //将头像ID保存本地并传递给主菜单
        }

    }    
}
