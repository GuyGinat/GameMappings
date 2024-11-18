
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableObjects
{
    [UnityEngine.CreateAssetMenu(fileName = "Layered Images", menuName = "Images/Layered", order = 0), InlineEditor]
    public class LayeredImages : UnityEngine.ScriptableObject
    {
        public List<Sprite> images = new List<Sprite>();
        // public List<LayeredPairingDescription> descriptions = new List<LayeredPairingDescription>();
        [TextArea(15, 10)]
        public string Description;
    }
}