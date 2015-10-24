using Caveman.Specification;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI.Battle
{
    public class BonusesPanel : MonoBehaviour
    {
        public Image iconSpeedBonus;
        public Text timerBonus;

        private Image iconCurrentBonus;
        private float timeLastBonusUpdate;
        private float durationBonus;
        private CanvasGroup canvasGroup;

        public void Start()
        {
            timerBonus.text = "";
            iconSpeedBonus.gameObject.SetActive(false);
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void BonusActivated(BonusSpecification.Types type, float duration)
        {
            switch (type)
            {
                case BonusSpecification.Types.Speed:
                {
                    iconCurrentBonus = iconSpeedBonus;
                }
                    break;
            }
            StopCoroutine(BonusTime());
            iconCurrentBonus.gameObject.SetActive(true);
            canvasGroup.alpha = 1;
            durationBonus = duration;
            timerBonus.text = durationBonus.ToString();
            StartCoroutine(BonusTime());
        }

        private IEnumerator BonusTime()
        {
            while (durationBonus > 0)
            {
                yield return new WaitForSeconds(1);
                durationBonus -= 1;
                timerBonus.text = durationBonus.ToString();
            }

            // Скрываем панель бонуса.
            iconCurrentBonus.gameObject.SetActive(false);
            timerBonus.text = "";
            canvasGroup.alpha = 0;
        }
    }
}
