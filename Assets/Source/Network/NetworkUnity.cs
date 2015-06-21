using UnityEngine;

namespace Caveman.Net
{
    public class NetworkUnity : MonoBehaviour
    {
        /// <summary>
        /// Кажется, есть две реазицации работы с сетью в юнити . старая и новая. про новую написали когда вышла unity5.1. 
        /// http://blogs.unity3d.com/2015/06/09/unity-5-1-is-here/
        /// прочел, как используется старая система с сетью и ,кажется, она нам не подходит, так как сервер предпологается машина на которой запущен unity.(могу ошибаться!)
        /// старая система - http://docs.unity3d.com/ru/current/Manual/net-UnityNetworkElements.html)
        /// центральная компонента - NetworkView. Любой объект который передается должен ее иметь. Эта компонента указывает, что обновлять и как. 
        /// 
        /// В новой реазизации они прям под одно пространство выделили 
        /// using UnityEngine.Networking;
        /// новая система http://docs.unity3d.com/Manual/UNetOverview.html
        /// Если не подойдет и новая реализации , то остается обычная работа с сокетами. Получать пакеты от сервера , парсить , отдавать. 
        /// Для начала, нужно , чтобы этот вариант работал хотя бы через консольное приложение.
        /// https://github.com/spbsmile/CavemanConsole
        /// </summary>
        /// 
        

        public void Start()
        {
            Network.Connect("188.166.37.212", 7175);
            if (!GetComponent<NetworkView>())
            {
                gameObject.AddComponent<NetworkView>();
            }
        }


        public int currentHealth = 0;
        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            if (stream.isWriting)
            {
                int healthC = currentHealth;
                stream.Serialize(ref healthC);
            }
            else
            {
                int healthZ = 0;
                stream.Serialize(ref healthZ);
                currentHealth = healthZ;
            }
        }
   
    }
}
