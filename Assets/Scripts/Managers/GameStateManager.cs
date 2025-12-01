using System.Collections;
using UnityEngine;

//게임의 전체 흐름을 FSM 형태로 관리합니다.
//상태 전환 시 이벤트로 전달해서 필요한 동작을 키거나 끕니다.
public class GameStateManager : MonoBehaviour, IManagerBase
{
    public int Priority => 12;

    public void Exit()
    {
        
    }

    public IEnumerator Initialize()
    {
        yield return null;
    }
}
