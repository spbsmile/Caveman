using UnityEngine;

namespace Caveman.UI
{
    public class CommonScene : MonoBehaviour
    {
        public void Start()
        {
            Application.LoadLevelAdditiveAsync("Common");
        }
    }
}
