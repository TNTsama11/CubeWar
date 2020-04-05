using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommunicationProtocol.Code;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GameMenuPanel : UIBase
{
    private Toggle VolumeTg;
    private Button ExitBtn;
    public GameObject MenuHidePos;
    public GameObject MenuBtn;
    public GameObject MenuBtnHidePos;
	
	void Awake()
	{
        VolumeTg = this.transform.Find("VolumeTg").GetComponent<Toggle>();
        ExitBtn = this.transform.Find("ExitBtn").GetComponent<Button>();
	}
	
    void Start()
    {
        ExitBtn.onClick.AddListener(Exit);
        VolumeTg.onValueChanged.AddListener(isSelect => 
        {
            if (isSelect)
            {
                Camera.main.GetComponent<AudioSource>().mute = true;
            }
            else
            {
                Camera.main.GetComponent<AudioSource>().mute = false;
            }
        });
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
#if UNITY_ANDROID
            if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if (EventSystem.current.currentSelectedGameObject.tag != "Menu")
                {
                    HideMenu();
                }
            }
#else
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (EventSystem.current.currentSelectedGameObject!=null)
                {
                    if(EventSystem.current.currentSelectedGameObject.tag != "Menu")
                    {
                        HideMenu();
                    }
                    
                }
            }
            else
            {
                HideMenu();
            }

#endif
        }

    }

    private void Exit()
    {

        SceneMesg sceneMesg = new SceneMesg(1, () =>
          {
              string id = PlayerPrefs.GetString("ID");
              Dispatch(AreaCode.UI, UIEvent.UI_CHANGE_ID, id);
          });
        SocketMessage smg = new SocketMessage(OpCode.GAME, GameCode.GAME_EXIT_CERQ, null);
        Dispatch(AreaCode.NET, 0, smg);
        Dispatch(AreaCode.SCENE, SceneEvent.SCENE_LOAD, sceneMesg);
    }

    private void HideMenu()
    {
        if (this.gameObject.activeSelf)
        {
            MenuBtn.transform.DOLocalRotate(MenuBtnHidePos.transform.localRotation.eulerAngles, 0.5f);
            Tween tween= this.transform.DOLocalMove(MenuHidePos.transform.localPosition, 0.5f);
            tween.OnComplete(()=> 
            {
                this.gameObject.SetActive(false);
            });           
            return;
        }
    }
}
