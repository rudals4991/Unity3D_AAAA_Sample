using System.Collections;
using UnityEngine;

//Camera의 제어를 담당합니다. 카메라 위치, 각도 등을 제어합니다.
public class CameraManager : MonoBehaviour, IManagerBase
{
    public int Priority => 10;

    public void Exit()
    {
        
    }

    public IEnumerator Initialize()
    {
        yield return null;
    }
}
