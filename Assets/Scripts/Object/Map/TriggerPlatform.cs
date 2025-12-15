using UnityEngine;

public class TriggerPlatform : MonoBehaviour
{
    bool isFlag = false;
    private void OnEnable()
    {
        isFlag = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (isFlag) return;
        if (!other.TryGetComponent<Player>(out _)) return;
        isFlag = true;
        DIContainer.Resolve<GameModeManager>().AdvanceMode();
    }
}
