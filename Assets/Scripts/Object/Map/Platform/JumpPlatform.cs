using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] float jumpForce = 12f;
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out _))
            other.GetComponent<Player>().PlayerJump.JumpByPlatform(jumpForce);
    }
}
