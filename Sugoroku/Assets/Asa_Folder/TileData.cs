using UnityEngine;

public class TileData : MonoBehaviour
{
    public int point;
    public bool collected;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        sr.color = color;
    }
}