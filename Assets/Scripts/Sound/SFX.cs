using UnityEngine;

public class SFX : SoundClipBase
{
    public override void Initialize()
    {
        base.Initialize();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0;
    }
    public void Play(AudioClip clip, float volume)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip, volume);
    }
}
