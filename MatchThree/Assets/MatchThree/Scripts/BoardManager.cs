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

    public List<SpriteRenderer> renders;

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
        {      
            for (int y = 0; y < ySize; y++)
            {          
                GameObject tile = Instantiate(tilePrefab, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);
                tilesArray[x, y] = tile;

                tile.gameObject.name += "posX:"+x + " posY: " + y;
                tile.transform.SetParent(transform);    

                tempSprite = spritesList[Random.Range(0, spritesList.Count)];
                tile.GetComponentInChildren<SpriteRenderer>().sprite = tempSprite;

            }
        }
    }

    public IEnumerator FindNullTiles()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (tilesArray[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
                }
            }
        }


        
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
               yield return StartCoroutine(MatchFinder.Instance.ClearAllMatchesAsync(tilesArray[x, y]));
               //MatchFinder.Instance.ClearAllMatches(tilesArray[x, y]);
            }
        }
        
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = 0.5f)
    {
        IsShifting = true;
       
        renders = new List<SpriteRenderer>();
        int nullCount = 0;

        for (int y = yStart; y < ySize; y++)
        {  
            SpriteRenderer render = tilesArray[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null)
            { 
                nullCount++;
            }
            renders.Add(render);
        }

        for (int i = 0; i < nullCount; i++)
        { 
            yield return new WaitForSeconds(shiftDelay);

            for (int k = 0; k < renders.Count-1; k++)
            { 
                renders[k].sprite = renders[k + 1].sprite;
                renders[k + 1].sprite = GetNewSprite(x, ySize);
               
            }
        }
     


        IsShifting = false;
    }


    
    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(spritesList);
        /*
        if (x > 0)
        {
            possibleCharacters.Remove(tilesArray[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < xSize - 1)
        {
            possibleCharacters.Remove(tilesArray[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleCharacters.Remove(tilesArray[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }
        */
        return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
    }

}
