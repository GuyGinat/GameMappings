using GameObjectBehaviours;
using StandaloneFileBrowser;
using UnityEngine;
using UnityEngine.UI;

// Standalone File Browser

namespace SystemHandlers
{
    public class ImageLoader : MonoBehaviour
    {
        public RawImage imageDisplay;  // UI element to display the loaded image

        public GameObject WorldGameObject;
        public GameObject LockPrefab;

        public void OpenFileDialog()
        {
            // Opens a file dialog and returns the file path selected by the user
            var extensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
                new ExtensionFilter("All Files", "*" )
            };

            string[] paths = StandaloneFileBrowser.StandaloneFileBrowser.OpenFilePanel("Open Image", "", extensions, false);
        
            if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                LoadImageAsSprite(paths[0]);
            }
            else
            {
                Debug.Log("No file selected.");
            }
        }

        void LoadImageAsSprite(string filePath)
        {
            GameObject go = Instantiate(WorldGameObject, Vector3.zero, Quaternion.identity);
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            
            GameObject l = Instantiate(LockPrefab, go.transform);
            Lock lockScript = l.GetComponent<Lock>();
            print("found lock script: " + lockScript != null ? "true" : "false");
            // lockScript.transform.parent = transform;
            lockScript.Position(new Vector3(0.45f, 0.45f, 0.0f));
            
        
            // Load the image as a Texture2D
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            // Convert the Texture2D to a Sprite
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite newSprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f)); // Center pivot
        
            // readjust the 2dbox collider to the new sprite size
            BoxCollider2D boxCollider = go.GetComponent<BoxCollider2D>();
            boxCollider.size = newSprite.bounds.size;

            // Assign the sprite to the UI Image component
            if (sr != null)
            {
                sr.sprite = newSprite;

                // Maintain the aspect ratio of the sprite
                // MaintainAspectRatio(texture.width, texture.height);
            }

            var wi = WorldGameObject.GetComponent<WorldImage>();
            wi.Init(lockScript);
        }
    
    
        void MaintainAspectRatio(int width, int height)
        {
            // Get the aspect ratio of the image
            float aspectRatio = (float)width / height;

            // Resize the Image UI element to maintain the aspect ratio
            RectTransform rt = imageDisplay.GetComponent<RectTransform>();

            if (rt != null)
            {
                // Use the parent width and adjust the height accordingly to maintain the aspect ratio
                float parentWidth = rt.parent.GetComponent<RectTransform>().rect.width;
                rt.sizeDelta = new Vector2(parentWidth, parentWidth / aspectRatio);
            }
        }
    
    
        void LoadImage(string filePath)
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            if (imageDisplay != null)
            {
                imageDisplay.texture = texture;
            }
        }
    }
}
