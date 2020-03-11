using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ISelectable
{



    [SerializeField] private Vector3 startingPos;
    [SerializeField] List<GameObject> adjecentTiles;



    private BoxCollider boxCollider;
    private SpriteRenderer spriteRenderer;

    public bool matchFound = false;

    private GameObject swappedObject;

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
       //CheckIfOverlap();



        if (!CheckIfOverlap())
        {
            transform.position = startingPos;
        }
        else
        {
            //MatchFinder.Instance.CheckForMatchesTest(swappedObject);
            MatchFinder.Instance.CheckForMatchesTest(new GameObject[]{ gameObject, swappedObject});
        }

       // swappedObject = null;
        adjecentTiles.Clear();


    }

    // Finds all adjecent tiles in all directions
    private List<GameObject> GetAdjacentTiles()
    {
        float detectionRadius = boxCollider.bounds.size.x;

        boxCollider.enabled = false;
        Collider[] adjecentColliders = Physics.OverlapSphere(transform.position, detectionRadius);

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

    //Finds adjencent tiles in Top, Down, Left, Right directions
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

    bool CheckIfOverlap()
    {
        Sprite tempSprite;
        if (adjecentTiles.Count == 0)
        {
            return false;
        }

        foreach (GameObject obj in adjecentTiles)
        {
            if (boxCollider.bounds.Intersects(obj.GetComponent<BoxCollider>().bounds))
            {
                float overlapDetectionRange = boxCollider.bounds.extents.x;

                if (Vector3.Distance(transform.position, obj.transform.position) < overlapDetectionRange)
                {
                    tempSprite = transform.GetComponent<SpriteRenderer>().sprite;
                    transform.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                    obj.GetComponent<SpriteRenderer>().sprite = tempSprite;

                    transform.position = startingPos;

                    swappedObject = obj;
                    return true;
                }
            }
        }
        return false;

    }

}
