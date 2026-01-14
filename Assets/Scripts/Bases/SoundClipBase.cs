using UnityEngine;

public abstract class SoundClipBase : MonoBehaviour
{
    protected AudioSource audioSource;
    public virtual void Initialize()
    {
        audioSource ??= GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }
    public virtual void SetVolume(float volume)
    { 
        audioSource.volume = volume;
    }
}
