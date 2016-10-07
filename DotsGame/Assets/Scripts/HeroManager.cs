using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HeroManager : MonoBehaviour 
{
    public enum Hero {Multiplier, Demolition, Thief, Eraser, Mystery};
    private Transform heroGroup;
    public GameObject possiblePowerUps;

    private GameObject[] boxes;
    private int toTriggerCount;

    private GameObject multiplier;
    private GameObject demolition;
    private GameObject thief;

    //private Vector3 powerUpScale;
    private List<int> usedBoxes;

    private int randomBox;


	void Start () 
    {
       
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //powerUpScale = GameObject.Find("BoxGroup").transform.localScale;

    	    boxes = GameObject.FindGameObjectsWithTag("Box");
            toTriggerCount = (int) Mathf.Sqrt(boxes.Length);

            multiplier = possiblePowerUps.transform.Find("Multiplier").gameObject;
            demolition = possiblePowerUps.transform.Find("Demolition").gameObject;
            thief = possiblePowerUps.transform.Find("Thief").gameObject;

            
            randomBox = Random.Range(0, boxes.Length);
            usedBoxes = new List<int>();

            heroGroup = GameObject.Find("HeroGroup").transform;
            //CampaignData.currentHero = Hero.Thief;
            //Debug.Log("Current Hero: " + CampaignData.currentHero);
            //Debug.Log("# Of PowerUps: " + toTriggerCount);

            EnableHeroButton();
        }
	}


    void EnableHeroButton ()
    {
        foreach (Transform hero in heroGroup)
        {
            hero.gameObject.SetActive(false);
        }

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


    public void ChooseHero ()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        string hero = buttonName.Substring(0, buttonName.Length - 6);           //minus "Button" in the string

        if (hero == Hero.Multiplier.ToString())
        {
            CampaignData.currentHero = Hero.Multiplier;
        }
    }

    public void UseMultiplier ()
    {
        for (int i = 0; i < toTriggerCount; i++)
        {
            int randomIndex = Random.Range(0, multiplier.transform.childCount);
            GameObject currentPower = multiplier.transform.GetChild(randomIndex).gameObject;
            

            while (usedBoxes.Contains(randomBox))
            {
                randomBox = Random.Range(0, boxes.Length);
            }
            usedBoxes.Add(randomBox);


            GameObject boxParent = boxes[randomBox];

            //currentPower.transform.localScale = powerUpScale;
            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);            
        }
        heroGroup.transform.Find("Multiplier").gameObject.SetActive(false);
    }


    public void UseDemolition ()
    {
        //for some reason, using a foreach only works for the 1st two children
        //Debug.Log("# of Bombs: " + demolition.transform.childCount);
        //foreach (Transform child in demolition.transform)
        for (int i = 0; i < toTriggerCount; i++)
        {
            GameObject currentPower = demolition.transform.GetChild(0).gameObject;
            //GameObject currentPower = child.gameObject;
            
            while (usedBoxes.Contains(randomBox))
            {
                randomBox = Random.Range(0, boxes.Length);
            }
            usedBoxes.Add(randomBox);


            GameObject boxParent = boxes[randomBox];

            //currentPower.transform.localScale = powerUpScale;
            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);
        }
        heroGroup.transform.Find("Demolition").gameObject.SetActive(false);
    }


    public void UseThief ()
    {
        for (int i = 0; i < toTriggerCount; i++)
        {
            GameObject currentPower = thief.transform.GetChild(0).gameObject;
            
            while (usedBoxes.Contains(randomBox))
            {
                randomBox = Random.Range(0, boxes.Length);
            }
            usedBoxes.Add(randomBox);


            GameObject boxParent = boxes[randomBox];

            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);
        }
        heroGroup.transform.Find("Thief").gameObject.SetActive(false);
    }
}








