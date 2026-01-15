using UnityEngine;
using UnityEngine.UI;

public class SFXController : MonoBehaviour
{
    [SerializeField] Slider slider;

    SoundManager soundManager;
    bool isBound;

    void OnEnable()
    {
        Bind();
    }
    void OnDisable()
    {
        Unbind();
    }
    void Bind()
    {
        if (isBound) return;
        if (slider == null) return;
        soundManager = TryResolveSoundManager();
        if (soundManager == null) return;

        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.SetValueWithoutNotify(soundManager.SFXVolume);

        slider.onValueChanged.RemoveListener(OnSliderChanged);
        slider.onValueChanged.AddListener(OnSliderChanged);

        isBound = true;
    }
    void Unbind()
    {
        slider.onValueChanged.RemoveListener(OnSliderChanged);
        soundManager = null;
        isBound = false;
    }
    void OnSliderChanged(float volume)
    { 
        if(soundManager == null) soundManager = TryResolveSoundManager();
        if (soundManager == null) return;
        soundManager.SetSFXVolume(volume);
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
