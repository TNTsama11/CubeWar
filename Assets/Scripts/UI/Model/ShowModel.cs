using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowModel : UIBase
{
    public static ShowModel Instance = null;

    public Transform ModelPos;
    private Button RightArrowBtn;
    private Button LeftArrowBtn;
    int currentModelId;
    GameObject currentObj;


    void Awake()
	{
        Instance = this;
        Bind(UIEvent.UI_SHOW_MODEL_ARROW, UIEvent.UI_SELECTED_MODEL,UIEvent.UI_SHOW_MODEL_VIEW);
    }
	
    void Start()
    {
        RightArrowBtn = this.transform.Find("RightArrow").GetComponent<Button>();
        RightArrowBtn.gameObject.SetActive(false);
        LeftArrowBtn = this.transform.Find("LeftArrow").GetComponent<Button>();
        LeftArrowBtn.gameObject.SetActive(false);
        currentModelId = PlayerPrefs.GetInt("ModelID");
        ShowPlayerModel(currentModelId);
        RightArrowBtn.onClick.AddListener(ClickRightBtn);
        LeftArrowBtn.onClick.AddListener(ClickLeftBtn);
    }

    void Update()
    {
        
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.UI_SHOW_MODEL_ARROW:
                ShowArrow();
                break;
            case UIEvent.UI_SELECTED_MODEL:
                SelectedModel();
                break;
            case UIEvent.UI_SHOW_MODEL_VIEW:
                SetActive((bool)message);
                break;
            default:
                break;
        }
    }

    public void ShowPlayerModel(int modelId)
    {
        if (currentObj != null)
        {
            Destroy(currentObj);
        }
        GameObject model = Resources.Load("Model/Model" + modelId) as GameObject;
        currentObj = Instantiate(model, ModelPos);
        currentModelId = modelId;
    }

    private void ClickRightBtn()
    {
        currentModelId++;
        if (currentModelId > 5)
        {
            currentModelId = 0;            
        }
        ShowPlayerModel(currentModelId);
    }

    private void ClickLeftBtn()
    {
        currentModelId--;
        if (currentModelId <0)
        {
            currentModelId = 5;
        }
        ShowPlayerModel(currentModelId);
    }

    private void ShowArrow()
    {
        RightArrowBtn.gameObject.SetActive(true);
        LeftArrowBtn.gameObject.SetActive(true);
    }

    private void SelectedModel()
    {
        PlayerPrefs.SetInt("ModelID", currentModelId);
        RightArrowBtn.gameObject.SetActive(false);
        LeftArrowBtn.gameObject.SetActive(false);
    }

    private void SetActive(bool act)
    {
        this.gameObject.SetActive(act);
    }
}
