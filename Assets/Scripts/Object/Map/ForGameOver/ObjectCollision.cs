using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    GameoverReason reason = GameoverReason.Collision;
    GameFlowManager gameFlowManager;
    void Awake()
    {
        gameFlowManager = DIContainer.Resolve<GameFlowManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out _)) return;
        if (!gameFlowManager.CanGameplay) return;
        gameFlowManager.GameOver(reason);
    }
}
