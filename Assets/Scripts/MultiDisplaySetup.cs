using UnityEngine;

public class MultiDisplaySetup : MonoBehaviour
{
    void Start()
    {
        // Check if there is more than one display
        if (Display.displays.Length > 1)
        {
            // Activate Display 2
            Display.displays[1].Activate();
        }

        // If you have more displays, activate them similarly
        // if (Display.displays.Length > 2)
        // {
        //     Display.displays[2].Activate();
        // }
    }
}