using UnityEngine;

public class TriggerTile : MonoBehaviour
{
    [SerializeField] GameMode nextMode;
    bool isFlag = false;
    void OnTriggerEnter(Collider other)
    {
        if (isFlag) return;
        if (!other.TryGetComponent<Player>(out _)) return;
        GameModeManager gm = DIContainer.Resolve<GameModeManager>();
        gm.SetMode(nextMode);
        isFlag = true;
    }
    public void ResetFlag()
    { 
        isFlag = false;
    }
}
