using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlatformType MyType { get; private set; }
    public void SetType(PlatformType type)
    { 
        MyType = type;
    }
}
