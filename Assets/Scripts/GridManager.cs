using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance 
    { 
        get; 
        private set; 
    }
    public bool IsBusy 
    { 
        get; 
        private set; 
    }

   public Cell[,]                        cells;
   public MatchFinder                    matchFinder;
   public float                          swapAnimSpeed;
   public float                          fallAnimSpeed;
   [SerializeField] private GemFactory   gemFactory;
   [SerializeField] private Camera       cam;
   [SerializeField] private ScoreManager scoreManager;
   [SerializeField] private int          height;
   [SerializeField] private int          width;
   [SerializeField] private int          pointsPerGem;
   [SerializeField] private float        cellSize;
    

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        IsBusy = false;
    }

    private void Start()
    {
        AdjustCamera();
    }

    public void StartGame()
    {
        InitBoard();
    }

    private void AdjustCamera()
    {
        float boardWidth = (width - 1) * cellSize;
        float boardHeight = (height - 1) * cellSize;
        cam.orthographicSize = (boardHeight * 1f);
        cam.transform.position = new Vector3(boardWidth / 2f, boardHeight / 2f, -10f);
    }

    private void InitBoard()
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, y * cellSize, 0);
                cells[x, y] = new Cell(new Vector2Int(x, y), pos);
                SpawnGem(x, y);
            }
        }

        RemoveAllInitialMatches(10);
    }

    private void SpawnGem(int x, int y)
    {
        Gem gem = gemFactory.CreateRandomGem();
        gem.SetCell(cells[x, y]);
        cells[x, y].CurrentGem = gem;
    }

    private void RemoveAllInitialMatches(int maxIterations)
    {
        int iter = 0;
        List<Gem> matches = matchFinder.FindMatches(cells);

        while (matches != null && matches.Count > 0 && iter < maxIterations)
        {
            iter++;
            HashSet<Gem> unique = new HashSet<Gem>(matches);

            // Replace the found gems with random ones until the matches disappear.
            foreach (var g in unique)
            {
                Vector2Int pos = g.Cell.GridPos;
                Destroy(g.gameObject);
                Gem newGem = gemFactory.CreateRandomGem();
                newGem.SetCell(cells[pos.x, pos.y]);
                cells[pos.x, pos.y].CurrentGem = newGem;
            }
            matches = matchFinder.FindMatches(cells);
        }

        if (iter >= maxIterations)
        {
            Debug.LogWarning("RemoveAllInitialMatches: reached iteration limit. Check MatchFinder logic.");
        }
    }

    public void SwapGemsData(Gem a, Gem b)
    {
        Cell cellA = a.Cell;
        Cell cellB = b.Cell;

        // Change link CurrentGem
        cells[cellA.GridPos.x, cellA.GridPos.y].CurrentGem = b;
        cells[cellB.GridPos.x, cellB.GridPos.y].CurrentGem = a;

        // Updating fields in gems
        a.SetCell(cells[cellB.GridPos.x, cellB.GridPos.y]);
        b.SetCell(cells[cellA.GridPos.x, cellA.GridPos.y]);
    }

    /// <summary>
    /// Match Processing
    /// </summary>
    /// <param name="matches"></param>
    public void ProcessMatches(List<Gem> matches)
    {
        StartCoroutine(ProcessMatchesRoutine(matches));
    }

    private IEnumerator ProcessMatchesRoutine(List<Gem> matches)
    {
        IsBusy = true;
        HashSet<Gem> unique = new HashSet<Gem>(matches);

        // If scoreManager is not empty and there are more than 0 elements in the list, then we award points.
        if (scoreManager != null && unique.Count > 0)
        {
            int totalPoints = unique.Count * pointsPerGem;
            scoreManager.Add(totalPoints);
        }

        // Removing gems
        foreach (var g in unique)
        {
            var pos = g.Cell.GridPos;
            cells[pos.x, pos.y].CurrentGem = null;
            Destroy(g.gameObject); 
        }

        int count = unique.Count;

        // If the number of matched gems is more than 4, then we display the popupText message.
        if (count >= 4)
        {
            string popupText = count switch
            {
                4 => "GOOD!",
                5 => "AMAZING!",
                >= 6 => "INSANE!",
                _ => ""
            };

            ComboPopupController.Instance.ShowPopup(popupText);
        }


        // Waiting to see the destruction of gems
        yield return new WaitForSeconds(0.1f);

        // When falling - move the gem down for each column
        for (int x = 0; x < width; x++)
        {
            int writeY = 0;
            for (int y = 0; y < height; y++)
            {
                if (cells[x, y].CurrentGem != null)
                {
                    if (writeY != y)
                    {
                        // Move gem to cell[x,writeY]
                        Gem gem = cells[x, y].CurrentGem;
                        cells[x, writeY].CurrentGem = gem;
                        gem.SetCell(cells[x, writeY]);
                        cells[x, y].CurrentGem = null;
                    }
                    writeY++;
                }
            }

            // Filling with new gems from above
            for (int y = writeY; y < height; y++)
            {
                Vector3 spawnPos = new Vector3(x, y + 4, 0);
                Gem newGem = gemFactory.CreateRandomGem();
                newGem.transform.position = spawnPos;
                newGem.SetCell(cells[x, y]);
                cells[x, y].CurrentGem = newGem;
            }
        }

        // Animate all gems falling until to their cell.WorldPosition
        yield return StartCoroutine(AnimateAllGemsToCells());

        List<Gem> newMatches = matchFinder.FindMatches(cells);
        if (newMatches.Count > 0)
        {
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(ProcessMatchesRoutine(newMatches));
            while (IsBusy)
                yield return null;
        }
        else
        {
            IsBusy = false;
        }
    }

    private IEnumerator AnimateAllGemsToCells()
    {
        bool moving = true;

        while (moving)
        {
            moving = false;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gem gem = cells[x, y].CurrentGem;
                    
                    if (gem == null) 
                    {
                        continue;
                    } 
                    Vector3 target = cells[x, y].WorldPosition;
                    
                    if ((gem.transform.position - target).sqrMagnitude > 0.001f)
                    {
                        gem.transform.position = Vector3.Lerp(gem.transform.position, target, Time.deltaTime * fallAnimSpeed);
                        moving = moving || true;
                    }
                    else
                    {
                        gem.transform.position = target;
                    }
                }
            }
            yield return null;
        }
    }
}