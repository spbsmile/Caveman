using UnityEngine;
using System.Collections;
using System.Threading;

public class ButtonTest : MonoBehaviour
{
    public CNButton Button;

    // Use this for initialization
    void Start()
    {
        Button.FingerTouchedEvent += controller => Debug.Log("Button pressed");
        Button.FingerLiftedEvent += controller => Debug.Log("Button released");
        Button.ControllerMovedEvent += (vector3, controller) => Debug.Log("Button is being pressed");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
