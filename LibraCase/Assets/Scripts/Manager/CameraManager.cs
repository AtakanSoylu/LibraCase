using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//sil
[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    //For device screen size optimation
    [SerializeField] private float _bestOrthographicFit = 667f;

    private void Start()
    {
        AdjustCameraOrthographicSize();
    }
    void OnGUI()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        Camera.main.orthographicSize = _bestOrthographicFit / currentAspect / 200f;
    }
    public void AdjustCameraOrthographicSize()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        Camera.main.orthographicSize = _bestOrthographicFit / currentAspect / 200f;
    }
}