using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TMP_Text itemCountTextEnergy;
    public TMP_Text itemCountTextTomato;
    public TMP_Text itemCountTextGun;
    public TMP_Text itemCountTextArmor;
    public TMP_Text itemCountTextPan;
    public int armorCounter;
    public int energyCounter;
    public int gunCounter;
    public int panCounter;
    public int tomatoCounter;
    public int totalItems; 
    public int collectedItems; 
    public Board gameBoard;


    public int totalItemsEnergy;
    public int totalItemsTomato;
    public int totalItemsGun;
    public int totalItemsArmor;
    public int totalItemsPan;

    public Image tickImageFirst;
    public Image tickImageLast;

    public Animator transition;



    public void Awake()
    {
        gameBoard = FindObjectOfType<Board>();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        

        switch (sceneIndex)
        {
            case 1:
                totalItemsEnergy = 5;
                totalItemsTomato = 5;
                gameBoard.width = 5;
                gameBoard.height = 5;

                


                break;
            case 2:
                totalItemsEnergy = 3;
                totalItemsGun = 5;
                totalItemsPan = 2;
                gameBoard.width = 4;
                gameBoard.height = 6;
                break;
            
            case 3:
                totalItemsGun = 12;
                totalItemsArmor = 8;
                gameBoard.width = 8;
                gameBoard.height = 6;
                break;
        }



        //collectedItems = 0;
        UpdateUI();
    }

   

    public void GetTag(string tag)
    {
        if(tag == "Armor")
        {
            armorCounter++;
            UpdateUI();
        }

        else if (tag == "Energy")
        {
            energyCounter++;
            UpdateUI();
        }
        else if (tag == "Gun")
        {
            gunCounter++;
            UpdateUI();
        }
        else if (tag == "Pan")
        {
            panCounter++;
            UpdateUI();
        }
        else if (tag == "Tomato")
        {
            tomatoCounter++;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 1) {
            itemCountTextEnergy.text = $" {energyCounter} / {totalItemsEnergy}";
            itemCountTextTomato.text = $" {tomatoCounter} / {totalItemsTomato}";

            if(energyCounter >= totalItemsEnergy && tomatoCounter >= totalItemsEnergy)
            {
                LoadNextLevel();
            }
        }
        else if (sceneIndex == 2)
        {
            itemCountTextEnergy.text = $" {energyCounter} / {totalItemsEnergy}";
            itemCountTextGun.text = $" {gunCounter} / {totalItemsGun}";
            itemCountTextPan.text = $" {panCounter} / {totalItemsPan}";
            if (energyCounter >= totalItemsEnergy && gunCounter >= totalItemsGun && panCounter >= totalItemsPan)
            {
                LoadNextLevel();
            }
        }else if (sceneIndex == 3)
        {
            
            itemCountTextGun.text = $" {gunCounter} / {totalItemsGun}";
            itemCountTextArmor.text = $" {armorCounter} / {totalItemsArmor}";
            if (gunCounter >= totalItemsGun && armorCounter >= totalItemsArmor)
            {
                LoadNextLevel();
            }
        }



    }

    public void RestartLevel()
    {
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelCo(SceneManager.GetActiveScene().buildIndex+1));
    }

    IEnumerator LoadLevelCo(int level)
    {
        transition.SetTrigger("episodeOpener");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(level);
    }

}
