using System;
using UnityEngine;
using Shapes;

public class RectangleHandler : MonoBehaviour
{
    
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabPointOffset = 0.5f;
    private Rectangle rectangle;

    private void OnValidate()
    {
        if (rectangle == null)
        {
            rectangle = GetComponent<Rectangle>();
        }
        grabPoint.position = new Vector3(rectangle.Width, rectangle.Height, 0);
    }


    private void OnEnable()
    {
        rectangle = GetComponent<Rectangle>();

    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rectangle.Width = grabPoint.position.x;        
    }
}
