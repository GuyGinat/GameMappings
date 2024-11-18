using System.Collections.Generic;
using ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace GameObjectBehaviours.UI.Videos
{
    public class VideoPlayerRenderer : MonoBehaviour
    {
        [SerializeField] private VideoClip videoClip;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private Renderer rendererComponent;
        [SerializeField] private RenderTexture renderTexture;
    
        [SerializeField] public bool control;
        
        [SerializeField] private List<TimedVideoDescription> descriptions;
        private int currentDescriptionIndex = 0;
        
        [Button]
        public void Init(VideoClip videoClip, List<TimedVideoDescription> descriptions)
        {
            this.videoClip = videoClip;
            this.descriptions = descriptions;
        }

        [Button]
        public void CreateVideoPlayer()
        {
            // create a new render texture
            float width = videoClip.width;
            float height = videoClip.height;
            renderTexture = new RenderTexture((int)width, (int)height, 24);
        
            // assign the render texture to the video player and disable the Play on Awake option
            GetComponent<VideoPlayer>().targetTexture = renderTexture;
            videoPlayer.playOnAwake = false;
            videoPlayer.clip = videoClip;
            videoPlayer.Prepare();
            videoPlayer.StepForward();
            // videoPlayer.
        
            rendererComponent.sharedMaterial.mainTexture = renderTexture;
        
            // set the scale of the quad to match the aspect ratio of the video
            // float aspectRatio = width / height;
            transform.localScale = new Vector3(32, 18, 1);
        }
    
        void OnDestroy()
        {
            // Clean up the RenderTexture when the object is destroyed
            if (renderTexture != null)
            {
                renderTexture.Release();
                renderTexture = null;
            }
        }

        private void Update()
        {
            if (control)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (videoPlayer.isPlaying)
                    {
                        videoPlayer.Pause();
                    }
                    else
                    {
                        videoPlayer.Play();
                    }
                }
                int lastDescriptionIndex = currentDescriptionIndex;
                string description = GetDescription(videoPlayer.time);
                print(videoPlayer.time);
                SecondCameraCanvasHandler.Instance.SetDescription(description);
                // if (lastDescriptionIndex != currentDescriptionIndex)
                // {
                //     print("Setting description: " + currentDescriptionIndex);
                //     SecondCameraCanvasHandler.Instance.SetDescription(description);
                // }
            }
        }
        
        private string GetDescription(double time)
        {
            int i = 0;
            string lastDescription = "";
            foreach (var description in descriptions)
            {
                if (time >= description.TimeStamp)
                {
                    currentDescriptionIndex = i;
                    print("Passed Timestamp");
                    lastDescription = description.Description;
                }
                i++;
            }

            return lastDescription;
        }
        
    }
}
