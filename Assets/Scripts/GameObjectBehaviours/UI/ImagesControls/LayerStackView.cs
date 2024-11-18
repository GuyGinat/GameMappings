using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjectBehaviours.UI.ImagesControls
{
    public class LayerStackView : MonoBehaviour
    {
    
        public GameObject togglePrefab;


        public void Init(Transform imageStack)
        {
            foreach (Transform child in imageStack.transform)
            {
                var toggle = Instantiate(togglePrefab, transform);
                toggle.GetComponentInChildren<TextMeshProUGUI>().text = child.name;
                LayerToggle layerToggle = toggle.GetComponent<LayerToggle>();
                layerToggle.Init(child);
            }
        }
    }
}
