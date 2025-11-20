using UnityEngine;

public class GemFactory : MonoBehaviour
{
    public Gem gemPrefab;
    public GemType[] gemTypes;

    [Tooltip("Parent under which all spawned gems will be placed")]
    public Transform gemsParent;

    public Gem CreateRandomGem()
    {
        if (gemPrefab == null || gemTypes == null || gemTypes.Length == 0)
        {
            Debug.LogError("GemFactory: prefab or gemTypes not set");
            return null;
        }

        GemType type = gemTypes[Random.Range(0, gemTypes.Length)];
        GameObject go = Instantiate(gemPrefab.gameObject, gemsParent ? gemsParent : null);
        Gem gem = go.GetComponent<Gem>();
        gem.Init(type);
        return gem;
    }

    public Gem CreateRandomGemAtPosition(Vector3 startPos)
    {
        Gem g = CreateRandomGem();
        if (g != null)
        {
            g.transform.position = startPos;
        }
        return g;
    }
}