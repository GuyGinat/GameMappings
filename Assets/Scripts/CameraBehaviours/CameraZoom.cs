using DG.Tweening;
using GameObjectBehaviours.UI;
using GameObjectBehaviours.UI.Videos;
using UnityEngine;

namespace CameraBehaviours
{
    public class CameraZoom : MonoBehaviour
    {
        public float zoomSpeed = 10f;  // Speed at which to zoom
        public float minZoom = 2f;     // Minimum zoom level
        public float maxZoom = 20f;    // Maximum zoom level
        public float zoomMargin = 1.1f; // Zoom margin around sprites
        [Range(0f, 1f)] public float yOffsetScale = 0.3f; // Offset from the sprite's center

        private Camera cam;
        private VideoPlayerRenderer vpr;

        void Start()
        {
            // Get the Camera component attached to this GameObject
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            // Get scroll wheel input (positive or negative)
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (cam.orthographic)
            {
                // Zoom for orthographic cameras
                cam.orthographicSize -= scrollInput * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
            else
            {
                // Zoom for perspective cameras
                cam.fieldOfView -= scrollInput * zoomSpeed;
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
            }
        }
        
        public void MoveAndZoomToSprite(Transform image, bool animate = false)
        {
            SpriteRenderer spriteRenderer = image.GetComponent<SpriteRenderer>();
            MeshCollider meshCollider = image.GetComponent<MeshCollider>();

            if (spriteRenderer == null)
            {
                Debug.LogWarning("No SpriteRenderer found on the target image, looking for bounds instead.");
                if (meshCollider == null)
                {
                    Debug.LogError("No SpriteRenderer or MeshRenderer found on the target image.");
                    return;
                }
            }
            
            if (vpr != null)
            {
                vpr.control = false;
            }
            vpr = image.GetComponent<VideoPlayerRenderer>();
            if (vpr != null)
            {
                vpr.control = true;
            }

            Bounds spriteBounds = spriteRenderer != null ? spriteRenderer.bounds : meshCollider.bounds;

            // calculate a 30% margin around above the sprite
            float yOffset = spriteBounds.size.y * yOffsetScale;
            // Set the camera's target position to center on the sprite
            Vector3 targetCamPosition = new Vector3(spriteBounds.center.x, spriteBounds.center.y + yOffset, cam.transform.position.z);

            // Calculate the zoom level based on the sprite's bounds and the camera's aspect ratio
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float spriteAspect = spriteBounds.size.x / spriteBounds.size.y;

            float targetZoom;
            if (cam.orthographic)
            {
                if (screenAspect >= spriteAspect)
                {
                    targetZoom = spriteBounds.extents.y * zoomMargin;
                }
                else
                {
                    targetZoom = spriteBounds.extents.x / screenAspect * zoomMargin;
                }

                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

                if (animate)
                {
                    cam.transform.DOMove(targetCamPosition, 1f);
                    DOVirtual.Float(cam.orthographicSize, targetZoom, 1f, value => cam.orthographicSize = value);
                }
                else
                {
                    cam.transform.position = targetCamPosition;
                    cam.orthographicSize = targetZoom;
                }
            }
            else
            {
                Debug.LogWarning("Zoom to fit sprite is not yet implemented for perspective cameras.");
                // Implement perspective zoom-to-fit if needed
            }
        }
    }
}