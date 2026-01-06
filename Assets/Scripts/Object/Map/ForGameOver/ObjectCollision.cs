using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    GameoverReason reason = GameoverReason.Collision;
    GameFlowManager gameFlowManager;
    void Awake()
    {
        gameFlowManager = DIContainer.Resolve<GameFlowManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent<Player>(out _)) return;
        if (!gameFlowManager.CanGameplay) return;
        gameFlowManager.GameOver(reason);
        Debug.Log("GameOver");
    }
}
