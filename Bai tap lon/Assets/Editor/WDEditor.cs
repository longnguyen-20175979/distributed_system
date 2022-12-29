using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class WDEditor 
{
    [MenuItem("Tools/Wido/Capture Screenshot")]
    public static void Take() {
        string guid = $"Screenshot_{Screen.width}x{Screen.height}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";        
        Debug.Log($"path to screenshot:{guid}");
        ScreenCapture.CaptureScreenshot($"Assets/Editor/Screenshots/{guid}.png");
    }

    [MenuItem("Tools/Wido/Clear Data")]
    public static void ClearData() {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Wido/Init Google API")]
    public static void InitGoogleAPI() {
        //GoogleAPI.Instance.ToString();
    }

    [MenuItem("Tools/Wido/Nest AnimClips in Controller", true)]
    static public bool nestAnimClipsValidate()
    {
        return Selection.activeObject != null && Selection.activeObject.GetType() == typeof(UnityEditor.Animations.AnimatorController);
    }

    [MenuItem("Tools/Wido/Nest AnimClips in Controller")]
    static public void nestAnimClips()
    {
        AnimatorController anim_controller = (AnimatorController)Selection.activeObject;
        UnityEngine.Object[] objects = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(anim_controller));
        if (anim_controller != null)
        {
            AssetDatabase.SaveAssets();
            List<ChildAnimatorState> states = new List<ChildAnimatorState>();
            AnimatorControllerLayer[] layers = anim_controller.layers;
            foreach (AnimatorControllerLayer layer in layers)
            {
                states.AddRange(layer.stateMachine.states.ToList<ChildAnimatorState>());
            }
            if (states.Count > 0)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    if (states[i].state.motion)
                    {
                        var newClip = UnityEngine.Object.Instantiate((AnimationClip)states[i].state.motion) as AnimationClip;
                        newClip.name = states[i].state.motion.name;
                        AssetDatabase.AddObjectToAsset(newClip, anim_controller);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newClip));
                        states[i].state.motion = newClip;
                    }
                }
            }
        }
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetType() == typeof(AnimationClip))
            {
                UnityEngine.Object.DestroyImmediate(objects[i], true);
            }
        }
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Wido/FlintGo/Count item in level")]
    public static void CountCoinAndGemInLevel()
    {
        GameObject level;
        GameObject[] coins;
        GameObject[] itemBlocks;
        GameObject chestCoin;
        GameObject[] bullets;

        for (int x = 1; x <= Constants.MAX_LEVEL; x++)
        {           
            level = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Level/Level" + x.ToString()));

            coins = GameObject.FindGameObjectsWithTag(Constants.TAG.COIN);
            itemBlocks = GameObject.FindGameObjectsWithTag(Constants.TAG.MYSTERY_BLOCK);

            int j = 0;
            int m = 0;
            int y = 0;
            int z = 0;

            for (int i = 0; i < itemBlocks.Length; i++)
            {
                ItemPresent item = itemBlocks[i].GetComponent<ItemPresent>();
                if (item.itemType == ResourceType.Coin)
                {
                    j++;
                }
                if (item.itemType == ResourceType.Gem)
                {
                    m++;
                }
                if (item.itemType == ResourceType.Heart)
                {
                    y++;
                }
                if (item.itemType == ResourceType.BulletNormal)
                {
                    z++;
                }
            }
            chestCoin = GameObject.Find("ChestCoins");
            int k;
            int t;
            if (chestCoin != null)
            {
                k = 1;
                t = 30;
            }
            else
            {
                k = 0;
                t = 0;
            }
            int totalCoin = coins.Length + j + t;

            bullets = GameObject.FindGameObjectsWithTag(Constants.TAG.BULLET_ITEM);
            int totalBullet = bullets.Length + z;

            WDDebug.Log("=====LEVEL " + x + "=====");
            WDDebug.Log("Coin " + totalCoin + ", chestcoins " + k);
            WDDebug.Log("Gem " + m);
            WDDebug.Log("Heart " + y);
            WDDebug.Log("Bullet " + totalBullet);

            UnityEngine.Object.DestroyImmediate(level);
        }
    }
}
