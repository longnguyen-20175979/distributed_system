using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNoInternet : BaseBox
{
    private static PopupNoInternet instance;

    public static PopupNoInternet Setup() {
        if (instance == null) {
            instance = Instantiate(Resources.Load<PopupNoInternet>(Constants.PathPrefabs.POPUP_NO_INTERNET));
        }
        return instance;
    }
}
