using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class HeroManager : MonoBehaviour 
{
    public enum Hero {Multiplier, Demolition, Thief, Eraser, Mystery};
    public Transform heroGroup;
    public GameObject possiblePowerUps;

    private GameObject[] boxes;
    private int toTriggerCount;

    private GameObject multiplier;
    private GameObject demolition;
    private GameObject thief;

    private Vector3 powerUpScale;
    private List<int> usedBoxes;

    private int randomBox;


	void Start () 
    {
        powerUpScale = GameObject.Find("BoxGroup").transform.localScale;

	    boxes = GameObject.FindGameObjectsWithTag("Box");
        toTriggerCount = (int) Mathf.Sqrt(boxes.Length);

        multiplier = possiblePowerUps.transform.Find("Multiplier").gameObject;
        demolition = possiblePowerUps.transform.Find("Demolition").gameObject;
        thief = possiblePowerUps.transform.Find("Thief").gameObject;

        
        randomBox = Random.Range(0, boxes.Length);
        usedBoxes = new List<int>();


        CampaignData.currentHero = Hero.Thief;

        Debug.Log("# Of PowerUps: " + toTriggerCount);

        EnableHeroButton();
	}


    void EnableHeroButton ()
    {
        foreach (Transform hero in heroGroup)
        {
            hero.gameObject.SetActive(false);
        }

        switch (CampaignData.currentHero)
        {
            case Hero.Multiplier:
                heroGroup.transform.Find("Multiplier").gameObject.SetActive(true);
                break;
            case Hero.Demolition:
                heroGroup.transform.Find("Demolition").gameObject.SetActive(true);
                break;
            case Hero.Thief:
                heroGroup.transform.Find("Thief").gameObject.SetActive(true);
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
        for (int i = 0; i < toTriggerCount - 1; i++)
        {
            int randomIndex = Random.Range(0, multiplier.transform.childCount);
            GameObject currentPower = multiplier.transform.GetChild(randomIndex).gameObject;
            

            while (usedBoxes.Contains(randomBox))
            {
                randomBox = Random.Range(0, boxes.Length);
            }
            usedBoxes.Add(randomBox);


            GameObject boxParent = boxes[randomBox];

            currentPower.transform.localScale = powerUpScale;
            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);
        }
        heroGroup.transform.Find("Multiplier").gameObject.SetActive(false);
    }


    public void UseDemolition ()
    {
        //starting at index 0 and looping thru all children for some reason only works for the 1st two children
        for (int i = toTriggerCount - 1; i > -1; i--)
        {
            GameObject currentPower = demolition.transform.GetChild(i).gameObject;
            
            while (usedBoxes.Contains(randomBox))
            {
                randomBox = Random.Range(0, boxes.Length);
            }
            usedBoxes.Add(randomBox);


            GameObject boxParent = boxes[randomBox];

            currentPower.transform.localScale = powerUpScale;
            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);
        }
        heroGroup.transform.Find("Demolition").gameObject.SetActive(false);
    }


    public void UseThief ()
    {
        for (int i = toTriggerCount - 1; i > -1; i--)
        {
            GameObject currentPower = thief.transform.GetChild(i).gameObject;
            
            while (usedBoxes.Contains(randomBox))
            {
                randomBox = Random.Range(0, boxes.Length);
            }
            usedBoxes.Add(randomBox);


            GameObject boxParent = boxes[randomBox];

            currentPower.transform.localScale = powerUpScale;
            currentPower.transform.SetParent(boxParent.transform, false);
            currentPower.SetActive(true);

            boxParent.GetComponent<Box>().SetPowerUp(currentPower);
        }
        heroGroup.transform.Find("Thief").gameObject.SetActive(false);
    }
}








