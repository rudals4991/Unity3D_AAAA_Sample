using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IManagerBase
{
    public int Priority => 4;
    List<IUIBase> uiList = new();

    public void Exit()
    {
        MySceneManager.OnSceneChanged -= SetUIActive;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        MySceneManager.OnSceneChanged -= SetUIActive;
        MySceneManager.OnSceneChanged += SetUIActive;
        RefreshUIList();
        InitializeUIs();
        SetUIActive(MySceneManager.Instance.CurrentScene);
    }
    public void RefreshUIList()
    {
        uiList.Clear();

        string uiName = MySceneManager.Instance.UISceneName;
        var uiScene = SceneManager.GetSceneByName(uiName);
        if (!uiScene.isLoaded) return;
        foreach (var root in uiScene.GetRootGameObjects())
        {
            uiList.AddRange(root.GetComponentsInChildren<IUIBase>(true));
        }
    }
    void SetUIActive(SceneList scene)
    {
        if (uiList.Count == 0)
        {
            RefreshUIList();
            InitializeUIs();
        }
        for (int i = uiList.Count - 1; i >= 0; i--)
        {
            if (uiList[i] is not MonoBehaviour mb || mb == null) uiList.RemoveAt(i);
        }
        foreach (var ui in uiList)
        {
            var mb = (MonoBehaviour)ui;
            if (mb == null) continue;
            bool visible = true;
            if (ui is IVisibleUI visi) visible = visi.IsVisible(scene);
            mb.gameObject.SetActive(visible);
            if (!visible) ui.SetActiveFalse();
        }
    }
    void InitializeUIs()
    {
        foreach (IUIBase ui in uiList)
        {
            ui.Initialize();
        }
    }
}
