using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IManagerBase
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] CameraViewController controller;
    Player player;
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
        if (player == null) CreatePlayer();
        player.Initialize(mode);
        controller.SetTarget(player.transform);
        controller.SetCameraMode(mode);
    }
    public void Tick(float dt)
    {
        player.Tick(dt);
    }
}
