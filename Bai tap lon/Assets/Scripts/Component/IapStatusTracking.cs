using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IapStatusTracking : MonoBehaviour
{
    public void StartPurchaseIap() {
        GameData.inIap = true;
    }
}
