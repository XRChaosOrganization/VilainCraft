using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentTiles : IEnumerable
{
    public Tile up;
    public Tile upRight;
    public Tile right;
    public Tile downRight;
    public Tile down;
    public Tile downLeft;
    public Tile left;
    public Tile upLeft;

    public IEnumerator GetEnumerator()
    {
        return new AdjTilesEnumerator(this);
    }

    public class AdjTilesEnumerator : IEnumerator
    {
        int _index = -1;
        AdjacentTiles adj;
        public AdjTilesEnumerator(AdjacentTiles _adj)
        {
            adj = _adj;
        }
        object IEnumerator.Current
        {
            get
            {
                switch (_index)
                {
                    case 0:
                        return adj.up;
                    case 1:
                        return adj.upRight;
                    case 2:
                        return adj.right;
                    case 3:
                        return adj.downRight;
                    case 4:
                        return adj.down;
                    case 5:
                        return adj.downLeft;
                    case 6:
                        return adj.left;
                    case 7:
                        return adj.upLeft;

                    default:
                        throw new System.NotImplementedException();

                }
            }
        }

        bool IEnumerator.MoveNext()
        {
            _index++;
            return _index < 8;
        }

        void IEnumerator.Reset()
        {
            _index = -1;
        }
    }


}


public class GridUtilities
{
    public static AdjacentTiles GetAdjacentTiles(Dictionary<Vector2, Tile> _grid, Vector2 _refPos)
    {
        
        AdjacentTiles output = new AdjacentTiles();
        output.up = _grid.ContainsKey(_refPos + Vector2.up) ? _grid[_refPos + Vector2.up] : null;
        output.upRight = _grid.ContainsKey(_refPos + Vector2.up + Vector2.right) ? _grid[_refPos + Vector2.up + Vector2.right] : null;
        output.upLeft = _grid.ContainsKey(_refPos + Vector2.up + Vector2.left) ? _grid[_refPos + Vector2.up + Vector2.left] : null;
        output.down = _grid.ContainsKey(_refPos + Vector2.down) ? _grid[_refPos + Vector2.down] : null;
        output.downRight = _grid.ContainsKey(_refPos + Vector2.down + Vector2.right) ? _grid[_refPos + Vector2.down + Vector2.right] : null;
        output.downLeft = _grid.ContainsKey(_refPos + Vector2.down + Vector2.left) ? _grid[_refPos + Vector2.down + Vector2.left] : null;
        output.right = _grid.ContainsKey(_refPos + Vector2.right) ? _grid[_refPos + Vector2.right] : null;
        output.left = _grid.ContainsKey(_refPos + Vector2.left) ? _grid[_refPos + Vector2.left] : null;

        return output;
    }

    public static int GetDirectionIndex(Vector2 _pointer)
    {
        switch (_pointer)
        {
            case Vector2 v when v.Equals(Vector2.up):
                return 0;
            case Vector2 v when v.Equals(Vector2.up + Vector2.right):
                return 1;
            case Vector2 v when v.Equals(Vector2.right):
                return 2;
            case Vector2 v when v.Equals(Vector2.down + Vector2.right):
                return 3;
            case Vector2 v when v.Equals(Vector2.down):
                return 4;
            case Vector2 v when v.Equals(Vector2.down + Vector2.left):
                return 5;
            case Vector2 v when v.Equals(Vector2.left):
                return 6;
            case Vector2 v when v.Equals(Vector2.up + Vector2.left):
                return 7;
            default:
                throw new System.NotImplementedException();
        }
    }

    public static Tile GetAdjacentFromIndex( AdjacentTiles _adj, int _i)
    {
        switch (_i)
        {
            case 0 :
                return _adj.up;
            case 1 :
                return _adj.upRight;
            case 2 :
                return _adj.right;
            case 3 :
                return _adj.downRight;
            case 4 :
                return _adj.down;
            case 5 :
                return _adj.downLeft;
            case 6 :
                return _adj.left;
            case 7 :
                return _adj.upLeft;
            default:
                throw new System.NotImplementedException();
        }
    }

}

