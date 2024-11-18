using InputHandlers;
using UnityEngine;

namespace GameObjectBehaviours
{
    public class WorldImage : MonoBehaviour
    {
        private bool isClicked;

        private Vector3 clickOffset;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private Vector2 normalizedClickPosition;
        [SerializeField] private float edgeThreshold = 0.1f;
        private Camera cam;

        bool isLocked = false;
        [SerializeField] private Lock lockScript;
        
        
        bool leftSideClicked = false;
        bool rightSideClicked = false;
        bool topSideClicked = false;
        bool bottomSideClicked = false;
        private bool ClickedOnlyOneEdge =>  
            (leftSideClicked ? 1 : 0) +
            (rightSideClicked ? 1 : 0) +
            (topSideClicked ? 1 : 0) +
            (bottomSideClicked ? 1 : 0) == 1;
            

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            cam = Camera.main;
        }

        public void Init(Lock lockScript)
        {
            print("World image lock script: " + lockScript.name);
            this.lockScript = lockScript;
        }
        
        private void OnMouseDrag()
        {
            if (isClicked && !ClickedOnlyOneEdge)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 curPosition = cam.ScreenToWorldPoint(curScreenPoint) + clickOffset;
                transform.position = curPosition;
            }
        }

        private void EvaluateClickPosition()
        {
            leftSideClicked = normalizedClickPosition.x < edgeThreshold;
            rightSideClicked = normalizedClickPosition.x > 1 - edgeThreshold;
            topSideClicked = normalizedClickPosition.y > 1 - edgeThreshold;
            bottomSideClicked = normalizedClickPosition.y < edgeThreshold;
        }

        private void OnMouseDown()
        {
            if (lockScript == null)
            {
                lockScript = GetComponentInChildren<Lock>();
            }
            if (lockScript != null && lockScript.IsLocked)
            {
                return;
            }
            clickOffset = transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        
            // Get the position on the sprite where it was clicked
            normalizedClickPosition = GetNormalizedClickPosition();
            EvaluateClickPosition();
            
            if (InputHandler.Instance.HasOneConnector)
            {
                
            }
            
            // clicking on an edge should either create
            // a connector or connect one to the first one
            if (ClickedOnlyOneEdge)
            {
                
            }
        
            isClicked = true;
        }
    
        private void OnMouseUp()
        {
            
            ResetClickedPositions();
            isClicked = false;
        }

        private void ResetClickedPositions()
        {
            leftSideClicked = false;
            rightSideClicked = false;
            topSideClicked = false;
            bottomSideClicked = false;
        }


        private Vector2 GetNormalizedClickPosition()
        {
            // Get the world position of the click
            Vector3 worldPosition = cam.ScreenToWorldPoint(Input.mousePosition);

            // Convert world position to local space of the sprite
            Vector3 localPosition = transform.InverseTransformPoint(worldPosition);

            // Get the size of the sprite in local units
            Bounds spriteBounds = spriteRenderer.sprite.bounds;
            float localX = localPosition.x - spriteBounds.min.x;
            float localY = localPosition.y - spriteBounds.max.y; // Adjust for the top-left origin

            // Normalize the coordinates between 0 and 1
            float normalizedX = localX / spriteBounds.size.x;
            float normalizedY = Mathf.Abs(localY / spriteBounds.size.y); // Adjust the Y axis direction

            // Return the normalized position
            return new Vector2(normalizedX, normalizedY);
        }
    }
}
