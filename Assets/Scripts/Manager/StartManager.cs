using System;
using System.Collections;
using UnityEngine;

//게임의 시작(스테이지의 시작)을 관리합니다.
public class StartManager : MonoBehaviour, IManagerBase
{
    //이벤트를 발행함으로써 스테이지가 시작됐음을 각 스크립트들에게 전달합니다. (임시)
    public static event Action<int> OnStageStart;
    public int Priority => 5;

    public void Exit()
    {
    }

    public IEnumerator Initialize()
    {
        DIContainer.Register(this);
        yield return null;
    }
}
