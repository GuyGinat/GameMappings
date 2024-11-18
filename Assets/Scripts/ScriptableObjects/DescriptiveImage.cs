using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Descriptive Image", menuName = "Images/Descriptive Image", order = 0), InlineEditor]
    public class DescriptiveImage : ScriptableObject
    {
        [HideLabel, PreviewField(Alignment = ObjectFieldAlignment.Left)] // Simple preview alignment for other uses
        public Sprite Image;
        
        [TextArea(25, 10)] // Make the text box larger, 5 lines minimum, 10 lines maximum
        public string Description;
        
        
        #if UNITY_EDITOR
        [OnInspectorGUI]
        private void DrawDynamicImagePreview()
        {
            if (Image == null)
            {
                GUILayout.Label("No image to preview.");
                return;
            }

            // Get the sprite texture dimensions
            float width = Image.texture.width;
            float height = Image.texture.height;

            // Calculate the aspect ratio
            float aspectRatio = width / height;

            // Get the current Inspector window width
            float inspectorWidth = EditorGUIUtility.currentViewWidth;

            // Apply some padding so it's not too close to the edge (optional)
            float padding = 20f;
            float maxPreviewWidth = inspectorWidth - padding;

            // Calculate the preview height based on the aspect ratio
            float previewHeight = maxPreviewWidth / aspectRatio;

            // Draw the dynamic preview
            GUILayout.Box(Image.texture, GUILayout.Width(maxPreviewWidth), GUILayout.Height(previewHeight));
        }
        #endif
    }
}