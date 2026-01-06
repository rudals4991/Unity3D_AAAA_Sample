using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("AutoMove")]
    public float moveSpeed;

    [Header("GyroMove")]
    public float gyroSpeedLeftRight;
    public float gyroSpeedForward;
    public float deadZone;

    public float sensitivity;

    [Header("Jump")]
    public float jumpForce;
    public float fallMultiplier;
    public int maxJumpCount_BackView;
    public int maxJumpCount_SideView;
}
