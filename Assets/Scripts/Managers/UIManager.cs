using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        uiList.AddRange(GetComponentsInChildren<IUIBase>(true));
        MySceneManager.OnSceneChanged -= SetUIActive;
        MySceneManager.OnSceneChanged += SetUIActive;
    }
    void SetUIActive(SceneList scene)
    { 
        //TODO: Scene에 따른 UI 활성화 결정
    }
    void InitializeUIs()
    {
        foreach (IUIBase ui in uiList)
        {
            ui.Initialize();
        }
    }
}
