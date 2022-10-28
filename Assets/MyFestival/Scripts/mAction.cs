
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class mAction : MonoBehaviour
{
    public GameObject[] gameObjects;
    public static mAction Instance;
    private bool isPortrait;
    public bool toggleAnimBool = false;
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 모바일회전에맞춰ui회전
    /// </summary>
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

    //------버튼 액션---
 
    public void SwitchPage(GameObject[] mList,int index )
    {
        for (int i = 0; i < mList.Length; i++)
        {
            if (i == index)
                mList[i].SetActive(true);
            else
            mList[i].SetActive(false);
        }
    }
}
