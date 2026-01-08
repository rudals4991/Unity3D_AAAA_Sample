using UnityEngine;

public class TestUI_Scene : MonoBehaviour
{
    public void Load()
    {
        DIContainer.Resolve<GameFlowManager>().GameStart();
    }
    public void Quit()
    { 
        Application.Quit();
    }
    public void Regame()
    {
        DIContainer.Resolve<GameFlowManager>().GameRestart();
    }
    public void OpenPanel(GameObject obj)
    { 
        obj.SetActive(true);
    }
    public void ClosePanel(GameObject obj)
    {
        obj.SetActive(false);
    }
}
