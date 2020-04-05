using System;
using System.Collections.Generic;
using System.Linq;

    public class SceneMesg
    {
    public int index;
    public string name;
    public Action onSceneLoaded;

    public SceneMesg()
    {
        index = -1;
        name = null;
        onSceneLoaded = null;
    }
    public SceneMesg(int index,Action onLoaded)
    {
        this.index = index;
        this.name = null;
        this.onSceneLoaded = onLoaded;
    }
    public SceneMesg(string name, Action onLoaded)
    {
        this.index = -1;
        this.name = name;
        this.onSceneLoaded = onLoaded;
    }
    public void Change(int index, string name, Action onLoaded)
    {
        this.index = index;
        this.name = name;
        this.onSceneLoaded = onLoaded;
    }

}


