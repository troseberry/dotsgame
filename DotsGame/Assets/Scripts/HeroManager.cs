using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HeroManager : MonoBehaviour 
{
    public enum Hero {Multiplier, Demolition, Thief, Eraser, Mystery, None};
    private Transform heroGroup;
    public GameObject possiblePowerUps;

    private GameObject[] boxes;
    private int toTriggerCount;

    private List<GameObject> openBoxes;

    //in game
    private GameObject multiplier;
    private GameObject demolition;
    private GameObject thief;

    //main menu
    public Toggle multiplierToggle;
    public Toggle demolitionToggle;
    public Toggle thiefToggle;

    private List<int> usedBoxNumbers;

    private int randomBox;


	void Start () 
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            boxes = GameObject.FindGameObjectsWithTag("Box");
            toTriggerCount = (int) Mathf.Sqrt(boxes.Length);

            openBoxes = new List<GameObject>();

            multiplier = possiblePowerUps.transform.Find("Multiplier").gameObject;
            demolition = possiblePowerUps.transform.Find("Demolition").gameObject;
            thief = possiblePowerUps.transform.Find("Thief").gameObject;

            
            randomBox = Random.Range(0, boxes.Length);
            usedBoxNumbers = new List<int>();

            heroGroup = GameObject.Find("HeroGroup").transform;
            CampaignData.currentHero = Hero.None;

            EnableHeroButton();
        }
        else
        {
            ManageUnlockedHeroes();
        }
	}


    //Enables hero choice buttons if player has unlocked them
    void ManageUnlockedHeroes ()
    {
        if (!CampaignData.GetHeroBoardStatus(Hero.Multiplier))
        {
            multiplierToggle.isOn = false;
            multiplierToggle.enabled = false;
            multiplierToggle.gameObject.transform.Find("On").gameObject.SetActive(false);

            Color tempImage = multiplierToggle.gameObject.transform.Find("Off").GetComponent<Image>().color;
            tempImage.a = 0.25f;
            multiplierToggle.gameObject.transform.Find("Off").GetComponent<Image>().color = tempImage;  

            Color tempLabel = multiplierToggle.gameObject.transform.Find("Label").GetComponent<Text>().color;
            tempLabel.a = 0.5f;
            multiplierToggle.gameObject.transform.Find("Label").GetComponent<Text>().color = tempLabel; 
        }

        if (!CampaignData.GetHeroBoardStatus(Hero.Demolition))
        {
            demolitionToggle.isOn = false;
            demolitionToggle.enabled = false;
            demolitionToggle.gameObject.transform.Find("On").gameObject.SetActive(false);

            Color temp = demolitionToggle.gameObject.transform.Find("Off").GetComponent<Image>().color;
            temp.a = 0.25f;
            demolitionToggle.gameObject.transform.Find("Off").GetComponent<Image>().color = temp;  

            Color tempLabel = demolitionToggle.gameObject.transform.Find("Label").GetComponent<Text>().color;
            tempLabel.a = 0.5f;
            demolitionToggle.gameObject.transform.Find("Label").GetComponent<Text>().color = tempLabel;
        }

        if (!CampaignData.GetHeroBoardStatus(Hero.Thief))
        {
            thiefToggle.isOn = false;
            thiefToggle.enabled = false;
            thiefToggle.gameObject.transform.Find("On").gameObject.SetActive(false);

            Color temp = thiefToggle.gameObject.transform.Find("Off").GetComponent<Image>().color;
            temp.a = 0.25f;
            thiefToggle.gameObject.transform.Find("Off").GetComponent<Image>().color = temp;  

            Color tempLabel = thiefToggle.gameObject.transform.Find("Label").GetComponent<Text>().color;
            tempLabel.a = 0.5f;
            thiefToggle.gameObject.transform.Find("Label").GetComponent<Text>().color = tempLabel;
        }

    }


    //For hero buttons in the main menu
    public void ChooseHero ()
    {
        if (multiplierToggle.isOn)
        {
            CampaignData.currentHero = Hero.Multiplier;
        }
        else if (demolitionToggle.isOn)
        {
            CampaignData.currentHero = Hero.Demolition;
        }
        else if (thiefToggle.isOn)
        {
            CampaignData.currentHero = Hero.Thief;
        }
        else
        {
            CampaignData.currentHero = Hero.None;
        }

        Debug.Log("Current Hero Is: " + CampaignData.currentHero);
    }


    //For in-game hero buttons
    void EnableHeroButton ()
    {
        foreach (Transform hero in heroGroup)
        {
            hero.gameObject.SetActive(false);
        }

        if (CampaignData.currentHero != Hero.None)
        { 
            GameObject heroToUse = heroGroup.transform.Find(CampaignData.currentHero.ToString()).gameObject;
            heroToUse.SetActive(true);

           switch (CampaignData.currentHero)
            {
                case Hero.Multiplier:
                    heroToUse.GetComponent<Button>().onClick.AddListener( ()=> UseMultiplier() );
                    break;
                case Hero.Demolition:
                    heroToUse.GetComponent<Button>().onClick.AddListener( ()=> UseDemolition() );
                    break;
                case Hero.Thief:
                    heroToUse.GetComponent<Button>().onClick.AddListener( ()=> UseThief() );
                    break;
            }
        }
    }


    public void UseMultiplier ()
    {
        foreach (GameObject box in boxes)
        {
            if (!box.GetComponent<Box>().IsComplete())
            {
                openBoxes.Add(box);
            }
        }
        randomBox = Random.Range(0, openBoxes.Count);


        if (openBoxes.Count < toTriggerCount) toTriggerCount = openBoxes.Count;

        for (int i = 0; i < toTriggerCount; i++)
        {
            int randomIndex = Random.Range(0, multiplier.transform.childCount);
            GameObject currentPower = multiplier.transform.GetChild(randomIndex).gameObject;
            

            while (usedBoxNumbers.Contains(randomBox))
            {
                randomBox = Random.Range(0, openBoxes.Count);
            }
            usedBoxNumbers.Add(randomBox);


            GameObject boxParent = openBoxes[randomBox];

            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);            
        }
        heroGroup.transform.Find("Multiplier").gameObject.SetActive(false);
    }


    public void UseDemolition ()
    {
        foreach (GameObject box in boxes)
        {
            if (!box.GetComponent<Box>().IsComplete())
            {
                openBoxes.Add(box);
            }
        }
        randomBox = Random.Range(0, openBoxes.Count);


        if (openBoxes.Count < toTriggerCount) toTriggerCount = openBoxes.Count;

        for (int i = 0; i < toTriggerCount; i++)
        {
            GameObject currentPower = demolition.transform.GetChild(0).gameObject;
            
            while (usedBoxNumbers.Contains(randomBox))
            {
                randomBox = Random.Range(0, openBoxes.Count);
            }
            usedBoxNumbers.Add(randomBox);


            GameObject boxParent = openBoxes[randomBox];

            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);
        }
        heroGroup.transform.Find("Demolition").gameObject.SetActive(false);
    }


    public void UseThief ()
    {
        foreach (GameObject box in boxes)
        {
            if (!box.GetComponent<Box>().IsComplete())
            {
                openBoxes.Add(box);
            }
        }
        randomBox = Random.Range(0, openBoxes.Count);


        if (openBoxes.Count < toTriggerCount) toTriggerCount = openBoxes.Count;


        for (int i = 0; i < toTriggerCount; i++)
        {
            GameObject currentPower = thief.transform.GetChild(0).gameObject;
            
            while (usedBoxNumbers.Contains(randomBox))
            {
                randomBox = Random.Range(0, openBoxes.Count);
            }
            usedBoxNumbers.Add(randomBox);


            GameObject boxParent = openBoxes[randomBox];

            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);
        }
        heroGroup.transform.Find("Thief").gameObject.SetActive(false);
    }


    //FOR DEV OPTIONS MENU ONLY
    public void ToggleMultiplier ()
    {
        if (!CampaignData.GetHeroBoardStatus(Hero.Multiplier))
        {
            CampaignData.SetHeroBoardStatus(HeroManager.Hero.Multiplier, true);
        }
        else
        {
            CampaignData.SetHeroBoardStatus(HeroManager.Hero.Multiplier, false);
        }
        Debug.Log("Multiplier Unlocked: " + CampaignData.GetHeroBoardStatus(Hero.Multiplier));
        
        SaveLoad.Save();
        SceneManager.LoadScene(0);
    }

    public void ToggleDemolition ()
    {
        if (!CampaignData.GetHeroBoardStatus(Hero.Demolition))
        {
            CampaignData.SetHeroBoardStatus(HeroManager.Hero.Demolition, true);
        }
        else
        {
            CampaignData.SetHeroBoardStatus(HeroManager.Hero.Demolition, false);
        }
        SaveLoad.Save();
        SceneManager.LoadScene(0);
    }

    public void ToggleThief ()
    {
       if (!CampaignData.GetHeroBoardStatus(Hero.Thief))
        {
            CampaignData.SetHeroBoardStatus(HeroManager.Hero.Thief, true);
        }
        else
        {
            CampaignData.SetHeroBoardStatus(HeroManager.Hero.Thief, false);
        } 
        SaveLoad.Save();
        SceneManager.LoadScene(0);
    }
}