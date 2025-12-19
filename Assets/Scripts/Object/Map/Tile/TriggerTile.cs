using UnityEngine;

public class TriggerTile : MonoBehaviour
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
