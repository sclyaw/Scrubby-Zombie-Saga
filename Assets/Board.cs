using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int width = 0;
    public int height = 0;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject[] dots;
    public GameObject[,] allPieces;
    private FindMatches findMatches;
    public LevelManager levelManager;
     
    void Start()
    {         
        findMatches = FindObjectOfType<FindMatches>();
        levelManager = FindObjectOfType<LevelManager>();
        allPieces = new GameObject[width, height];
        SetUp();
    }
   
    private void SetUp()
    {      
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offSet);
                Vector2 tilePosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";

                int dotToUse = Random.Range(0, dots.Length);

                int maxIterations = 0;

                while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }
                maxIterations = 0;

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().yAll = j;
                dot.GetComponent<Dot>().xAll = i;
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";
                allPieces[i, j] = dot;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allPieces[column - 1, row] != null && allPieces[column - 2, row] != null)
            {
                if (allPieces[column - 1, row].tag == piece.tag && allPieces[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
            if (allPieces[column, row - 1] != null && allPieces[column, row - 2] != null)
            {
                if (allPieces[column, row - 1].tag == piece.tag && allPieces[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }

        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allPieces[column, row - 1] != null && allPieces[column, row - 2] != null)
                {
                    if (allPieces[column, row - 1].tag == piece.tag && allPieces[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            if (column > 1)
            {
                if (allPieces[column - 1, row] != null && allPieces[column - 2, row] != null)
                {
                    if (allPieces[column - 1, row].tag == piece.tag && allPieces[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private void DestroyMatchesAt(int column, int row)
    {
        if (allPieces[column, row].GetComponent<Dot>().isMatched)
        {
            levelManager.GetTag((allPieces[column, row].tag));
            
            Destroy(allPieces[column, row]);

           
            allPieces[column, row] = null;

        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allPieces[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo());
    }

   

    private IEnumerator DecreaseRowCo()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
               
                if (allPieces[i, j] == null)
                {
                   
                    for (int k = j + 1; k < height; k++)
                    {
                        
                        if (allPieces[i, k] != null)
                        {
                            
                            allPieces[i, k].GetComponent<Dot>().yAll = j;
                           
                            allPieces[i, k] = null;
                           
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allPieces[i, j] == null )
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    int maxIterations = 0;

                    while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                    {
                        maxIterations++;
                        dotToUse = Random.Range(0, dots.Length);
                    }

                    maxIterations = 0;
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allPieces[i, j] = piece;
                    piece.GetComponent<Dot>().yAll = j;
                    piece.GetComponent<Dot>().xAll = i;

                    if (allPieces[i, j] != piece.gameObject)
                    {
                        allPieces[i, j] = piece.gameObject;
                    }

                }
            }
        }
    }
    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allPieces[i, j] != null)
                {
                    if (allPieces[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator FillBoardCo()
    {
        
        RefillBoard();
        yield return new WaitForSeconds(0.5f);

        while (MatchesOnBoard())
        {
            
            DestroyMatches();
            yield return new WaitForSeconds(1f);

        }
        findMatches.currentMatches.Clear();      
        yield return new WaitForSeconds(0.5f);
        
        currentState = GameState.move;  
    }
}