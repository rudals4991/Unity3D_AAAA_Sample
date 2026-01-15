using System.Collections;
using UnityEngine;

public enum BgmState
{
    Title,
    GamePlay,
    GameOver
}
public class SoundManager : MonoBehaviour, IManagerBase
{
    public int Priority => 1;

    [Header("BGM Clips - Title")]
    [SerializeField] AudioClip titleIntro;
    [SerializeField] AudioClip titleLoop;

    [Header("BGM Clips - GamePlay")]
    [SerializeField] AudioClip gameIntro;
    [SerializeField] AudioClip gameLoop;

    [Header("BGM Clips - GameOver")]
    [SerializeField] AudioClip gameOverLoop;

    [Header("Default Volume")]
    [Range(0f, 1f)]
    [SerializeField] float defaultBGMVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] float defaultSFXVolume = 1f;
    public float BGMVolume { get; private set; }
    public float SFXVolume { get; private set; }
    BGM bgm;
    SFX sfx;
    BgmState? currentState;

    public void Exit()
    {
        MySceneManager.OnSceneLoaded -= SceneLoaded;
        GameFlowManager.OnGameStarted -= GameStarted;
        GameFlowManager.OnGameOvered -= GameOvered;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);
        BGMVolume = Mathf.Clamp01(BGMVolume);
        SFXVolume = Mathf.Clamp01(SFXVolume);
        EnsureBGMPlayer();
        EnsureSFXPlayer();

        bgm.SetVolume(BGMVolume);

        MySceneManager.OnSceneLoaded -= SceneLoaded;
        MySceneManager.OnSceneLoaded += SceneLoaded;
        GameFlowManager.OnGameStarted -= GameStarted;
        GameFlowManager.OnGameStarted += GameStarted;
        GameFlowManager.OnGameOvered -= GameOvered;
        GameFlowManager.OnGameOvered += GameOvered;

        RequestBGM(BgmState.Title);
    }
    void EnsureBGMPlayer()
    {
        bgm = GetComponentInChildren<BGM>(true);
        if (bgm != null)
        {
            bgm.Initialize();
            return;
        }
        var go = new GameObject("[BGM]");
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = true;
        src.spatialBlend = 0f;
        bgm = go.AddComponent<BGM>();
        bgm.Initialize();
    }
    void EnsureSFXPlayer()
    {
        sfx = GetComponentInChildren<SFX>(true);
        if (sfx != null)
        {
            sfx.Initialize();
            return;
        }
        var go = new GameObject("[SFX]");
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
        src.spatialBlend = 0f;
        sfx = go.AddComponent<SFX>();
        sfx.Initialize();
    }
    public void SetBGMVolume(float volume)
    {
        BGMVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
        if (bgm != null) bgm.SetVolume(BGMVolume);
    }
    public void SetSFXVolume(float volume)
    { 
        SFXVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }
    void SceneLoaded(SceneList scene)
    {
        if (scene == SceneList.Title) RequestBGM(BgmState.Title);
    }
    void GameStarted()
    {
        RequestBGM(BgmState.GamePlay);
    }
    void GameOvered(GameoverReason _)
    {
        RequestBGM(BgmState.GameOver);
    }
    void RequestBGM(BgmState next)
    {
        if (currentState.HasValue && currentState.Value == next) return;
        currentState = next;
        switch (next)
        {
            case BgmState.Title: PlayIntroLoop(titleIntro, titleLoop); break;
            case BgmState.GamePlay: PlayIntroLoop(gameIntro, gameLoop); break;
            case BgmState.GameOver:
                {
                    if (gameOverLoop != null) bgm.PlayLoop(gameOverLoop, BGMVolume);
                    else bgm.Stop();
                } break;
        }
    }
    void PlayIntroLoop(AudioClip intro, AudioClip loop)
    {
        if (intro == null && loop == null)
        {
            bgm.Stop();
            return;
        }
        if (intro == null || intro.length <= 0f)
        {
            bgm.PlayLoop(loop, BGMVolume);
            return;
        }
        if (loop == null)
        {
            bgm.PlayIntroToLoop(intro, null, BGMVolume);
            return;
        }
        bgm.PlayIntroToLoop(intro, loop, BGMVolume);
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfx.Play(clip, SFXVolume);
    }
}
