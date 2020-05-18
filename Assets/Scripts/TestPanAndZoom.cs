using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class TestPanAndZoom : MonoBehaviour
{
    [SerializeField] ProCamera2DPanAndZoom zoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 360 == 0)
        {
            Debug.Log($"Active {zoom.isActiveAndEnabled} Zoom allowed: {zoom.AllowZoom} Pan: {zoom.AllowPan}");

            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        }
    }
}
