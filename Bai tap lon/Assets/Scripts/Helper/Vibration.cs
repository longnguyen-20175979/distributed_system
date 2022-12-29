using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vibration
{
    //public static void SetVibration()
    //{
    //    if (GameData.VibrationEnabled)
    //    {
    //        MMVibrationManager.Haptic(HapticTypes.Failure);
    //    }
    //}

    //public static void VibrateLight()
    //{
    //    if (GameData.VibrationEnabled)
    //    {
    //        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    //    }
    //}

    public static void VibrateLightImpact()
    {
        if (GameData.IsVibrateEnabled)
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
    }

    public static void VibrateWarning()
    {
        if (GameData.IsVibrateEnabled)
        {
            MMVibrationManager.Haptic(HapticTypes.Warning);
        }
    }
}
