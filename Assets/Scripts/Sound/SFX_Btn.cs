using UnityEngine;
using UnityEngine.UI;

public class SFX_Btn : MonoBehaviour
{
    [SerializeField] AudioClip sfx;
    SoundManager soundManager;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySFX);
    }
    void OnEnable()
    {
        soundManager = TryResolveSoundManager();
    }

    void OnDestroy()
    {
        if (button != null) button.onClick.RemoveListener(PlaySFX);
    }
    void PlaySFX()
    {
        if (sfx == null) return;
        if (soundManager == null) soundManager = TryResolveSoundManager();
        soundManager?.PlaySFX(sfx);
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
