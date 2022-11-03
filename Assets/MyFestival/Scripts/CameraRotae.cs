using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraRotae : MonoBehaviour
{
    public float interval = 0.1f;
    private bool isPortrait;
    private float _time = 0;



    void Update()
    {
        _time += Time.deltaTime;
        if (_time > interval)
        {
            //print("do");
           ChangeUiRotation();
            _time = 0;
        }
    }

    public void ChangeUiRotation()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            if (!isPortrait)
            {
                //codes for portrait
                isPortrait = true;

                //Screen.orientation = ScreenOrientation.Portrait;
                //자동회전 
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;

            }
        }
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            if (isPortrait)
            {
                //codes for Landspace;
                isPortrait = false;

                //Screen.orientation = ScreenOrientation.LandscapeRight;
                //자동회전 
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;

            }
        }
    }
    public void SceneLoad(string ScName)
    {
        SceneManager.LoadSceneAsync(ScName, LoadSceneMode.Single);
    }
}
