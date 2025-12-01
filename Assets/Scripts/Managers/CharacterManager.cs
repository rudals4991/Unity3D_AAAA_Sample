using System.Collections;
using UnityEngine;

//CharacterManager는 맵에 존재하는 Character Object의 생성과 Update를 담당합니다.
public class CharacterManager : MonoBehaviour, IManagerBase
{
    public int Priority => 2;

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }

    public void Tick(float dt) 
    {
        //CharacterManager 하위의 Character Object들의 Update를 이곳에서 처리합니다.
        //해당 클래스의 Tick 메서드는 최상위 Manager인 GameManager에서 호출합니다.
        //예시: testCharacter.Tick(dt);
    }
}
