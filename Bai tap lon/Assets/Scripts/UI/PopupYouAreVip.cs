using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupYouAreVip : BaseBox {
    public static PopupYouAreVip instance;
    public static PopupYouAreVip Setup() {
        if (instance == null) {
            instance = Instantiate(Resources.Load<PopupYouAreVip>(StringConstants.PathPrefabs.POPUP_YOU_ARE_VIP));
        }
        return instance;
    }
}
