using UnityEngine;

public class Gem : MonoBehaviour
{
    public GemType GemType 
    { 
        get; 
        private set; 
    }
    public Cell Cell 
    { 
        get; 
        private set; 
    }

    public void Init(GemType type)
    {
        GemType = type;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if(sr != null && type != null)
        {
            sr.sprite = type.Icon;
        }
    }

    public void SetCell(Cell cell)
    {
        Cell = cell;
        if (cell != null)
        {
            transform.position = cell.WorldPosition;
        }
    }
}