using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Caveman.UI.Battle
{
    public class ChestIcon : MonoBehaviour
    {
        private Action actionOnTap;
        private CanvasGroup canvasGroup;

        public void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void ChestActivated(Action openHandler, bool isEnable)
        {
            canvasGroup.alpha = isEnable ? 1 : 0;
            actionOnTap = openHandler;
        }

        [UsedImplicitly]
        public void TapIcon()
        {
            if (actionOnTap != null)
            {
                canvasGroup.alpha = 0;
                actionOnTap();
            }
        }
    }
}