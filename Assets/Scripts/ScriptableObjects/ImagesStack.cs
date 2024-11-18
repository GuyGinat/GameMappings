using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Images Stack", menuName = "Images/Stack", order = 0), InlineEditor]
    public class ImagesStack : ScriptableObject
    {
        public Sprite TitleImage;
        public List<DescriptiveVideo> VideoClips = new List<DescriptiveVideo>();
        public List<LayeredImages> LayeredImages = new List<LayeredImages>();
        public List<Sprite> SingleImages = new List<Sprite>();
        public List<DescriptiveImage> DescriptiveImages = new List<DescriptiveImage>();
    }
}