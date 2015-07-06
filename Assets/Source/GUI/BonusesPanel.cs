using Caveman.Bonuses;
using UnityEngine;
using UnityEngine.UI;

public class BonusesPanel : MonoBehaviour
{
    public Image iconSpeedBonus;
    public Text timerBonus;

    private Image iconCurrentBonus;
    private bool timeBonus;
    private float timeLastBonusUpdate;
    private float durationBonus;


    public void Start()
    {
        timerBonus.text = "";
        iconSpeedBonus.gameObject.SetActive(false);
    }

    //todo все переделать на события 
    public void Update () 
    {
        if (timeBonus)
        {
            if (Time.timeSinceLevelLoad - timeLastBonusUpdate > 1)
            {
                timeLastBonusUpdate = Time.timeSinceLevelLoad;
                if (durationBonus - 1 > 0)
                {
                    durationBonus -= 1;
                    timerBonus.text = (durationBonus).ToString();
                }
                else
                {
                    iconCurrentBonus.gameObject.SetActive(false);
                    timerBonus.text = "";
                    timeBonus = false;
                }
            }
        }
	}

    public void BonusActivated(BonusType type, int duration)
    {
        switch (type)
        {
            case BonusType.Speed:
                {
                    iconCurrentBonus = iconSpeedBonus;
                }
                break;
        }
        iconCurrentBonus.gameObject.SetActive(true);
        timeBonus = true;
        durationBonus = duration;
    }
}
