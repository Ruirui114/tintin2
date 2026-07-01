using UnityEngine;

public class TileManager : MonoBehaviour
{
    public TileData[] tiles;

    public void ChangeTileColor(int tileIndex, Color color)
    {
        if (tileIndex >= 0 && tileIndex < tiles.Length)
        {
            tiles[tileIndex].ChangeColor(color);
        }
    }
}
