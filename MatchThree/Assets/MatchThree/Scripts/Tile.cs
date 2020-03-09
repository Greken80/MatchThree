using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ISelectable
{

   

    [SerializeField] private Vector3 startingPos;

    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    [SerializeField] List<GameObject> adjecentTiles;

    private BoxCollider boxCollider;
    private SpriteRenderer spriteRenderer;

    private bool matchFound = false;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void OnSelected()
    {
        startingPos = transform.position;
        adjecentTiles = GetAdjacentTiles();
       
        Debug.Log("I was hit: " + gameObject.name);
    }


    public void OnDeselected()
    {

        bool isOverlapping = CheckIfOverlaps();
        if (!isOverlapping)
        {
            transform.position = startingPos;
            return;
        }


        //Need to wait for the positions to change
        
    }


    private List<GameObject> GetAdjacentTiles()
    {
        boxCollider.enabled = false;
        Collider[] adjecentColliders = Physics.OverlapSphere(transform.position, 5f);

        if (adjecentColliders.Length == 0)
        {
            return null;
        }

        List<GameObject> tempObjList = new List<GameObject>();
        foreach (Collider c in adjecentColliders)
        {

            tempObjList.Add(c.gameObject);

        }
        boxCollider.enabled = true;
        return tempObjList;

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

    bool CheckIfOverlaps()
    {
        Vector3 otherTilePosition;
        Sprite tempSprite;
        if (adjecentTiles.Count == 0)
        {
            return false;
        }

        foreach (GameObject obj in adjecentTiles)
        {
            if (boxCollider.bounds.Intersects(obj.GetComponent<BoxCollider>().bounds))
            {
                otherTilePosition = obj.transform.position;

                float overlapRange = boxCollider.bounds.extents.x;

                if (Vector3.Distance(transform.position, obj.transform.position) < overlapRange)
                {
                    tempSprite = transform.GetComponent<SpriteRenderer>().sprite;
                    transform.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                    obj.GetComponent<SpriteRenderer>().sprite = tempSprite;

                    transform.position = startingPos;
                    // obj.transform.position = startingPos;

                    MatchFinder.Instance.CheckForMatchesTest(obj);
                   // StartCoroutine(MatchFinder.Instance.CheckForMatches(obj));
                    return true;
                }
            }
        }
        return false;

    }

}
