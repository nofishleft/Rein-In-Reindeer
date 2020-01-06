using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScreenShot : Editor
{
    [MenuItem("Tools/Take Screenshot")]
    static void Screenshot()
    {
        ScreenCapture.CaptureScreenshot("Assets/screenshot.png");
    }
}
