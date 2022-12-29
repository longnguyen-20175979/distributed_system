using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInput : MonoBehaviour
{
    public static UIInput instance;
    private float direction;

    public float Direction { get => direction; set => direction = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public float GetDirectionFromButton()
    {
        return direction;
    }   
}
