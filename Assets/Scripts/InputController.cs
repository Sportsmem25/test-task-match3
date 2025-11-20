using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Camera cam;
    private Gem   selectedGem;
    private bool  inputLocked = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 w2 = new Vector2(world.x, world.y);
            RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);

            if(hit.collider != null)
            {
                Gem gem = hit.collider.GetComponent<Gem>();

                if(gem == null)
                {
                    return;
                }

                if (selectedGem == null)
                {
                    selectedGem = gem;
                }
                else
                {
                    TrySwap(selectedGem, gem);
                    selectedGem = null;
                }
            }
        }
    }

    public void TrySwap(Gem a, Gem b)
    {
        if(a == null || b == null)
        {
            return;
        }

        if(!AreNeighbors(a, b))
        {
            return;
        }

        StartCoroutine(TrySwapRoutine(a, b));
    }

    private bool AreNeighbors(Gem a, Gem b)
    {
        Vector2Int posA = a.Cell.GridPos;
        Vector2Int posB = b.Cell.GridPos;

        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y) == 1;
    }

    private IEnumerator TrySwapRoutine(Gem a, Gem b)
    {
        inputLocked = true;
        yield return StartCoroutine(SwapAnimation(a, b));
        GridManager.Instance.SwapGemsData(a, b);
        List<Gem> matches = GridManager.Instance.matchFinder.FindMatches(GridManager.Instance.cells);

        // If matches is not null and the number of gems is greater than 0, then we check the gems for matches.
        if (matches != null && matches.Count > 0)
        {
            GridManager.Instance.ProcessMatches(matches);
            
            while (GridManager.Instance.IsBusy)
                yield return null;
        }
        else
        {
            yield return StartCoroutine(SwapAnimation(a, b));
            GridManager.Instance.SwapGemsData(a, b);
        }

        inputLocked = false;
    }

    private  IEnumerator SwapAnimation(Gem a, Gem b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        float t = 0;
        float dur = 1f / GridManager.Instance.swapAnimSpeed;

        while (t < dur)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / dur);
            a.transform.position = Vector3.Lerp(posA, posB, p);
            b.transform.position = Vector3.Lerp(posB, posA, p);
            yield return null;
        }

        // Checking that the correct positions
        a.transform.position = posB;
        b.transform.position = posA;
    }
}
