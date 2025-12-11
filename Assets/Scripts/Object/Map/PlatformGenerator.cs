using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    PlatformPool pool;
    void Awake()
    {
        DIContainer.Register(this);
    }
    public void CreatePlatformByMode(GameMode mode, PlatformPool pool)
    {
        //TODO: 소환 위치 정하기 + 소환 로직 구현
        this.pool = pool;
        switch (mode)
        {
            case GameMode.SideView_ToTop: 
                CreateToTop(PlatformType.ForJump, Vector3.zero, Quaternion.identity); break;
            case GameMode.SideView_ToDown: 
                CreateToDown(PlatformType.None, Vector3.zero,Quaternion.identity); break;
        }
    }
    void CreateToTop(PlatformType wantType, Vector3 wantPos, Quaternion wantRot)
    {
        GameObject targetPlatform = pool.Get(wantType, wantPos, wantRot);
    }
    void CreateToDown(PlatformType wantType, Vector3 wantPos, Quaternion wantRot)
    {
        GameObject targetPlatform = pool.Get(wantType, wantPos, wantRot);
    }
}
