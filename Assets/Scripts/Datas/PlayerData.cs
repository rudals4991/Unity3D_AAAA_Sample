using UnityEngine;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("AutoMove")]
    public float moveSpeed;                 //초기값: 5f;

    [Header("GyroMove")]
    public float gyroSpeedLeftRight;        //초기값: 5f;
    public float gyroSpeedForward;          //초기값: 5f;
    public float deadZone;                  //초기값: 0.1f;
    public float sensitivity;               //초기값: 1.2f

    [Header("Jump")]
    public float jumpForce;                 //초기값: 4f;
    public float fallMultiplier;            //초기값: 2f;
    public int maxJumpCount_BackView;       //초기값: 1;
    public int maxJumpCount_SideView;       //초기값: 2;
}
