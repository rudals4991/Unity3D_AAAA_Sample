using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log($"touchCount = {Input.touchCount}");

            for (int i = 0; i < Input.touchCount; i++)
            {
                var t = Input.GetTouch(i);
                Debug.Log($"Finger {i} at {t.position}");
            }
        }
    }
}
