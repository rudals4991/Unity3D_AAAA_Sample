using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("Gyro")]
    [SerializeField] private GyroTest gyro;

    [Header("Move Mode 1: Just Move, 2: Line Move")]
    [SerializeField] private int index = 0;

    [Header("Values")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float laneDistance = 2f;
    [SerializeField] private float laneLerpSpeed = 5f;
    private int currentLane = 0;

    private void Update()
    {
        if (index == 1) MoveLeftToRight();
        else if (index == 2) MoveLine();
        else return;
    }
    private void MoveLeftToRight()
    {
        float tilt = gyro.GetHorizontalTilt();
        transform.position += Vector3.right * tilt * moveSpeed * Time.deltaTime;
    }
    private void MoveLine()
    {
        float tilt = gyro.GetHorizontalTilt();

        if (tilt < -0.3f) currentLane = -1;
        else if (tilt > 0.3f) currentLane = 1;
        else currentLane = 0;

        Vector3 targetPos = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * laneLerpSpeed);
    }
}
