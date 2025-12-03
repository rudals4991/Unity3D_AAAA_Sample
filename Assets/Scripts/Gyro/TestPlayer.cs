using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("Gyro")]
    [SerializeField] private GyroTest gyro;

    //[Header("Move Mode 1: Just Move, 2: Line Move")]
    //[SerializeField] private int index = 0;


    private void Update()
    {
        MoveLeftToRight();
    }
    private void MoveLeftToRight()
    {
        float speed = gyro.GetHorizontalSpeed();
        transform.position += Vector3.right * speed * Time.deltaTime;
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
