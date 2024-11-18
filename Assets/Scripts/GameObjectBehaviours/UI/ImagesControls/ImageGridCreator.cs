using System;
using System.Collections.Generic;
using ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using GameObjectBehaviours.UI.Videos;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameObjectBehaviours.UI.ImagesControls
{
    public class ImageGridCreator : MonoBehaviour
    {
        public static ImageGridCreator Instance;

        [Header("Image Stack"), PropertySpace(SpaceAfter = 15)]
        public ImagesStack images;

        [TabGroup("Startup Settings")] public CanvasGroup canvasGroup;

        [TabGroup("Startup Settings"), Range(0f, 1f)]
        public float UIAlpha = 0.5f;

        [TabGroup("Grid Settings")] public int columns = 1;
        [TabGroup("Grid Settings")] public float xGapInPixels;
        [TabGroup("Grid Settings")] public float yGapInPixels;
        [TabGroup("Grid Settings")] public float xGapInUnits;
        [TabGroup("Grid Settings")] public float yGapInUnits;

        [TabGroup("Grid Settings"), Range(0f, 1f)]
        public float descriptiveImagesTopMarginPercentage;

        [TabGroup("Grid Settings")] public TMP_FontAsset fontAsset;
        [TabGroup("Grid Settings")] public float fontSize;
        [TabGroup("Grid Settings")] public bool useWorldSpaceCanvasForDescriptions;
        [TabGroup("Grid Settings")] public bool useSecondaryCameraForDescriptions;

        // [TabGroup("Grid Settings")] public LayerMask layer;


        [Tooltip("If true, gaps will be in pixels, otherwise in units.")] [TabGroup("Grid Settings")]
        public bool usePixels;

        private float xGap;
        private float yGap;


        [TabGroup("Generated Objects")] public List<GameObject> generatedImages;
        [TabGroup("Generated Objects")] public List<GameObject> generatedVideos;
        [TabGroup("Generated Objects")] public List<GameObject> generatedTabButtons;
        [TabGroup("Generated Objects")] public List<GameObject> generatedLayeredControls;
        [TabGroup("Generated Objects")] public List<ImageTabButton> imageTabButtons;

        [TabGroup("References & Prefabs")] [Title("Tab Buttons")]
        public Transform tabButtonsParent;

        [TabGroup("References & Prefabs")] public GameObject imageTabButtonPrefab;
        [TabGroup("References & Prefabs")] public GameObject layersControlPrefab;
        [TabGroup("References & Prefabs")] public GameObject videoPlayerPrefab;
        [TabGroup("References & Prefabs")] public Toggle snapToggle;


        private float globalXOffset;
        private float globalYOffset;

        private Dictionary<int, ImageTabButton> imageButtonsDictionary;
        [SerializeField] private int currentImageIndex = 0;

        #region Unity Events

        private void Start()
        {
            Instance = this;
            canvasGroup.alpha = UIAlpha;

            imageButtonsDictionary = new Dictionary<int, ImageTabButton>();
            int i = 0;

            foreach (ImageTabButton imageTabButton in imageTabButtons)
            {
                imageButtonsDictionary.Add(i, imageTabButton);
                i++;
            }
        }

        private void Update()
        {
            HandleToggleCanvasInput();
            HandleToggleSnapToImageInput();
            HandleMoveToNextImageInput();
        }

        #endregion


        #region Input Handling

        private void HandleToggleSnapToImageInput()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                snapToggle.isOn = !snapToggle.isOn;
            }
        }

        private void HandleToggleCanvasInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleCanvas();
            }
        }

        private void HandleMoveToNextImageInput()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                print("Right Arrow");
                if (currentImageIndex < imageButtonsDictionary.Count - 1)
                {
                    currentImageIndex++;
                }
                else
                {
                    currentImageIndex = 0;
                }

                imageButtonsDictionary[currentImageIndex].MoveToImage();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                print("Left Arrow");
                if (currentImageIndex > 0)
                {
                    currentImageIndex--;
                }
                else
                {
                    currentImageIndex = imageButtonsDictionary.Count - 1;
                }

                imageButtonsDictionary[currentImageIndex].MoveToImage();
            }
        }

        #endregion


        #region Grid Generation

        [Button]
        public void DestroyGrid()
        {
            foreach (var image in generatedImages)
            {
                DestroyImmediate(image);
            }

            generatedImages.Clear();

            foreach (var tabButton in generatedTabButtons)
            {
                DestroyImmediate(tabButton);
            }

            generatedTabButtons.Clear();

            foreach (var imageTabButton in imageTabButtons)
            {
                DestroyImmediate(imageTabButton);
            }

            imageTabButtons.Clear();

            foreach (var layersControl in generatedLayeredControls)
            {
                DestroyImmediate(layersControl);
            }

            generatedLayeredControls.Clear();

            foreach (var video in generatedVideos)
            {
                DestroyImmediate(video);
            }

            generatedVideos.Clear();
        }


        [Button]
        public void CreateGrid(float xPosition, float yPosition)
        {
            float xOffset = xPosition;
            float yOffset = yPosition;

            xGap = usePixels ? xGapInPixels / 100f : xGapInUnits;
            yGap = usePixels ? yGapInPixels / 100f : yGapInUnits;


            GenerateDescriptiveImages(ref xOffset, ref yOffset);
            GenerateVideoClips(ref xOffset, ref yOffset);
            GenerateSingleImages(ref xOffset, ref yOffset);
            GenerateLayeredImages(ref xOffset, ref yOffset);
        }


        private void GenerateSingleImages(ref float xOffset, ref float yOffset)
        {
            for (int i = 0; i < images.SingleImages.Count; i++)
            {
                var img = images.SingleImages[i];
                Vector3 imageSize = GetImageSizeInUnits(img);

                if (i % columns == 0 && i != 0)
                {
                    xOffset = 0;
                    yOffset += imageSize.y + yGap;
                }

                float xPos = globalXOffset + xOffset;
                float yPos = globalYOffset + yOffset;

                GameObject singleImage = GenerateSingleImage(img, new Vector3(xPos, yPos, 0));
                xOffset += imageSize.x + xGap;

                GameObject tabButton = Instantiate(imageTabButtonPrefab, tabButtonsParent);
                var tabButtonComponent = tabButton.GetComponent<ImageTabButton>();
                tabButtonComponent.Init(singleImage.transform, singleImage.name, null);

                imageTabButtons.Add(tabButtonComponent);
                generatedTabButtons.Add(tabButton);
            }
        }

        private void GenerateLayeredImages(ref float xOffset, ref float yOffset)
        {
            for (int i = 0; i < images.LayeredImages.Count; i++)
            {
                Sprite firstImage = images.LayeredImages[i].images[0];
                Vector3 imageSize = GetImageSizeInUnits(firstImage);

                if (i % columns == 0 && i != 0)
                {
                    xOffset = 0;
                    yOffset += imageSize.y + yGap;
                }

                float xPos = globalXOffset + xOffset;
                float yPos = globalYOffset + yOffset;

                GameObject layeredImages = GenerateLayeredImages(images.LayeredImages[i], new Vector3(xPos, yPos, 0));
                xOffset += imageSize.x + xGap;

                GameObject layersControl = null;
                if (layeredImages.transform.childCount > 1)
                {
                    layersControl = GenerateLayersControl(layeredImages.transform);
                    generatedLayeredControls.Add(layersControl);
                }

                GameObject tabButton = Instantiate(imageTabButtonPrefab, tabButtonsParent);
                var tabButtonComponent = tabButton.GetComponent<ImageTabButton>();
                tabButtonComponent.Init
                    (
                        layeredImages.transform.GetChild(0), 
                        layeredImages.name, 
                        layersControl,
                        hasSecondaryCameraDescription: useSecondaryCameraForDescriptions, 
                        secondaryCameraDescription: images.LayeredImages[i].Description   
                    );

                imageTabButtons.Add(tabButtonComponent);
                generatedTabButtons.Add(tabButton);
            }
        }

        private void GenerateVideoClips(ref float xOffset, ref float yOffset)
        {
            for (int i = 0; i < images.VideoClips.Count; i++)
            {
                DescriptiveVideo dv = images.VideoClips[i];
                VideoClip vc = dv.VideoClip;

                if (i % columns == 0 && i != 0)
                {
                    xOffset = 0;
                    yOffset += 18 + yGap;
                }

                float xPos = globalXOffset + xOffset;
                float yPos = globalYOffset + yOffset;

                GameObject videoPlayer = GenerateVideoPlayer(vc, new Vector3(xPos, yPos, 0), dv.Descriptions);
                xOffset += 32 + xGap;

                GameObject tabButton = Instantiate(imageTabButtonPrefab, tabButtonsParent);
                var tabButtonComponent = tabButton.GetComponent<ImageTabButton>();
                
                tabButtonComponent.Init(
                        videoPlayer.transform,
                        vc.name,
                        canvasGroup: videoPlayer.GetComponentInChildren<CanvasGroup>(),
                        hasSecondaryCameraDescription: useSecondaryCameraForDescriptions,
                        secondaryCameraDescription: dv.Descriptions[0].Description
                    );

                imageTabButtons.Add(tabButtonComponent);
                generatedTabButtons.Add(tabButton);
                generatedVideos.Add(videoPlayer);
            }
        }

        private void GenerateDescriptiveImages(ref float xOffset, ref float yOffset)
        {
            for (int i = 0; i < images.DescriptiveImages.Count; i++)
            {
                DescriptiveImage di = images.DescriptiveImages[i];
                Vector3 imageSize = GetImageSizeInUnits(di.Image);

                if (i % columns == 0 && i != 0)
                {
                    xOffset = 0;
                    yOffset += imageSize.y + yGap;
                }

                float xPos = globalXOffset + xOffset;
                float yPos = globalYOffset + yOffset;

                GameObject singleImage = GenerateSingleImage(di.Image, new Vector3(xPos, yPos, 0));

                if (useWorldSpaceCanvasForDescriptions)
                {
                    singleImage.AddComponent<WorldImage>();
                    singleImage.AddComponent<BoxCollider2D>();
                    singleImage.layer = LayerMask.NameToLayer("WorldElements");
                    SpriteRenderer sr = singleImage.GetComponent<SpriteRenderer>();
                    var canvas = GenerateWorldSpaceCanvas(sr);
                    AddUIDescription(canvas, di.Description);
                }

                xOffset += imageSize.x + xGap;

                GameObject tabButton = Instantiate(imageTabButtonPrefab, tabButtonsParent);
                var tabButtonComponent = tabButton.GetComponent<ImageTabButton>();
                tabButtonComponent.Init
                (
                    image: singleImage.transform,
                    text: singleImage.name,
                    layersControl: null,
                    hasSecondaryCameraDescription: useSecondaryCameraForDescriptions,
                    secondaryCameraDescription: di.Description
                );

                imageTabButtons.Add(tabButtonComponent);
                generatedTabButtons.Add(tabButton);
            }
        }

        [Button, TabGroup("Functions")]
        public GameObject GenerateLayeredImages(LayeredImages layeredImages, Vector3 position)
        {
            if (layeredImages == null)
            {
                Debug.LogError("Layered Images is null");
                return null;
            }

            GameObject imageStack = new GameObject("Image Stack");
            imageStack.name = layeredImages.images[0].name;

            int i = 2;
            foreach (Sprite img in layeredImages.images)
            {
                GameObject image = new GameObject("Image");
                var sr = image.AddComponent<SpriteRenderer>();
                sr.sprite = img;
                sr.sortingOrder = i++;
                image.name = img.name;
                image.transform.position = position;
                image.transform.parent = imageStack.transform;
            }

            generatedImages.Add(imageStack);
            return imageStack;
        }


        [Button, TabGroup("Functions")]
        public GameObject GenerateSingleImage(Sprite img, Vector3 position)
        {
            GameObject image = new GameObject("Image");
            var sr = image.AddComponent<SpriteRenderer>();
            sr.sprite = img;
            image.name = img.name;
            image.transform.position = position;
            generatedImages.Add(image);
            return image;
        }

        [Button, TabGroup("Functions")]
        public GameObject GenerateVideoPlayer(VideoClip clip, Vector3 position,
            List<TimedVideoDescription> descriptions)
        {
            GameObject videoPlayer = Instantiate(videoPlayerPrefab, position, Quaternion.identity);
            videoPlayer.transform.localScale = new Vector3(32f, 18f, 1f);
            var vpr = videoPlayer.GetComponent<VideoPlayerRenderer>();
            vpr.Init(clip, descriptions);
            vpr.CreateVideoPlayer();
            return videoPlayer;
        }

        private GameObject GenerateLayersControl(Transform imageStack)
        {
            GameObject layersControl = Instantiate(layersControlPrefab, tabButtonsParent.parent);
            layersControl.GetComponent<LayerStackView>().Init(imageStack);
            return layersControl;
        }


        private Canvas GenerateWorldSpaceCanvas(SpriteRenderer spriteRenderer)
        {
            // Create a new GameObject for the Canvas and attach it to the sprite's GameObject
            GameObject canvasObject = new GameObject("WorldCanvas");
            canvasObject.transform.SetParent(spriteRenderer.transform, false); // Attach to the sprite

            // Add the Canvas component and set it to World Space
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            // Add a CanvasScaler component to handle UI scaling in world space
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.dynamicPixelsPerUnit = 10; // Adjust as needed for sharpness

            // Add a GraphicRaycaster to the Canvas (if you want the Canvas to be interactable)
            canvasObject.AddComponent<GraphicRaycaster>();

            // Get the size of the sprite in world space
            Bounds spriteBounds = spriteRenderer.bounds;
            float width = spriteBounds.size.x;
            float height = spriteBounds.size.y;

            // Calculate the margin as a percentage of the sprite's height
            float margin = height * descriptiveImagesTopMarginPercentage;

            // Adjust the position of the canvas to be above the sprite (with margin)
            canvasObject.transform.localPosition = new Vector3(0, margin, 0); // Shift upwards by the margin

            // Set the size of the Canvas to match the sprite's size
            RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);

            return canvas;
            // Optionally, you can add some UI elements to this canvas
            // AddUIElements(canvasObject);
        }

        private void AddUIDescription(Canvas canvasObject, string description)
        {
            // Create a Text object and parent it to the canvas
            GameObject textObject = new GameObject("Description");
            textObject.transform.SetParent(canvasObject.transform, false);

            // Add TextMeshProUGUI component
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = description;
            text.fontSizeMin = 0;
            text.font = fontAsset; // Make sure fontAsset is properly set in the class
            text.fontSize = fontSize; // Set the font size as needed
            text.alignment = TextAlignmentOptions.BottomLeft; // Align text to the bottom-left
            text.enableWordWrapping = true; // Enable word wrapping if needed

            // Get the RectTransform of the TextMeshProUGUI component
            RectTransform textRectTransform = textObject.GetComponent<RectTransform>();

            // Set anchor to stretch horizontally with a top margin, and be fixed vertically
            textRectTransform.anchorMin =
                new Vector2(0, 1 - descriptiveImagesTopMarginPercentage); // Anchored just below the top of the canvas
            textRectTransform.anchorMax = new Vector2(1, 1); // Stretch across the entire width, anchored to the top
            textRectTransform.pivot = new Vector2(0.5f, 1); // Set pivot to the top-center

            // Set the offsets to 0 to make sure the text fills the defined space
            textRectTransform.offsetMin = Vector2.zero; // Left and bottom
            textRectTransform.offsetMax = Vector2.zero; // Right and top
        }

        #endregion


        #region Grid Handling

        public void HideAllLayersControlsExcluding(ImageTabButton tabButtonToKeep)
        {
            foreach (var imageTabButton in imageTabButtons)
            {
                if (imageTabButton == tabButtonToKeep)
                {
                    continue;
                }

                imageTabButton.HideLayersControl();
            }
        }

        public void HideAllVideoControlsExcluding(ImageTabButton tabButtonToKeep)
        {
            foreach (var imageTabButton in imageTabButtons)
            {
                if (imageTabButton == tabButtonToKeep)
                {
                    continue;
                }

                imageTabButton.HideCanvasGroup();
            }
        }

        public void ToggleCanvas()
        {
            float fadeValue = canvasGroup.alpha == 0 ? 1f : 0;
            canvasGroup.DOFade(fadeValue, 0.2f);
        }

        #endregion


        [Button, TabGroup("Functions")]
        public Vector3 GetImageSizeInUnits(Sprite img)
        {
            float pixelsPerUnit = img.pixelsPerUnit;
            float x = img.rect.width / pixelsPerUnit;
            float y = img.rect.height / pixelsPerUnit;

            return new Vector3(x, y, 0);
        }
    }
}