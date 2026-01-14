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
    [SerializeField] float defaultBgmVolume = 1f;
    public float BgmVolume { get; private set; }
    BGM bgmPlayer;
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
        BgmVolume = PlayerPrefs.GetFloat("BGMVolume", defaultBgmVolume);
        BgmVolume = Mathf.Clamp01(BgmVolume);
        EnsureBgmPlayer();
        bgmPlayer.SetVolume(BgmVolume);

        MySceneManager.OnSceneLoaded -= SceneLoaded;
        MySceneManager.OnSceneLoaded += SceneLoaded;
        GameFlowManager.OnGameStarted -= GameStarted;
        GameFlowManager.OnGameStarted += GameStarted;
        GameFlowManager.OnGameOvered -= GameOvered;
        GameFlowManager.OnGameOvered += GameOvered;

        RequestBgm(BgmState.Title);
    }
    void EnsureBgmPlayer()
    {
        bgmPlayer = GetComponentInChildren<BGM>(true);
        if (bgmPlayer != null)
        {
            bgmPlayer.Initialize();
            return;
        }
        var go = new GameObject("[BGM]");
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = true;
        src.spatialBlend = 0f;
        bgmPlayer = go.AddComponent<BGM>();
        bgmPlayer.Initialize();
    }
    public void SetBgmVolume(float volume)
    {
        BgmVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("BGMVolume", BgmVolume);

        if (bgmPlayer != null)
            bgmPlayer.SetVolume(BgmVolume);
    }
    void SceneLoaded(SceneList scene)
    {
        if (scene == SceneList.Title) RequestBgm(BgmState.Title);
    }
    void GameStarted()
    {
        RequestBgm(BgmState.GamePlay);
    }
    void GameOvered(GameoverReason _)
    {
        RequestBgm(BgmState.GameOver);
    }
    void RequestBgm(BgmState next)
    {
        if (currentState.HasValue && currentState.Value == next) return;
        currentState = next;
        switch (next)
        {
            case BgmState.Title: PlayIntroLoop(titleIntro, titleLoop); break;
            case BgmState.GamePlay: PlayIntroLoop(gameIntro, gameLoop); break;
            case BgmState.GameOver:
                {
                    if (gameOverLoop != null) bgmPlayer.PlayLoop(gameOverLoop, BgmVolume);
                    else bgmPlayer.Stop();
                } break;
        }
    }
    void PlayIntroLoop(AudioClip intro, AudioClip loop)
    {
        if (intro == null && loop == null)
        {
            bgmPlayer.Stop();
            return;
        }
        if (intro == null || intro.length <= 0f)
        {
            bgmPlayer.PlayLoop(loop, BgmVolume);
            return;
        }
        if (loop == null)
        {
            bgmPlayer.PlayIntroToLoop(intro, null, BgmVolume);
            return;
        }
        bgmPlayer.PlayIntroToLoop(intro, loop, BgmVolume);
    }
}
