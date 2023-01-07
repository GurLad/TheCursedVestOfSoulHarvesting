using Godot;
using System;

public class FloorMarker : Node
{
    public enum MarkType { None, PreviewMove = 1, PreviewAttack = 2, Move = 4, Attack = 8 }
    private Floor[,] floors;
    private Vector2Int size;

    public void NewLevel(Vector2Int size)
    {
        this.size = size;
        floors = new Floor[size.x, size.y];
    }

    public void AddFloor(int x, int y, Floor floor)
    {
        floors[x, y] = floor;
        floors[x, y].Init(new Vector2Int(x, y), this);
    }

    public void ClearMarkers()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (floors[x, y] != null)
                {
                    floors[x, y].RemoveMarker(MarkType.PreviewMove);
                    floors[x, y].RemoveMarker(MarkType.PreviewAttack);
                    floors[x, y].RemoveMarker(MarkType.Move);
                    floors[x, y].RemoveMarker(MarkType.Attack);
                }
            }
        }
    }

    public void AddMarker(Vector2Int pos, MarkType markType, AUnitAction action = null)
    {
        AddMarker(pos.x, pos.y, markType, action);
    }

    public void AddMarker(int x, int y, MarkType markType, AUnitAction action = null)
    {
        if (floors[x, y] == null)
        {
            throw new Exception("Marking something that isn't a floor!");
        }
        floors[x, y].AddMarker(markType, action);
    }

    public void RemoveMarker(Vector2Int pos, MarkType markType)
    {
        RemoveMarker(pos.x, pos.y, markType);
    }

    public void RemoveMarker(int x, int y, MarkType markType)
    {
        if (floors[x, y] == null)
        {
            throw new Exception("Marking something that isn't a floor!");
        }
        floors[x, y].RemoveMarker(markType);
    }
}
