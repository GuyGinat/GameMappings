using Systems;
using TMPro;
using UnityEngine;

namespace GameObjectBehaviours.UI
{
    public class SecondCameraCanvasHandler : MonoBehaviourSingleton<SecondCameraCanvasHandler>
    {
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        public void SetDescription(string description)
        {
            descriptionText.text = description;
        }
    }
}