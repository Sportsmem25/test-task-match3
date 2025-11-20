using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    public List<Gem> FindMatches(Cell[,] grid)
    {
        List<Gem> matches = new List<Gem>();
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        // Horizontal
        for (int y = 0; y < height; y++)
        {
            int x = 0;
            while (x < width)
            {
                List<Gem> run = new List<Gem>();
                Gem current = grid[x, y].CurrentGem;
                if (current == null)
                {
                    x++;
                    continue;
                }

                run.Add(current);
                int nx = x + 1;

                while (nx < width && grid[nx, y].CurrentGem != null && grid[nx, y].CurrentGem.GemType == current.GemType)
                {
                    run.Add(grid[nx, y].CurrentGem);
                    nx++;
                }

                if (run.Count >= 3)
                {
                    matches.AddRange(run);
                }

                x = nx;
            }
        }

        //Vertical
        for (int x = 0; x < width; x++)
        {
            int y = 0;
            while(y < height)
            {
                List<Gem> run = new List<Gem>();
                Gem current = grid[x, y].CurrentGem;
                if (current == null)
                {
                    y++;
                    continue;
                }

                run.Add(current);
                int ny = y + 1;

                while (ny < height && grid[x, ny].CurrentGem != null && grid[x, ny].CurrentGem.GemType == current.GemType)
                {
                    run.Add(grid[x, ny].CurrentGem);
                    ny++;
                }
                if(run.Count >= 3)
                {
                    matches.AddRange(run);
                }

                y = ny;
            }
        }

        return matches;
    }
}