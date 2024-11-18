using CameraBehaviours;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameObjectBehaviours.UI.ImagesControls
{
    public class ImageTabButton : MonoBehaviour
    {
        public TextMeshProUGUI buttonText;
        [SerializeField] private Transform image;
        
        [SerializeField] private GameObject layersControl;
        [SerializeField] private CanvasGroup canvasGroup;
        
        [SerializeField] private bool hasSecondaryCameraDescription;
        [SerializeField] private string secondaryCameraDescription;
    
        public void Init(Transform image, string text, GameObject layersControl = null, CanvasGroup canvasGroup = null, bool hasSecondaryCameraDescription = false, string secondaryCameraDescription = "")
        {
            this.image = image;
            buttonText.text = text;
            this.layersControl = layersControl;
            this.canvasGroup = canvasGroup;
            this.hasSecondaryCameraDescription = hasSecondaryCameraDescription;
            this.secondaryCameraDescription = secondaryCameraDescription;
        }
        
    
        public void MoveToImage()
        {
            Camera cam = Camera.main;

            CameraZoom cameraZoom = cam.GetComponent<CameraZoom>();
            
            // Example of invoking the method when you click a button
            cameraZoom.MoveAndZoomToSprite(image, !UISettingsManager.Instance.SnapToImage);

            // if (UISettingsManager.Instance.SnapToImage)
            // {
            //     cam.transform.position = image.position - Vector3.forward * 10f;
            // }
            // else
            // {
            //     print("Moving to image with animation");
            //     cam.transform.DOMove(image.position - Vector3.forward * 10f, 1f);
            // }
            if (layersControl != null)
            {
                layersControl.SetActive(true);
            }
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            if (hasSecondaryCameraDescription)
            {
                SecondCameraCanvasHandler.Instance.SetDescription(secondaryCameraDescription);
            }
            ImageGridCreator.Instance.HideAllLayersControlsExcluding(this);
            ImageGridCreator.Instance.HideAllVideoControlsExcluding(this);
        }
        
        public void HideLayersControl()
        {
            if (layersControl != null)
            {
                layersControl.SetActive(false);
            }
        }
        
        public void HideCanvasGroup()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
   
    }
}
