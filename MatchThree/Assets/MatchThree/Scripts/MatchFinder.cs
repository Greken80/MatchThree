using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : Singleton<MatchFinder>
{

    bool isRunning = false;

    public IEnumerator CheckForMatches(GameObject obj)
    {
        if (!isRunning)
        {
            isRunning = true;
            //Must wait so the raycast starts from the tile new position
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            //Checking for vertical matches
            CheckMatches(obj, new Vector2[] { Vector2.up, Vector2.down });

            //Checking for horizontal matches
            CheckMatches(obj, new Vector2[] { Vector2.left, Vector2.right });

            // CheckMatches(obj, new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right });



            isRunning = false;
        }
        StartCoroutine(BoardManager.Instance.FindNullTiles());
    }

    public void CheckForMatchesTest(GameObject obj)
    {

        //Checking for vertical matches
        CheckMatches(obj, new Vector2[] { Vector2.up, Vector2.down });

        //Checking for horizontal matches
        CheckMatches(obj, new Vector2[] { Vector2.left, Vector2.right });

        // CheckMatches(obj, new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right });

        StartCoroutine(BoardManager.Instance.FindNullTiles());
    }


    private GameObject GetAdjacent(Vector2 castDir)
    {
        Ray ray = new Ray(transform.position, castDir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }


    private List<GameObject> FindMatch(Vector2 castDir, Transform transformToRaycastFrom)
    {
        List<GameObject> matchingTiles = new List<GameObject>();

        Ray ray = new Ray(transformToRaycastFrom.position, castDir);
        RaycastHit hit;

        Sprite objSprite = transformToRaycastFrom.GetComponent<SpriteRenderer>().sprite;

        if (Physics.Raycast(ray, out hit))
        {
            while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == objSprite)
            {
                matchingTiles.Add(hit.collider.gameObject);
                ray = new Ray(hit.collider.transform.position, castDir);
                Physics.Raycast(ray, out hit);
            }
        }

        return matchingTiles;
    }


    private void CheckMatches(GameObject targetObj, Vector2[] directions)
    {
        List<GameObject> matchedTiles = new List<GameObject>();


        for (int i = 0; i < directions.Length; i++)
        {
            matchedTiles.AddRange(FindMatch(directions[i], targetObj.transform));

        }
        if (matchedTiles.Count >= 2)
        {
            for (int i = 0; i < matchedTiles.Count; i++)
            {
                matchedTiles[i].GetComponent<SpriteRenderer>().sprite = null;
            }
            targetObj.GetComponent<SpriteRenderer>().sprite = null;

        }

        matchedTiles.Clear();

    }

    /*
    public void ClearAllMatches(GameObject obj)
    {
        if (obj.GetComponent<SpriteRenderer>().sprite == null)
            return;

        CheckMatches(obj,new Vector2[2] { Vector2.left, Vector2.right });
        CheckMatches(obj, new Vector2[2] { Vector2.up, Vector2.down });
        if (matchFound)
        {
            render.sprite = null;
            matchFound = false;
            StopCoroutine(BoardManager.instance.FindNullTiles()); //Add this line
            StartCoroutine(BoardManager.instance.FindNullTiles()); //Add this line
            SFXManager.instance.PlaySFX(Clip.Clear);
        }
    }

    */
}
