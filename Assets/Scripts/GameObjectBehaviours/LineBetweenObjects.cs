using System;
using UnityEngine;

namespace GameObjectBehaviours
{
    public class LineBetweenObjects : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        
        private Transform startTransform;
        private Transform endTransform;
        
        private bool initilized;

        
        public void Init(Transform start, Transform end)
        {
            startTransform = start;
            endTransform = end;
            
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
            
            lineRenderer.positionCount = 4;
            initilized = true;
        }
        
        public void SetEndPosition(Transform end)
        {
            endTransform = end;
        }
        
        private void Update()
        {
            if (!initilized || startTransform == null || endTransform == null)
            {
                return;
            }
            
            lineRenderer.SetPosition(0, startTransform.position);
            
            Vector3 midPoint = (startTransform.position + endTransform.position) / 2;
            Vector3 firstControlPoint = Vector3.Lerp(startTransform.position, midPoint, 0.75f);
            Vector3 secondControlPoint = Vector3.Lerp(midPoint, endTransform.position, 0.75f);
            
            lineRenderer.SetPosition(1, firstControlPoint);
            lineRenderer.SetPosition(2, secondControlPoint);
            
            lineRenderer.SetPosition(3, endTransform.position);
        }
    }
}