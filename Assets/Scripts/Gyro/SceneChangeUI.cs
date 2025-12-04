using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeUI : MonoBehaviour
{
    public void ChangeScene()
    {
        if (SceneManager.GetActiveScene().name == "BackView_Forward") SceneManager.LoadScene("SideView_ToRight");
        else if (SceneManager.GetActiveScene().name == "SideView_ToRight") SceneManager.LoadScene("BackView_Forward");
        else Debug.Log("Error");
    }
}
