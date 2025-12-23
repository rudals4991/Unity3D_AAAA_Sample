using UnityEngine;

public class TestUI_Scene : MonoBehaviour
{
    public void Load()
    {
        DIContainer.Resolve<GameFlowManager>().GameStart();
    }
}
