using UnityEngine;
using UnityEngine.SceneManagement;

public class AAAA : MonoBehaviour
{
    public void SceneChange(string scene)
    { 
        SceneManager.LoadScene(scene);
    }
}
