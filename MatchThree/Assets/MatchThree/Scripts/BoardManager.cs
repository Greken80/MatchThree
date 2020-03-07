using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    
    [Tooltip("The sprites that the gameboard will randomise from")]
    [SerializeField] private List<Sprite> spritesList = new List<Sprite>(); 

    [Tooltip("Prefab that will be used for the tiles")]
    [SerializeField] private GameObject tilePrefab;      

    [Header("Board configuration")]
    [SerializeField] private int xSize;
    [SerializeField] private int ySize;

    private GameObject[,] tilesArray;

  

    public bool IsShifting { get; set; }

    void Start()
    {
        Vector2 size = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(size.x, size.y);
    }

    private void CreateBoard(float xOffset, float yOffset)
    {
        tilesArray = new GameObject[xSize, ySize]; 


        float startX = transform.position.x;
        float startY = transform.position.y;

        Sprite tempSprite;
   
        for (int x = 0; x < xSize; x++)
        {      // 11
            for (int y = 0; y < ySize; y++)
            {
                int spriteNumber = Random.Range(0, spritesList.Count);
                GameObject newTile = Instantiate(tilePrefab, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);
                tilesArray[x, y] = newTile;

                newTile.gameObject.name += "posX:"+x + " posY: " + y;
                newTile.transform.SetParent(transform);    

                tempSprite = spritesList[Random.Range(0, spritesList.Count)];
                newTile.GetComponentInChildren<SpriteRenderer>().sprite = tempSprite;

            }
        }
    }



}
