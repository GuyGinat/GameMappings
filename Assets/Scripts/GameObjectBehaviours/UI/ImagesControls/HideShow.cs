using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjectBehaviours.UI.ImagesControls
{
    public class HideShow : MonoBehaviour
    {
    
        public GameObject gameObjectToHide;
        public Image imageToHide;

        private Color startColor;
        private void Start()
        {
            startColor = imageToHide.color;
        }

        public void Toggle()
        {
            if (gameObjectToHide != null)
            {
                gameObjectToHide.SetActive(!gameObjectToHide.activeSelf);
            }
            if (imageToHide != null)
            {
                if (imageToHide.color.a == 0f)
                {
                    Color c = startColor;
                    c.a = 1f;
                    imageToHide.color = c;
                }
                else
                    imageToHide.color = new Color(0, 0, 0, 0f);
            }
        }

    }
}
