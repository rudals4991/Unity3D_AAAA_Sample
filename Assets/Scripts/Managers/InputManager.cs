using System.Collections;
using UnityEngine;

//InputSystem을 기반으로 입력을 통합하여 처리합니다.
//현재 ViewMode에 맞는 입력 방식으로 변환하고 PlayerMovement(예시)에 전달합니다.
public class InputManager : MonoBehaviour, IManagerBase
{
    public int Priority => 11;

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        yield return null;
    }
}
