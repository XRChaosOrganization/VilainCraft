using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEngine
{
    public class LevelData : MonoBehaviour
    {
        public static LevelData current;
        [HideInInspector] public List<Tile> serializableGrid = new List<Tile>();
        public Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();



        private void Awake()
        {
            current = this;
            LoadGrid();
        }

        public void SaveGrid()
        {
            serializableGrid.Clear();
            foreach (KeyValuePair<Vector2, Tile> pair in grid)
            {
                Tile t = new Tile(pair.Key, pair.Value.type, pair.Value.height, pair.Value.isBlocked);
                t.TileGO = pair.Value.TileGO;
                t.node = pair.Value.node;

                serializableGrid.Add(t);
            }
        }

        public void LoadGrid()
        {
            foreach (Tile tile in serializableGrid)
                grid[tile.gridPos] = tile;
        }
    }

}
