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


    //Made it serialized to be able to inspect it in editor
    [SerializeField] private List<SpriteRenderer> renders;

    private GameObject[,] tilesArray;

    public bool IsShifting { get; set; }

    void Start()
    {
        CreateBoard();

    }

    private void CreateBoard()
    {
        Vector2 size = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;
        SetupBoard(size.x, size.y);
    }


    private void SetupBoard(float xOffset, float yOffset)
    {
        tilesArray = new GameObject[xSize, ySize];

      
        float startX = transform.position.x;
        float startY = transform.position.y;

        Sprite[] previousLeft = new Sprite[ySize];
        Sprite previousBelow = null;
        Sprite tempSprite;
  
        for (int x = 0; x < xSize; x++)
        {      
            for (int y = 0; y < ySize; y++)
            {          
                GameObject tile = Instantiate(tilePrefab, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), Quaternion.identity);
                tilesArray[x, y] = tile;

                tile.gameObject.name += "posX:"+x + " posY: " + y;
                tile.transform.SetParent(transform);

                List<Sprite> possibleSprites = new List<Sprite>();

                possibleSprites.AddRange(spritesList); 
                possibleSprites.Remove(previousLeft[y]); 
                possibleSprites.Remove(previousBelow);

                tempSprite = possibleSprites[Random.Range(0, possibleSprites.Count)];
                tile.GetComponentInChildren<SpriteRenderer>().sprite = tempSprite;

                previousLeft[y] = tempSprite;
                previousBelow = tempSprite;
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
               MatchFinder.Instance.ClearAllMatches(tilesArray[x, y]);
            }
        }
       
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = 0.1f)
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
                renders[k + 1].sprite = GetNewSprite(x, ySize - 1);

            }
        }
     
        IsShifting = false;
    }


    
    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleSprites = new List<Sprite>();
        possibleSprites.AddRange(spritesList);
        
        if (x > 0)
        {
            possibleSprites.Remove(tilesArray[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < xSize - 1)
        {
            possibleSprites.Remove(tilesArray[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleSprites.Remove(tilesArray[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }
        
        return possibleSprites[Random.Range(0, possibleSprites.Count)];
    }

    public void ResetBoard()
    {
        StopAllCoroutines();
        IsShifting = true;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Destroy(tilesArray[x, y]);
            }
        }
        ScoreManager.Instance.ScoreReset();
        CreateBoard();
        IsShifting = false;
    }


}
