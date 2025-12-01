using System.Collections;
using UnityEngine;

//UI들의 활성화 여부를 결정합니다
public class UIManager : MonoBehaviour, IManagerBase
{
    public int Priority => 6;

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }
}
