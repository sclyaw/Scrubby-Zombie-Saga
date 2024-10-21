using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{

    private Board gameBoard;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        gameBoard = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }
    private void AddToListAndMatch(GameObject piece)
    {
        if (!currentMatches.Contains(piece))
        {
            currentMatches.Add(piece);
        }
        piece.GetComponent<Dot>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject piece1, GameObject piece2, GameObject piece3)
    {
        AddToListAndMatch(piece1);
        AddToListAndMatch(piece2);
        AddToListAndMatch(piece3);
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < gameBoard.width; i++)
        {
            for (int j = 0; j < gameBoard.height; j++)
            {
                GameObject currentDot = gameBoard.allPieces[i, j];

                if (currentDot != null)
                {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();
                    if (i > 0 && i < gameBoard.width - 1)
                    {   
                        GameObject leftDot = gameBoard.allPieces[i - 1, j];
                        GameObject rightDot = gameBoard.allPieces[i + 1, j];
                        if (leftDot != null && rightDot != null)
                        {
                            Dot rightDotDot = rightDot.GetComponent<Dot>();
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                GetNearbyPieces(leftDot, currentDot, rightDot);
                            }
                        }

                    }

                    if (j > 0 && j < gameBoard.height - 1)
                    {
                        GameObject upDot = gameBoard.allPieces[i, j + 1];
                        GameObject downDot = gameBoard.allPieces[i, j - 1];

                        if (upDot != null && downDot != null)
                        {
                            Dot downDotDot = downDot.GetComponent<Dot>();
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                GetNearbyPieces(upDot, currentDot, downDot);
                                

                            }
                        }
                    }

                }
            }
        }

    }



}