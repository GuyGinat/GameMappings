using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Descriptive Video", menuName = "Images/Descriptive Video", order = 0)]
    public class DescriptiveVideo : ScriptableObject
    {
        [HideLabel, PreviewField(Alignment = ObjectFieldAlignment.Left)] // Simple preview alignment for other uses
        public VideoClip VideoClip; 
        
        public List<TimedVideoDescription> Descriptions;
    }
    
    [Serializable]
    public class TimedVideoDescription
    {
        [TextArea(25, 10)] // Make the text box larger, 5 lines minimum, 10 lines maximum
        public string Description;
        public float TimeStamp;
    }
}