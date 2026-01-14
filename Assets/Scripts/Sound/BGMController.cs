using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
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
        if (bgmSlider == null) return;
        soundManager = TryResolveSoundManager();
        if (soundManager == null) return;
        bgmSlider.minValue = 0f;
        bgmSlider.maxValue = 1f;
        bgmSlider.SetValueWithoutNotify(soundManager.BgmVolume);
        bgmSlider.onValueChanged.RemoveListener(OnValueChanged);
        bgmSlider.onValueChanged.AddListener(OnValueChanged);
        isBinding = true;
    }
    void Unbind()
    {
        if (bgmSlider != null)  bgmSlider.onValueChanged.RemoveListener(OnValueChanged);
        isBinding = false;
        soundManager = null;
    }
    void OnValueChanged(float value)
    {
        if (soundManager == null)
            soundManager = TryResolveSoundManager();

        if (soundManager == null) return;

        soundManager.SetBgmVolume(value);
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
