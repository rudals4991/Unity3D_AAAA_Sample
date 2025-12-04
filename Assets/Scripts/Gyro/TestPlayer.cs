using UnityEngine;

public class TestPlayer : MonoBehaviour
{

    [Header("Run Mode")]
    [SerializeField] GameMode gameMode;
    [SerializeField] CameraControll cameraControll;

    [Header("Gyro")]
    [SerializeField] private GyroTest gyro;

    [Header("Jump Setting")]
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask ground;

    [Header("Auto Run Setting")]
    [SerializeField] private float moveSpeed;

    private int maxJumpCount = 2;
    private int jumpCount = 0;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isGround;

    float gravityStrength;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        cameraControll.SetCameraMode(gameMode);

        switch (gameMode)
        {
            case GameMode.SideView_ToRight:
            case GameMode.BackView_ToForward:
            case GameMode.SideView_ToTop:
                gravityStrength = 1f; break;
            case GameMode.SideView_ToDown:
                gravityStrength = 0.05f; break;
        }
    }
    private void Update()
    {
        AutoRun(gameMode);
        GyroMove(gameMode);
        CheckIsGround();
        if (IsTouch()) TryJump(gameMode);
    }
    private void FixedUpdate()
    {
        ApplyGravity();
    }
    private void AutoRun(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.SideView_ToRight:
            case GameMode.BackView_ToForward:
                transform.position += Vector3.forward * moveSpeed * Time.deltaTime; break;
        }
    }
    private void GyroMove(GameMode mode)
    {
        float speed = gyro.GetHorizontalSpeed();
        switch (mode)
        {
            case GameMode.BackView_ToForward:
            case GameMode.SideView_ToTop:
            case GameMode.SideView_ToDown:
                //float speed = gyro.GetHorizontalSpeed();
                transform.position += Vector3.right * speed * Time.deltaTime; break;

            case GameMode.SideView_ToRight: break;
        }
    }
    private void TryJump(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.SideView_ToRight:
            case GameMode.BackView_ToForward:
            case GameMode.SideView_ToTop: Jump(); break;
            case GameMode.SideView_ToDown: break;
        }
    }
    public void Jump()
    {
        if (jumpCount >= maxJumpCount) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;
    }
    private void CheckIsGround()
    {
        float dist = capsuleCollider.bounds.extents.y + 0.1f;
        if (Physics.Raycast(transform.position, Vector3.down, dist, ground))
        {
            isGround = true;
            jumpCount = 0;
        }
        else
        {
            isGround = false;
        }
    }
    private bool IsTouch()
    {
        if (Input.GetMouseButtonDown(0)) return true;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) return true;

        return false;
    }
    private void ApplyGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.up * Physics.gravity.magnitude * gravityStrength, ForceMode.Acceleration);
        }
    }
    #region 라인옮기기 기능
    //private void MoveLine()
    //{
    //    float tilt = gyro.GetHorizontalTilt();

    //    if (tilt < -0.3f) currentLane = -1;
    //    else if (tilt > 0.3f) currentLane = 1;
    //    else currentLane = 0;

    //    Vector3 targetPos = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);
    //    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * laneLerpSpeed);
    //}
    #endregion
}
