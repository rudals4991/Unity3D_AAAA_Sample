using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//IManagerBase interface를 가진 모든 Manager 클래스들을 한꺼번에 초기화 / 종료시킵니다.
public static class ManagerInitializer
{
    private static List<IManagerBase> managers;
    public static IEnumerator InitializeAll()
    {
        // 씬 안의 모든 IManagerBase 오브젝트를 찾아 Priority를 기준으로 오름차순으로 정렬 후
        // 순서대로 Initialize() 를 실행합니다.
        managers = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IManagerBase>().OrderBy(m => m.Priority).ToList();
        foreach (var manager in managers)
        {
            yield return manager.Initialize();
        }
    }
    public static void ExitAll()
    {
        // 모든 IManagerBase 오브젝트들의 Exit()을 실행합니다.
        // 종료 시에는 초기화와 반대로 내림차순으로 정렬 후 Exit()을 실행합니다.
        if (managers is null) return; //방어용
        managers.Reverse(); // 오름차순으로 정렬된 Manager들을 반대로 내림차순으로 재정렬
        foreach (var manager in managers)
        { 
            manager.Exit();
        }
    }
}
