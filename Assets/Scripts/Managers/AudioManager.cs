using System.Collections;
using UnityEngine;

//AudioManager는 BGM, SFX와의 연결과 볼륨 조절을 담당합니다
public class AudioManager : MonoBehaviour, IManagerBase
{
    public int Priority => 1; 

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this); //DIContainer에 스스로를 등록합니다.
        yield return null; // 한 프레임을 대기합니다.
                           // -> Unity의 생명 주기 메서드인 Awake, Start의 사용을 보장합니다.
    }
}
