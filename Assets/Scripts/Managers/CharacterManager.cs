using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IManagerBase
{
    [SerializeField] GameObject playerPrefab;
    Player player;
    CameraViewController controller;
    bool tickEnabled = false;
    public int Priority => 3;

    public void Exit()
    {
        GameFlowManager.OnGameStarted -= HandleGameStarted;
        GameFlowManager.OnGamePlayBegin -= HandleGamePlayBegin;
        GameFlowManager.OnGameOvered -= HandleGameOver;
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;

        GameFlowManager.OnGameStarted -= HandleGameStarted;
        GameFlowManager.OnGameStarted += HandleGameStarted;

        GameFlowManager.OnGamePlayBegin -= HandleGamePlayBegin;
        GameFlowManager.OnGamePlayBegin += HandleGamePlayBegin;

        GameFlowManager.OnGameOvered -= HandleGameOver;
        GameFlowManager.OnGameOvered += HandleGameOver;
    }
    void HandleGameStarted()
    {
        tickEnabled = false; 
    }
    void HandleGamePlayBegin()
    {
        tickEnabled = true;
    }
    void HandleGameOver(GameoverReason _)
    {
        Debug.Log("Stop");
        tickEnabled = false;
    }
    public void CreatePlayer()
    {
        if (player != null) Destroy(player.gameObject);
        player = Instantiate(playerPrefab, new Vector3(0, 2, -24.7f), Quaternion.identity).GetComponent<Player>();
    }
    public void InitializePlayer(GameMode mode)
    {
        if (controller == null) controller = DIContainer.Resolve<CameraViewController>();
        controller.SetTarget(player.transform);
        player.Initialize(mode);
        SetMode(mode);
    }
    public void SetMode(GameMode mode)
    {
        player.ApplyGameMode(mode);
    }
    public void Tick(float dt)
    {
        if (!tickEnabled) return;
        if (player == null) return;
        player.Tick(dt);
    }
    public void FixedTick(float fdt)
    {
        if (!tickEnabled) return;
        if (player == null) return;
        player.FixedTick(fdt);
    }
}
