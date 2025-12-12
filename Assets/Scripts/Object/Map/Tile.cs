using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType MyType { get; private set; }
    public void SetType(TileType type)
    { 
        MyType = type;
    }
}
