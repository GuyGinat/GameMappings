using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using ScriptableObjects;

public class DescriptiveImageGenerator : EditorWindow
{
    private string folderPath = "Assets/Textures"; // Default folder path for the textures
    private string outputFolderPath = "Assets/ScriptableObjects"; // Folder to save the ScriptableObjects

    [MenuItem("Tools/Generate Descriptive Images")]
    public static void ShowWindow()
    {
        GetWindow<DescriptiveImageGenerator>("Descriptive Image Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Descriptive Image Generator", EditorStyles.boldLabel);

        // Display the selected texture folder path
        GUILayout.Label("Selected Texture Folder: " + folderPath);

        // Button to select the texture folder
        if (GUILayout.Button("Select Texture Folder"))
        {
            folderPath = EditorUtility.OpenFolderPanel("Select Texture Folder", folderPath, "");
            // Ensure the path starts with "Assets" (to work within Unity's asset system)
            if (folderPath.StartsWith(Application.dataPath))
            {
                folderPath = "Assets" + folderPath.Substring(Application.dataPath.Length);
            }
        }

        // Display the output folder path
        GUILayout.Label("Selected Output Folder: " + outputFolderPath);

        // Button to select the output folder
        if (GUILayout.Button("Select Output Folder"))
        {
            outputFolderPath = EditorUtility.OpenFolderPanel("Select Output Folder", outputFolderPath, "");
            if (outputFolderPath.StartsWith(Application.dataPath))
            {
                outputFolderPath = "Assets" + outputFolderPath.Substring(Application.dataPath.Length);
            }
        }

        // Button to generate the Descriptive Images
        if (GUILayout.Button("Generate Descriptive Images"))
        {
            if (!string.IsNullOrEmpty(folderPath) && !string.IsNullOrEmpty(outputFolderPath))
            {
                GenerateDescriptiveImages();
            }
            else
            {
                Debug.LogError("Please select valid folders for textures and output.");
            }
        }
    }

    private void GenerateDescriptiveImages()
    {
        // Ensure the output folder exists
        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }

        // Get all the texture files in the folder (png, jpg, jpeg)
        string[] pngPaths = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);
        string[] jpgPaths = Directory.GetFiles(folderPath, "*.jpg", SearchOption.AllDirectories);
        string[] jpegPaths = Directory.GetFiles(folderPath, "*.jpeg", SearchOption.AllDirectories);

        // Combine all file paths into one array
        string[] texturePaths = pngPaths.Concat(jpgPaths).Concat(jpegPaths).ToArray();

        if (texturePaths.Length == 0)
        {
            Debug.LogWarning("No textures found in the folder: " + folderPath);
            return;
        }

        foreach (string texturePath in texturePaths)
        {
            Debug.Log("Processing Texture: " + texturePath);

            // Convert the full system path to a Unity asset-relative path
            string relativePath = texturePath.Replace(Application.dataPath, "").Replace('\\', '/');
            Debug.Log("Attempting to load sprite at: " + relativePath);

            // Load the Sprite asset instead of Texture2D
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);

            if (sprite == null)
            {
                Debug.LogError("Failed to load sprite at path: " + relativePath);
                continue;
            }

            // Create a new DescriptiveImage ScriptableObject
            DescriptiveImage descriptiveImage = ScriptableObject.CreateInstance<DescriptiveImage>();
            descriptiveImage.Image = sprite; // Assign the sprite directly

            descriptiveImage.Description = "Description for " + sprite.name;

            // Save the ScriptableObject to the output folder
            string outputPath = Path.Combine(outputFolderPath, sprite.name + ".asset");
            AssetDatabase.CreateAsset(descriptiveImage, outputPath);
        }

        // Save all changes to the assets
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Descriptive Images generated successfully.");
    }

    /*private void GenerateDescriptiveImages()
    {
        // Ensure the output folder exists
        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }

        // Get all the texture files in the folder
        // Get all the texture files in the folder (png, jpg, jpeg)
        string[] pngPaths = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);
        string[] jpgPaths = Directory.GetFiles(folderPath, "*.jpg", SearchOption.AllDirectories);
        string[] jpegPaths = Directory.GetFiles(folderPath, "*.jpeg", SearchOption.AllDirectories);

        string[] texturePaths = pngPaths.Concat(jpgPaths).Concat(jpegPaths).ToArray();

        if (texturePaths.Length == 0)
        {
            Debug.LogWarning("No textures found in the folder: " + folderPath);
            return;
        }

        foreach (string texturePath in texturePaths)
        {
            Debug.Log("Processing Texture: " + texturePath);

            // Load the Texture2D asset
            string relativePath = "Assets" + texturePath.Replace(Application.dataPath, "").Replace('\\', '/');
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(relativePath);

            if (texture != null)
            {
                Debug.Log("Generating Descriptive Image for: " + texture.name);
                // Create a new DescriptiveImage ScriptableObject
                DescriptiveImage descriptiveImage = ScriptableObject.CreateInstance<DescriptiveImage>();
                descriptiveImage.Image = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));

                descriptiveImage.Description = "Description for " + texture.name;

                // Save the ScriptableObject to the output folder
                string outputPath = Path.Combine(outputFolderPath, texture.name + ".asset");
                AssetDatabase.CreateAsset(descriptiveImage, outputPath);
            }
        }

        // Save all changes to the assets
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Descriptive Images generated successfully.");
    }*/
}