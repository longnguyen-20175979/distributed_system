using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionVipSelect : MonoBehaviour {
    [SerializeField] GameObject select;
    [SerializeField] bool isBest;
    [SerializeField] Image imgBtn;
    [SerializeField] Sprite spNoSelect, spSelect;
    private void Start() {
        if (isBest)
            SelectOption();
    }
    public void SelectOption() {
        select.SetActive(true);
        imgBtn.sprite = spSelect;

    }
    public void NoSelectOption() {
        select.SetActive(false);
        imgBtn.sprite = spNoSelect;
    }
}
