using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handle;
    private Toggle toggle;
    private Vector2 handlePos;

    [SerializeField] private GameObject background;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePos = handle.anchoredPosition;
        toggle.onValueChanged.AddListener(OnSwitch);
        //if (toggle.isOn)
        //{
        //    OnSwitch(true);
        //}
    }

    public void OnSwitch(bool on)
    { 
        if (!on)
        {
            handle.anchoredPosition = -handlePos;
            background.SetActive(false);
        }
        else
        {
            handle.anchoredPosition = handlePos;
            background.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
