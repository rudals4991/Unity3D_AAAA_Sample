using TMPro;
using UnityEngine;

public class UITest : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GyroTest gyro;

    private void Update()
    {
        text.text = gyro.GetHorizontalTilt().ToString();
    }
}
