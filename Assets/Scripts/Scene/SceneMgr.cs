using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 场景管理类
/// </summary>
    public class SceneMgr:ManagerBase
    {
    public static SceneMgr Instance = null;

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Add(SceneEvent.SCENE_LOAD, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.SCENE_LOAD:
                SceneMesg sMesg = message as SceneMesg;
                LoadScene(sMesg);
                break;
            default: break;
        }
    }

    private Action onSceneLoaded = null;

    /// <summary>
    /// 加载场景并且执行加载完成后的回调
    /// </summary>
    /// <param name="sMesg"></param>
    private void LoadScene(SceneMesg sMesg)
    {
        if (sMesg.index != -1)
        {
            SceneManager.LoadScene(sMesg.index);
        }else if (sMesg.name!=null)
        {
            SceneManager.LoadScene(sMesg.name);
        }
        if (sMesg.onSceneLoaded != null)
        {
            onSceneLoaded = sMesg.onSceneLoaded;
        }
    }

    /// <summary>
    /// 场景加载完成时调用
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    private void OnSceneLoaded(Scene scene,LoadSceneMode loadSceneMode)
    {
        if (onSceneLoaded!= null)
        {
            onSceneLoaded();
        }
    }
}

