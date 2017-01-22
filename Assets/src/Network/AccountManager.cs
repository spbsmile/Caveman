using UnityEngine;
using UnityEngine.UI;

namespace Caveman.Network
{
    public class AccountManager : MonoBehaviour
    {
        public const string KeyNickname = "Nickname";

        public InputField inputNickname;        
        
        public void Start()
        {
            if (PlayerPrefs.HasKey(KeyNickname))
            {
                inputNickname.text = PlayerPrefs.GetString(KeyNickname);
            }
        }

        public void SaveNickname()
        {
            PlayerPrefs.SetString(KeyNickname, inputNickname.text);
        }
    }
}
