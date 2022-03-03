using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    //For device screen size optimation
    [SerializeField] private float _bestOrthographicFit = 667f;

    private void Start()
    {
        AdjustCameraOrthographicSize();
    }

    public void AdjustCameraOrthographicSize()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        Camera.main.orthographicSize = _bestOrthographicFit / currentAspect / 200f;
    }
}