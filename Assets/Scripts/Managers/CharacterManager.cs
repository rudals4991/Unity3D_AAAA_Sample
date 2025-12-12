using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IManagerBase
{
    [SerializeField] GameObject playerPrefab;
    Player player;
    CameraViewController controller;
    public int Priority => 2;

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }
    public void CreatePlayer()
    {
        if (player != null) Destroy(player.gameObject);
        player = Instantiate(playerPrefab).GetComponent<Player>();
    }
    public void InitializePlayer(GameMode mode)
    {
        if(controller == null) controller = DIContainer.Resolve<CameraViewController>();
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
        if (player == null) return;
        player.Tick(dt);
    }
}
