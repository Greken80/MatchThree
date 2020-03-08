using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : Singleton<MatchFinder>
{

    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    [SerializeField] List<GameObject> adjecentTiles;




    public IEnumerator CheckForMatches(GameObject obj)
    {
        //Must wait so the raycast starts from the tiles new position
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        ClearMatch(obj);
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

    private List<GameObject> GetAllAdjacentTiles()
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
        }
        return adjacentTiles;
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




    public void ClearMatch(GameObject targetObj) 
    {

        List<GameObject> matchingTilesVertical = new List<GameObject>(); // 2
        List<GameObject> matchingTilesHorizontal = new List<GameObject>();

        

       

        for (int i = 0; i < adjacentDirections.Length; i++) // 3
        {
            matchingTilesVertical.AddRange(FindMatch(adjacentDirections[i], targetObj.transform));

        }
        if (matchingTilesVertical.Count >= 2) // 4
        {
            for (int i = 0; i < matchingTilesVertical.Count; i++) // 5
            {
                matchingTilesVertical[i].GetComponent<SpriteRenderer>().sprite = null;
            }
           targetObj.GetComponent<SpriteRenderer>().sprite = null;
          //  matchFound = true; // 6
        }
    }
}
