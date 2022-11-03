
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class mAction : MonoBehaviour
{
    public static mAction Instance;
    private bool isPortrait;
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// �����ȸ��������uiȸ��
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
                //�ڵ�ȸ�� 
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
                //�ڵ�ȸ�� 
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;

            }
        }
    }

    //------��ư �׼�---
 

}
