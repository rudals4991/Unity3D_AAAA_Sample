using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    [SerializeField] Slider slider;
    SoundManager soundManager;
    bool isBinding;
    private void OnEnable()
    {
        Bind();
    }
    private void OnDisable()
    {
        Unbind();
    }
    void Bind()
    {
        if (isBinding) return;
        if (slider == null) return;
        soundManager = TryResolveSoundManager();
        if (soundManager == null) return;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.SetValueWithoutNotify(soundManager.BGMVolume);
        slider.onValueChanged.RemoveListener(OnValueChanged);
        slider.onValueChanged.AddListener(OnValueChanged);
        isBinding = true;
    }
    void Unbind()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
        isBinding = false;
        soundManager = null;
    }
    void OnValueChanged(float value)
    {
        if (soundManager == null)
            soundManager = TryResolveSoundManager();

        if (soundManager == null) return;

        soundManager.SetBGMVolume(value);
    }
    SoundManager TryResolveSoundManager()
    {
        try
        {
            return DIContainer.Resolve<SoundManager>();
        }
        catch
        {
            return null;
        }
    }
}
