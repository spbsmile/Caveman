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
            iconSpeedBonus.gameObject.SetActive(false);
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void BonusActivated(BonusSpecification.Types type, float duration)
        {
            switch (type)
            {
                case BonusSpecification.Types.Speed:
                    iconCurrentBonus = iconSpeedBonus;
                    break;
            }

            StopAllCoroutines();
            StartCoroutine(BonusTimer(duration));
        }

        private IEnumerator BonusTimer(float duration)
        {
            StartCoroutine(BonusPanelShow());

            durationBonus = duration - 0.5f;
            timerBonus.text = duration.ToString();
            while (durationBonus >= 0)
            {
                yield return new WaitForSeconds(0.5f);
                durationBonus -= 0.5f;
                timerBonus.text = Mathf.Ceil(durationBonus).ToString();
            }

            StartCoroutine(BonusPanelHide());
        }

        private IEnumerator BonusPanelShow()
        {
            iconCurrentBonus.gameObject.SetActive(true);
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += 0.2f;
                yield return new WaitForSeconds(0.01f);
            }
        }

        private IEnumerator BonusPanelHide()
        {
            StopCoroutine(BonusPanelShow());
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= 0.2f;
                yield return new WaitForSeconds(0.01f);
            }
            iconCurrentBonus.gameObject.SetActive(false);
        }
    }
}
