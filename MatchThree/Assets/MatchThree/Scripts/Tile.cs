using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ISelectable
{

    [SerializeField] private Vector3 startingPos;
    [SerializeField] List<GameObject> adjecentTiles;

    private BoxCollider boxCollider;
    private SpriteRenderer spriteRenderer;
    private GameObject swappedObject;

    public bool matchFound = false;
 

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

        if (!CheckIfOverlap())
        {
            transform.position = startingPos;
        }
        else
        {

            MatchFinder.Instance.CheckForMatches(new GameObject[] { gameObject, swappedObject });
        }

        adjecentTiles.Clear();

    }

    // Finds all adjecent tiles in all directions
    private List<GameObject> GetAdjacentTiles()
    {
        float detectionRadius = boxCollider.bounds.size.x;

        boxCollider.enabled = false;
        Collider[] adjecentColliders = Physics.OverlapSphere(transform.position, detectionRadius + BoardManager.Instance.tilePadding);

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
                    tempSprite = spriteRenderer.sprite;
                    spriteRenderer.sprite = obj.GetComponent<SpriteRenderer>().sprite;
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
