using UnityEngine.EventSystems;
using UnityEngine;

namespace CameraBehaviours
{
    public class CameraMove : MonoBehaviour
    {
        private Camera cam;
        private bool isDragging;
        private Vector3 dragOrigin;
    
        // LayerMask for ignoring certain layers if necessary (e.g., UI)
        public LayerMask ignoreLayers;

    
    
        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
        }

    
        // Update is called once per frame
        void Update()
        {
        
        
            if (Input.GetMouseButtonDown(0))
            {
                // Perform a raycast check to ensure no object is being clicked
                if (!IsPointerOverObject())
                {
                    dragOrigin = Input.mousePosition;
                    isDragging = true;
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (isDragging)
                {
                    Vector3 currentMousePosition = Input.mousePosition;
                
                    // Calculate the difference in mouse positions
                    Vector3 difference = cam.ScreenToViewportPoint(dragOrigin - currentMousePosition);
                
                    // Apply this difference to the camera's position
                    Vector3 move = new Vector3(difference.x * cam.orthographicSize * 2, difference.y * cam.orthographicSize * 2, 0);
                
                    cam.transform.position += move;

                    // Update the drag origin to current mouse position
                    dragOrigin = currentMousePosition;
                }
            }

            // Stop dragging when the right mouse button is released
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    
        // Check if the mouse is clicking on any objects
        private bool IsPointerOverObject()
        {
            
            // Check if the pointer is over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the 2D raycast and check for hits, ignoring layers if specified
            if (Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, ignoreLayers))
            {
                // If an object is hit, return true (object hit, don't drag the camera)
                return true;
            }
        

            // Otherwise, return false (no object hit, we can drag the camera)
            return false;
        }
    }
}
