using UnityEngine;

public class TestUI_Scene : MonoBehaviour
{
    public void Load()
    {
        MySceneManager.Instance.LoadSingleScene(SceneList.GamePlay);
    }
}
