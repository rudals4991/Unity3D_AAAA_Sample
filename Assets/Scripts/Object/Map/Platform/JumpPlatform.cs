using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] float jumpForce = 12f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out _))
            collision.gameObject.GetComponent<Player>().PlayerJump.JumpByPlatform(jumpForce);
    }
}
