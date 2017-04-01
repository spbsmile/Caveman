using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Caveman.UI
{
    public class Profile : MonoBehaviour
    {
        [UsedImplicitly]
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}


