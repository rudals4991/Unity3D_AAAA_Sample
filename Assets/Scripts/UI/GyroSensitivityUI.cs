using UnityEngine;
using UnityEngine.UI;

public class GyroSensitivityUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    float minSens = 0.6f;
    float maxSens = 1.8f;
    float defaultValue = 1.2f;
    void Awake()
    {
        slider.minValue = minSens;
        slider.maxValue = maxSens;
        float saved = PlayerPrefs.GetFloat("GyroSen", defaultValue);
        saved = Mathf.Clamp(saved, minSens, maxSens);
        slider.SetValueWithoutNotify(saved);
        slider.onValueChanged.RemoveListener(OnChanged);
        slider.onValueChanged.AddListener(OnChanged);
    }
    void OnChanged(float value)
    {
        value = Mathf.Clamp(value, minSens, maxSens);
        PlayerPrefs.SetFloat("GyroSens", value);
        PlayerPrefs.Save();
    }
}
