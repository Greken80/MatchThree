using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ISelectable
{

    private bool isSelected;

    [SerializeField]private Vector3 startingPos;

    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    [SerializeField] List<GameObject> adjecentTiles;

    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        
    }


    public void OnSelected()
    {
        startingPos = transform.position;
        adjecentTiles = GetAdjacentTiles();
        isSelected = true;
        Debug.Log("I was hit: " + gameObject.name);
    }


    public void OnDeselected()
    {
        if (!CheckIfOverlaps())
        {
            transform.position = startingPos;
        }
        isSelected = false;

    }

    private List<GameObject> GetAdjacentTiles()
    {
        boxCollider.enabled = false;
        Collider[] adjecentColliders = Physics.OverlapSphere(transform.position, 3f);

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

    private List<GameObject> GetAllAdjacentTiles()
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
        }
        return adjacentTiles;
    }

    bool CheckIfOverlaps()
    {
        Vector3 otherTilePosition;

        foreach (GameObject obj in adjecentTiles)
        {
            if (boxCollider.bounds.Intersects(obj.GetComponent<BoxCollider>().bounds))
            {
                otherTilePosition = obj.transform.position;
                if (Vector3.Distance(transform.position, obj.transform.position) < 1)
                {
                    transform.position = otherTilePosition;
                    obj.transform.position = startingPos;
                    return true;
                }
            }
        }
        return false;


    }

}
