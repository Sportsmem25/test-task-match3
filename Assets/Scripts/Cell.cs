using UnityEngine;

public class Cell
{
    public Vector2Int GridPos;
    public Vector3 WorldPosition;
    public Gem CurrentGem;
    public bool isEmpty => CurrentGem == null;

    public Cell(Vector2Int pos, Vector3 world)
    {
        GridPos = pos;
        WorldPosition = world;
        CurrentGem = null;
    }
}