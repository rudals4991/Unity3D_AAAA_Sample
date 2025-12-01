using System.Collections;
using UnityEngine;

//하위 Pool들을 관리합니다. pool에 등록된 오브젝트는 모두 PoolManager를 거쳐서 호출합니다.
public class PoolManager : MonoBehaviour, IManagerBase
{
    public int Priority => 4;

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }
}
