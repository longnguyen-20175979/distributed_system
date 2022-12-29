using System.Collections;
using System.Collections.Generic;
//using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationPlaceHolderText : MonoBehaviour {

    //private LocalizationParamsManager localManager;

    //private void Awake()
    //{
    //    localManager = GetComponent<LocalizationParamsManager>();
    //}

    public void SetValue(string value) {
        this.SetValues(value);
        GetComponent<Text>().text = value.ToString();
    }

    public void SetValues(params string[] values) {
        //List<LocalizationParamsManager.ParamValue> paramValues = localManager._Params;
        //for (int i = 0; i < paramValues.Count; i++) {
        //    LocalizationParamsManager.ParamValue paramValue = paramValues[i];
        //    if (i < values.Length) {
        //        localManager.SetParameterValue(paramValue.Name, values[i]);
        //    }
        //}
    }
}
