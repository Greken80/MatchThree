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

    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void OnSelected()
    {

        startingPos = transform.position;
        adjecentTiles = GetAllAdjacentTiles();

        //Debug.Log("I was hit: " + gameObject.name);
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


    private List<GameObject> GetAllAdjacentTiles()
    {
        boxCollider.enabled = false;
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
        }
        boxCollider.enabled = true;
        return adjacentTiles;
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

    bool CheckIfOverlap()
    {
        Sprite tempSprite;

        if (adjecentTiles.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < adjecentTiles.Count; i++)
        {
            if (adjecentTiles[i] != null)
            {
                if (boxCollider.bounds.Intersects(adjecentTiles[i].GetComponent<BoxCollider>().bounds))
                {
                    float overlapDetectionRange = boxCollider.bounds.extents.x;

                    if (Vector3.Distance(transform.position, adjecentTiles[i].transform.position) < overlapDetectionRange)
                    {
                        tempSprite = spriteRenderer.sprite;
                        spriteRenderer.sprite = adjecentTiles[i].GetComponent<SpriteRenderer>().sprite;
                        adjecentTiles[i].GetComponent<SpriteRenderer>().sprite = tempSprite;

                        transform.position = startingPos;

                        swappedObject = adjecentTiles[i];
                        return true;
                    }
                }
            }
        }
        return false;

    }






}
