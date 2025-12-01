using System.Collections;
using UnityEngine;

// 속도 제어를 담당합니다. 카메라, 캐릭터 등의 속도를 관리합니다.
public class SpeedManager : MonoBehaviour, IManagerBase
{
    public int Priority => 9;

    public void Exit()
    {
        
    }

    public IEnumerator Initialize()
    {
        yield return null;
    }
}
