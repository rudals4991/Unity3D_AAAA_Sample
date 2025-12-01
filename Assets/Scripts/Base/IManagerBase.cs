using System.Collections;
using UnityEngine;

public interface IManagerBase
{
    int Priority { get; } // 초기화(Initialize) 우선순위 보장용
    IEnumerator Initialize(); // 초기화 메서드 
    void Exit(); // 정리용 메서드
}
