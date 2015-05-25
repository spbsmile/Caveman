using UnityEngine;

namespace Caveman.GUI
{
    public class CommonScene : MonoBehaviour
    {

        public void Start()
        {
            Application.LoadLevelAdditiveAsync("Common");
        }
    }
}
