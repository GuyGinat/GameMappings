using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsManager : MonoBehaviour
{
    
    public static UISettingsManager Instance;
    
    public bool SnapToImage = true;
    public bool ResizeToImage = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }    
    }
    
    public void ToggleSnapToImage(bool value)
    {
        SnapToImage = value;
    }
    
    
    public void ToggleResizeToImage(bool value)
    {
        ResizeToImage = value;
    }

    
}
