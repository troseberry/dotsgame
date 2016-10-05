using UnityEngine;
using System.Collections;

public class FirstOrSecond : MonoBehaviour 
{
    public void Yes ()
    {
        this.gameObject.SetActive(false);
    }

    public void No ()
    {
        Invoke("DelaySetPlayerTurn", 0.5f);
        this.gameObject.SetActive(false);
    }

    public void DelaySetPlayerTurn ()
    {
        CampaignGameManager.Instance.isPlayerTurn = false;
    }
}
