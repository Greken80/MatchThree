using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : Singleton<MatchFinder>
{

    public void CheckForMatchesTest(GameObject obj)
    {

        //Checking for vertical matches
        CheckMatches(obj, new Vector2[] { Vector2.up, Vector2.down });

        //Checking for horizontal matches
        CheckMatches(obj, new Vector2[] { Vector2.left, Vector2.right });

        //CheckMatches(obj, new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right });

        ClearAllMatches(obj);

    }


    public void CheckForMatchesTest(GameObject[] objects)
    {

        foreach (GameObject obj in objects)
        {
            //Checking for vertical matches
            CheckMatches(obj, new Vector2[] { Vector2.up, Vector2.down });

            //Checking for horizontal matches
            CheckMatches(obj, new Vector2[] { Vector2.left, Vector2.right });

           // CheckMatches(obj, new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right });
          
        }
     
        foreach (GameObject obj in objects)
        {
            ClearAllMatches(obj);
        }

    }


    private List<GameObject> FindMatch(Vector2 castDir, Transform rayOrigin)
    {
        rayOrigin.GetComponent<BoxCollider>().enabled = false;
        List<GameObject> matchingTiles = new List<GameObject>();

        Ray ray = new Ray(rayOrigin.position, castDir);
        RaycastHit hit;

        Sprite objSprite = rayOrigin.GetComponent<SpriteRenderer>().sprite;

        if (Physics.Raycast(ray, out hit))
        {
            while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == objSprite)
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

        }

        if (matchedTiles.Count >= 2)
        {
            for (int i = 0; i < matchedTiles.Count; i++)
            {
                matchedTiles[i].GetComponent<SpriteRenderer>().sprite = null;
            }

            targetObj.GetComponent<Tile>().matchFound = true;
        }

    }

    public void ClearAllMatches(GameObject obj)
    {

        if (obj.GetComponent<SpriteRenderer>().sprite == null)
            return;

        //CheckMatches(obj, new Vector2[2] { Vector2.left, Vector2.right });
       // CheckMatches(obj, new Vector2[2] { Vector2.up, Vector2.down });

        if (obj.GetComponent<Tile>().matchFound)
        {
            obj.GetComponent<SpriteRenderer>().sprite = null;
            obj.GetComponent<Tile>().matchFound = false;

         
            BoardManager.Instance.StopAllCoroutines();
           // StopCoroutine(BoardManager.Instance.FindNullTiles()); 
            StartCoroutine(BoardManager.Instance.FindNullTiles()); 

        }

    }


}
