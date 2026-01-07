using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] float jumpForce = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
            player.PlayerJump.RequestPlatformJump(jumpForce);
    }
}
