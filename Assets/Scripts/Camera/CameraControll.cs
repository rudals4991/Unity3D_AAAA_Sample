using UnityEngine;

public class CameraControll : MonoBehaviour
{
    //[SerializeField] Transform target;
    float targetAspet = 16f / 9f;
    private Camera cam;
    private void Start()
    {
        cam = GetComponent<Camera>();
        UpdateView();
    }
    //private void FixedUpdate()
    //{
    //    transform.position = target.position;
    //}
    private void UpdateView()
    { 
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspet;
        if (scaleHeight < 1f)
        { 
            Rect rect = cam.rect;
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1f - scaleHeight) / 2f;
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}
