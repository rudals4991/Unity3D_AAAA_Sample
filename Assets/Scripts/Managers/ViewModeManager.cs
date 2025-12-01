using System.Collections;
using UnityEngine;

//게임 진행중 시점 전환을 담당합니다. 시점에 따라 카메라 위치, 이동 축, 입력 방식을 변경합니다.
public class ViewModeManager : MonoBehaviour, IManagerBase
{
    public int Priority => 7;

    public void Exit()
    {
        
    }

    public IEnumerator Initialize()
    {
        yield return null;
    }
}
