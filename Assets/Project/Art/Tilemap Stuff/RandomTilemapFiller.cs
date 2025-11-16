using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RandomTilemapFiller : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [Header("Tiles")]
    [SerializeField] private TileBase blankTile;
    [SerializeField] private List<TileBase> specialTiles;

    [Header("Grid Size")]
    [SerializeField] private int width = 50;
    [SerializeField] private int height = 50;

    [Header("Distribution")]
    [Range(0f, 1f)]
    [SerializeField] private float specialChance = 0.2f;   // 20%

    [ContextMenu("Fill Tilemap Randomly")]
    private void FillTilemap()
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool placeSpecial = Random.value < specialChance;

                if (placeSpecial && specialTiles.Count > 0)
                {
                    // pick a random tile from the list
                    TileBase chosen = specialTiles[
                        Random.Range(0, specialTiles.Count)
                    ];

                    tilemap.SetTile(new Vector3Int(x, y, 0), chosen);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), blankTile);
                }
            }
        }

        Debug.Log("Tilemap filled with multiple special tile types.");
    }
}