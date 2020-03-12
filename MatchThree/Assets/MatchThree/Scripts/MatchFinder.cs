using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : Singleton<MatchFinder>
{

    public void CheckForMatches(GameObject[] objects)
    {
        
        foreach (GameObject obj in objects)
        {

            //Turned off diagonal search because of a "bug". Tiles dosent get a new sprite sometimes, results in one or more empty tiles in the board.
            //The bug is in the tile shifter. During a diagonally match it is possible that the column can have empty-filled-empty sprites. This causes the current shifter to behave porly.
            //Checks diagonal for matches Top Right/Bottom Left;     
            //CheckMatches(obj, new Vector2[] { new Vector2(1, 1), new Vector2(-1, -1) });

            //Checks diagonal for matches Top left/Bottom Right;
            //CheckMatches(obj, new Vector2[] { new Vector2(-1, 1), new Vector2(1, -1) });

            //Checking for vertical matches
            CheckMatches(obj, new Vector2[] { Vector2.up, Vector2.down });

            //Checking for horizontal matches
            CheckMatches(obj, new Vector2[] { Vector2.left, Vector2.right });  

        }
           

            GameObject[,] tiles = BoardManager.Instance.tilesArray;

        foreach (GameObject obj in tiles)
        {
            ClearTilesIfMatched(obj);
        }

        BoardManager.Instance.StopAllCoroutines();
        StartCoroutine(BoardManager.Instance.FindNullTiles());

    }


        private List<GameObject> FindMatch(Vector2 castDir, Transform rayOrigin)
    {
        rayOrigin.GetComponent<BoxCollider>().enabled = false;
        List<GameObject> matchingTiles = new List<GameObject>();

        Ray ray = new Ray(rayOrigin.position, castDir);
        RaycastHit hit;

        Sprite originSprite = rayOrigin.GetComponent<SpriteRenderer>().sprite;

        if (Physics.Raycast(ray, out hit))
        {
#if UNITY_EDITOR
            Debug.DrawRay(rayOrigin.position, castDir * 100, Color.magenta, 1f, false);
#endif

            while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == originSprite)
            {
                matchingTiles.Add(hit.collider.gameObject);
                ray = new Ray(hit.collider.transform.position, castDir);
                Physics.Raycast(ray, out hit);
            }
        }
        rayOrigin.GetComponent<BoxCollider>().enabled = true;
        return matchingTiles;
    }


    private void CheckMatches(GameObject targetObj, Vector2[] directions)
    {

        List<GameObject> matchedTiles = new List<GameObject>();


        for (int i = 0; i < directions.Length; i++)
        {
            matchedTiles.AddRange(FindMatch(directions[i], targetObj.transform));

            if (matchedTiles.Count >= 2)
            {          

                for (int y = 0; y < matchedTiles.Count; y++)
                {
                    matchedTiles[y].GetComponent<Tile>().matchFound = true;
                }
                targetObj.GetComponent<Tile>().matchFound = true;           
            }
        }

        if(matchedTiles.Count >= 2)
        {
            //Adding +1 becuase the Tile dosent count itself as a match
            ScoreManager.Instance.AddPoints(matchedTiles.Count + 1);
        }
      

    }

    public void ClearAllMatches(GameObject obj)
    {

        if (obj.GetComponent<SpriteRenderer>().sprite == null)
            return;

        CheckMatches(obj, new Vector2[2] { Vector2.left, Vector2.right });
        CheckMatches(obj, new Vector2[2] { Vector2.up, Vector2.down });

        //Turned off diagonal matching
        //Checks diagonal for matches Top Right/Bottom Left;     
        //CheckMatches(obj, new Vector2[] { new Vector2(1, 1), new Vector2(-1, -1) });

        //Checks diagonal for matches Top left/Bottom Right;
       // CheckMatches(obj, new Vector2[] { new Vector2(-1, 1), new Vector2(1, -1) });

        if (obj.GetComponent<Tile>().matchFound)
        {
            obj.GetComponent<SpriteRenderer>().sprite = null;
            obj.GetComponent<Tile>().matchFound = false;

            BoardManager.Instance.StopAllCoroutines();
            StartCoroutine(BoardManager.Instance.FindNullTiles());

        }

    }


    public void ClearTilesIfMatched(GameObject obj)
    {
    
        if (obj.GetComponent<Tile>().matchFound)
        {
            obj.GetComponent<SpriteRenderer>().sprite = null;
            obj.GetComponent<Tile>().matchFound = false;      
        }
    }


}
