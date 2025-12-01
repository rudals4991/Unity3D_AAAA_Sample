using System.Collections;
using UnityEngine;

// 현재 시점에 맞는 Stage 생성 및 제거 (세그먼트 기반의 구간 생성 및 제거 + 트리거 관리)
public class StageManager : MonoBehaviour, IManagerBase
{
    public int Priority => 8;

    public void Exit()
    {
        
    }

    public IEnumerator Initialize()
    {
        yield return null;
    }
}
