using System.Collections;
using UnityEngine;

//로딩, 즉 일반 Object들과 Manager들의 초기화를 시각적으로 표시합니다.
public class LoadingManager : MonoBehaviour, IManagerBase
{
    public int Priority => 3;

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }
}
