using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    TilePool pool;
    void Awake()
    {
        DIContainer.Register(this);
    }
    public void CreateTileByMode(GameMode mode, TilePool pool)
    {
        //TODO: 소환 위치 정하기 + 소환 로직 구현
        this.pool = pool;
        switch (mode)
        {
            case GameMode.SideView_ToRight: 
                CreateForRight(TileType.Linear, Vector3.zero, Quaternion.identity); break;
            case GameMode.BackView_ToForward: 
                CreateForForward(TileType.Linear, Vector3.zero, Quaternion.identity); break;
        }
    }
    void CreateForForward(TileType wantType, Vector3 wantPos, Quaternion wantRot)
    {
        GameObject targetPlatform = pool.Get(wantType, wantPos, wantRot);
    }
    void CreateForRight(TileType wantTpye, Vector3 wantPos, Quaternion wantRot)
    {
        GameObject targetPlatform = pool.Get(wantTpye, wantPos, wantRot);
    }
}
