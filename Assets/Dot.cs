using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Dot : MonoBehaviour
{

    
    public int xAll;
    public int yAll;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;     
    private FindMatches findMatches;
    private Board gameBoard;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition;              
    public Dot selectedObject;
    public bool isHorizontalDrag;
    public bool isVerticalDrag;
    public bool isDragging;
    public GameObject[] clonesRight;
    public GameObject[] clonesLeft;
    public GameObject[] selectedOnes;
    public bool XRightBool;
    public bool XLeftBool;
    public bool YUpBool;
    public bool YDownBool;
    public List<GameObject> selectedOnesList;
    public int BoardMatchCheckerInt;
           
    void Start()
    {
               
       
        gameBoard = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
    }


  


  
    void Update()
    {
        targetX = xAll;
        targetY = yAll;
        if (Mathf.Abs(targetX - transform.position.x) > .01)
        {          
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if(xAll <= gameBoard.width && yAll <= gameBoard.height)
            {
                if (gameBoard.allPieces[xAll, yAll] != this.gameObject)
                {

                    gameBoard.allPieces[xAll, yAll] = this.gameObject;
                }
            }
            
            findMatches.FindAllMatches();
        } else {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .01)
        {         
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (gameBoard.allPieces[xAll, yAll] != this.gameObject)
            {
                gameBoard.allPieces[xAll, yAll] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }  else {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    

    private void OnMouseDown()
    {     
        if (gameBoard.currentState == GameState.move)
        {
            startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(startTouchPosition, Vector2.zero);
            if (hit.collider != null)
            {
                selectedObject = hit.collider.GetComponent<Dot>();

                if (selectedObject != null)
                {

                    isDragging = true;

                    foreach (GameObject comp in gameBoard.allPieces)
                    {
                        comp.GetComponent<Dot>().previousColumn = comp.GetComponent<Dot>().xAll;
                        comp.GetComponent<Dot>().previousRow = comp.GetComponent<Dot>().yAll;
                    }
                }
            }
        }
        isHorizontalDrag = false;
        isVerticalDrag = false;
        //clonesRight = new GameObject[selectedObject.yAll];
        //clonesLeft = new GameObject[selectedObject.yAll];     
    }

    

    

    void OnMouseDrag()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - startTouchPosition);

        if (isDragging)
        {
                                    
            if (!isHorizontalDrag && !isVerticalDrag && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                isHorizontalDrag = true;                                  
            } else if (!isHorizontalDrag && !isVerticalDrag && Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            {
                isVerticalDrag = true;
            }

            if (isHorizontalDrag)
            {
                float a = mousePosition.x - selectedObject.transform.position.x;        
                foreach (GameObject component in gameBoard.allPieces)
                {
                    if (component != null)
                    {
                        if (component == selectedObject)
                        {
                            //Debug.Log(component.name);
                            component.transform.position = new Vector3(mousePosition.x, component.transform.position.y, 0);
                        }
                        if (component.GetComponent<Dot>().yAll == selectedObject.yAll && component != selectedObject)
                        {                         
                            //Debug.Log(component.name);
                            //Debug.Log(mousePosition.x - selectedObject.transform.position.x);
                            component.transform.position = new Vector3(component.transform.position.x + (a), component.transform.position.y, 0);
                        }
                    }
                }
            }
            else if (isVerticalDrag)
            { 
                float b = mousePosition.y - selectedObject.transform.position.y;
                foreach (GameObject component in gameBoard.allPieces)
                {
                    if (component != null)
                    {
                        if (component == selectedObject)
                        {
                            //Debug.Log(component.name);
                            component.transform.position = new Vector3(component.transform.position.x, mousePosition.y, 0);
                        }
                        if (component.GetComponent<Dot>().xAll == selectedObject.xAll && component != selectedObject)
                        {

                            
                            component.transform.position = new Vector3(component.transform.position.x, component.transform.position.y + (b), 0);
                        }
                    }

                }
            }

        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);

        bool BoardMatchCheckerBool = false;
        if (selectedOnesList.Count > 0) {
            foreach (GameObject component in selectedOnesList)
            {            
                //Debug.Log(BoardMatchCheckerInt);
                if (component != null)
                {

                    if (component.GetComponent<Dot>().isMatched != true)
                    {

                    }
                    else
                    {
                        BoardMatchCheckerBool = true;
                    }
                }                             
            }

            if (!BoardMatchCheckerBool)
            {
                foreach (GameObject selectedOne in selectedOnesList)
                {
                    selectedOne.GetComponent<Dot>().xAll = selectedOne.GetComponent<Dot>().previousColumn;
                    selectedOne.GetComponent<Dot>().yAll = selectedOne.GetComponent<Dot>().previousRow;

                }
                gameBoard.currentState = GameState.move;


            }
            else
            {
                gameBoard.DestroyMatches();
            }
        }     
    }
    void OnMouseUp()
    {
        //Debug("MouseUp");
        //component.GetComponent<Dot>().isDragging = false;
        if (gameBoard.currentState == GameState.move)
        {
            //

        }
        endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(endTouchPosition.x - startTouchPosition.x); !!!
        //Debug.Log(endTouchPosition.y - startTouchPosition.y); !!!

        if (isHorizontalDrag) {
            if (endTouchPosition.x - endTouchPosition.x > 0.40f) {
                XRightBool = true;

            } else if (endTouchPosition.x - startTouchPosition.x < -0.40f)  {
                XLeftBool = true;
            }

        } else if (isVerticalDrag)  {
            if (endTouchPosition.y - startTouchPosition.y > 0.40f) {
                YUpBool = true;

            }  else if (endTouchPosition.y - startTouchPosition.y < -0.40f) {
                YDownBool = true;
            }
        }
        Vector2 directionEnd = (startTouchPosition - endTouchPosition);
        //Debug.Log(endTouchPosition.x - startTouchPosition.x); !!!
        //Debug.Log(endTouchPosition.y - startTouchPosition.y); !!!
        //Vector2 directionEnd = (startTouchPosition - endTouchPosition);
        //Debug.Log(directionEnd.x );

        if (isVerticalDrag || isHorizontalDrag)
        {
            isHorizontalDrag = false;
            isVerticalDrag = false;

            foreach (GameObject component in gameBoard.allPieces)
            {
                {
                    if((Mathf.RoundToInt(component.transform.position.x)) < 0)
                    {
                        component.GetComponent<Dot>().xAll = (Mathf.RoundToInt(component.transform.position.x) + gameBoard.width);
                    }else if ((Mathf.RoundToInt(component.transform.position.x)) >= gameBoard.width){
                        component.GetComponent<Dot>().xAll = (Mathf.RoundToInt(component.transform.position.x) - gameBoard.width);
                    }
                    else
                    {
                        component.GetComponent<Dot>().xAll = (Mathf.RoundToInt(component.transform.position.x));
                    }

                    if ((Mathf.RoundToInt(component.transform.position.y)) < 0)
                    {
                        component.GetComponent<Dot>().yAll = (Mathf.RoundToInt(component.transform.position.y) + gameBoard.height);
                    }
                    else if ((Mathf.RoundToInt(component.transform.position.y)) >= gameBoard.width)
                    {
                        component.GetComponent<Dot>().yAll = (Mathf.RoundToInt(component.transform.position.y) - gameBoard.height);
                    }
                    else
                    {
                        component.GetComponent<Dot>().yAll = (Mathf.RoundToInt(component.transform.position.y));
                    }               
                }
            }

            foreach (GameObject component in gameBoard.allPieces)
            {
                if (component.GetComponent<Dot>().yAll == selectedObject.yAll && selectedOnesList.Count < gameBoard.width)
                {
                    selectedOnesList.Add(component);
                }
            }
            FindMatches();
            StartCoroutine(CheckMoveCo());
        }                      
    }

    void FindMatches()
    {
        if (xAll > 0 && xAll < gameBoard.width - 1)
        {
            GameObject leftDot1 = gameBoard.allPieces[xAll - 1, yAll];
            GameObject rightDot1 = gameBoard.allPieces[xAll + 1, yAll];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (yAll > 0 && yAll < gameBoard.height - 1)
        {
            GameObject upDot1 = gameBoard.allPieces[xAll, yAll + 1];
            GameObject downDot1 = gameBoard.allPieces[xAll, yAll - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}