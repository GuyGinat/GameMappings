using UnityEngine;
using UnityEngine.UI;

namespace GameObjectBehaviours.UI.ImagesControls
{
    public class LayerToggle : MonoBehaviour
    {

        [SerializeField] private Transform imageLayer;
        [SerializeField] private Toggle toggle;
    
        // Start is called before the first frame update
        void Start()
        {
            if (toggle == null)
            {
                toggle = GetComponent<Toggle>();
            }
            toggle.onValueChanged.AddListener((value) =>
            {
                imageLayer.gameObject.SetActive(value);
            });
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveAllListeners();
        }

        public void Init(Transform imageLayer)
        {
            this.imageLayer = imageLayer;
        }
    
    
    }
}
