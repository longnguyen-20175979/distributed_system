using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using AOT;
#if UNITY_IOS
using System.Runtime.InteropServices;
using UnityEngine.iOS;
#endif
using UnityEngine;

public class IOSHelper {
#if UNITY_IOS
    private static UnityAction<int, string> dcCallback;
    [DllImport("__Internal")]
    private static extern void NativeOpenAppSettings();

    [DllImport("__Internal")]
    private static extern bool NativeIsAppInstalled(string bundleIdentifier);

    [DllImport("__Internal")]
    public static extern void EnableFBAdvertiserTracking();
#endif

    public static void OpenAppSettings() {
#if UNITY_IOS
        NativeOpenAppSettings();
#endif
    }

    public static bool IsAppInstalled(string bundleIdentifier) {
#if UNITY_IOS
        return NativeIsAppInstalled(bundleIdentifier);
#else
        return false;
#endif
    }


}
