using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAutoSize : MonoBehaviour
{

    CanvasScaler canvas;
    void Start()
    {
        canvas = gameObject.GetComponent<CanvasScaler>();
        AutoSize();
    }
    private void Update()
    {
        AutoSize();
    }
    void AutoSize()
    {
        canvas.matchWidthOrHeight = 0.5f + ((Screen.width / Screen.height) - (1920f / 1080));
    }
}
