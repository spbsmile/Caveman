namespace Caveman.UI.Common
{
    public class LaunchGui : AdditiveScene
    {
        public override string scene
        {
            get { return "BattleGui"; }
        }

        private EnterPoint enterpoint;

        public void Awake()
        {
            // Откладываем старт битвы до загрузки UI
            enterpoint = FindObjectOfType<EnterPoint>();
            enterpoint.gameObject.SetActive(false);
        }

        protected override void AfterLoad()
        {
            enterpoint.gameObject.SetActive(true);
            base.AfterLoad();
        }
    }
}