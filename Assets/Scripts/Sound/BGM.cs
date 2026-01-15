using System.Collections;
using UnityEngine;

public class BGM : SoundClipBase
{
    Coroutine introRoutine;
    int playToken;
    public override void Initialize()
    {
        base.Initialize();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f;
    }
    public void PlayIntroToLoop(AudioClip introClip, AudioClip loopClip, float volume)
    {
        Initialize();
        playToken++;
        CancelIntroRoutine();
        audioSource.Stop();
        SetVolume(volume);
        if (introClip == null || introClip.length <= 0f)
        {
            PlayLoop(loopClip, volume);
            return;
        }
        audioSource.clip = introClip;
        audioSource.loop = false;
        audioSource.Play();
        if (loopClip != null)
            introRoutine = StartCoroutine(IntroToLoop(introClip.length, loopClip, playToken));
    }
    public void PlayLoop(AudioClip clip, float volume)
    {
        Initialize();
        playToken++;
        CancelIntroRoutine();
        if (clip == null) return;
        audioSource.Stop();
        SetVolume(volume);
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void Stop()
    {
        Initialize();
        playToken++;
        CancelIntroRoutine();
        if (audioSource.isPlaying) audioSource.Stop();
    }
    void CancelIntroRoutine()
    {
        if (introRoutine != null)
        {
            StopCoroutine(introRoutine);
            introRoutine = null;
        }
    }
    IEnumerator IntroToLoop(float introLength, AudioClip loopClip, int token)
    {
        yield return new WaitForSecondsRealtime(introLength);
        if (token != playToken) yield break;
        if (loopClip == null) yield break;
        audioSource.Stop();
        audioSource.clip = loopClip;
        audioSource.loop = true;
        audioSource.Play();
        introRoutine = null;
    }
}
