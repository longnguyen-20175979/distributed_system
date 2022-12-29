﻿//using AppsFlyerSDK;
//using Facebook.Unity;
using com.adjust.sdk;
using Facebook.Unity;
using Firebase.Crashlytics;
using Firebase.DynamicLinks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SDKInitializer : MonoBehaviour {

    private bool isFirebaseInitialized;
    private bool tokenSent;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void LoadAppOpenAd() {
        AppOpenAdManager.Instance.LoadAd(ScreenOrientation.Landscape);
    }

    // Start is called before the first frame update
    private void Awake() {
        InitializeFirebase();
        InitializeFacebook();
    }

    private void Start() {
        StartCoroutine(InitializeFirebaseMessaging());
        InitializeAdjust();
        StartCoroutine(UpdateAdjustAdId());
        AdsMediationController.Instance.InitRewardedAds();
    }

	private void OnDynamicLink(object sender, ReceivedDynamicLinkEventArgs e) {
        if (e != null) {
            Dictionary<string, string> queryParams = ParseQueryString(e.ReceivedDynamicLink.Url.Query);
            if (queryParams.ContainsKey("eventCode")) {
                string eventCode = queryParams["eventCode"];
                if (eventCode.Equals(StringConstants.HALLOWEEN_2022_EVENT_CODE)) {
                    GameData.isPendingShowHalloweenPopup = true;
                }
            }
        }
        
    }

	private void InitializeFirebase() {
        if (!isFirebaseInitialized) {
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                if (task.Result == Firebase.DependencyStatus.Available) {
                    Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                    isFirebaseInitialized = true;
                    GameAnalytics.isFirebaseInitialized = true;
                    InitializeFirebaseComponents();
                } else {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                }
                InitializeFirebaseComponents();
            }, taskScheduler);
        } else {
            isFirebaseInitialized = true;
            GameAnalytics.isFirebaseInitialized = true;
            InitializeFirebaseComponents();
        }
    }

    private void InitializeFirebaseComponents() {
        Crashlytics.IsCrashlyticsCollectionEnabled = true;
        RemoteConfigManager.InitializeRemoteConfig();
        SetupSessionTimeCount();
        DynamicLinks.DynamicLinkReceived += OnDynamicLink;
    }

    private IEnumerator InitializeFirebaseMessaging() {
        while (!isFirebaseInitialized) {
            if (Time.realtimeSinceStartup > 5.0f) {
                break;
            }
            yield return null;
        }
        if (isFirebaseInitialized) {
#if ENV_LOG
                    Debug.Log("Initializing Firebase Messaging");
#endif
            Firebase.Messaging.FirebaseMessaging.TokenReceived += FirebaseMessaging_TokenReceived;
            Firebase.Messaging.FirebaseMessaging.MessageReceived += FirebaseMessaging_MessageReceived;
        }
    }

    private void FirebaseMessaging_MessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
    }

    private void FirebaseMessaging_TokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs e) {
#if ENV_LOG
            Debug.Log("FirebaseMessaging token: " + e.Token);
#endif
    }

    private void InitializeFacebook() {
        if (!FB.IsInitialized) {
            FB.Init(InitCallback, OnHideUnity);
        } else {
            GameAnalytics.isFBInitialized = true;
        }
    }

    private void OnHideUnity(bool isUnityShown) {
        if (!isUnityShown) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    private void InitCallback() {
        if (FB.IsInitialized) {
            GameAnalytics.isFBInitialized = true;
            FB.ActivateApp();
        } else {
            Debug.LogError("Failed to Initialize the Facebook SDK");
        }
    }

    private void SetupSessionTimeCount() {
        SDKLogsPrefs.SessionID++;
        if (SDKLogsPrefs.firstOpen) {
            SDKLogsPrefs.firstOpen = false;
            SDKLogsPrefs.firstOpenTime = UnbiasedTime.Instance.Now();
        } else {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("session_id", SDKLogsPrefs.SessionID.ToString());
        }
        var deltaTime = UnbiasedTime.Instance.Now() - SDKLogsPrefs.firstOpenTime;
        if (SDKLogsPrefs.DayFromInstall != (int)deltaTime.TotalDays) {
            SDKLogsPrefs.EngageDay++;
        }
        SDKLogsPrefs.DayFromInstall = (int)deltaTime.TotalDays;
        GameAnalytics.LogFirebaseUserProperty("session_id", $"ss{SDKLogsPrefs.SessionID}:dfi{SDKLogsPrefs.DayFromInstall}:d{SDKLogsPrefs.EngageDay}");

    }
    private void InitializeAdjust() {
#if ENV_PROD
        AdjustEnvironment environment = AdjustEnvironment.Production;
#else
        AdjustEnvironment environment = AdjustEnvironment.Sandbox;
#endif

        AdjustConfig config = new AdjustConfig(GameSDKSettings.adjustAppId, environment, true);
#if ENV_PROD
        config.setLogLevel(AdjustLogLevel.Suppress);
#else
        config.setLogLevel(AdjustLogLevel.Info);
#endif
        config.setSendInBackground(true);
        Adjust.start(config);
    }
    IEnumerator UpdateAdjustAdId() {
        while (string.IsNullOrEmpty(Adjust.getAdid()) || !isFirebaseInitialized) {
            yield return null;
        }
        GameAnalytics.LogFirebaseUserProperty("adid", Adjust.getAdid());
    }


	private Dictionary<string, string> ParseQueryString(string query) {
		if (query.Length == 0)
			return null;
		Dictionary<string, string> result = new Dictionary<string, string>();
		Encoding encoding = Encoding.UTF8;
		var decodedLength = query.Length;
		var namePos = 0;
		var first = true;

		while (namePos <= decodedLength) {
			int valuePos = -1, valueEnd = -1;
			for (var q = namePos; q < decodedLength; q++) {
				if ((valuePos == -1) && (query[q] == '=')) {
					valuePos = q + 1;
				} else if (query[q] == '&') {
					valueEnd = q;
					break;
				}
			}

			if (first) {
				first = false;
				if (query[namePos] == '?')
					namePos++;
			}

			string name;
			if (valuePos == -1) {
				name = null;
				valuePos = namePos;
			} else {
				name = UnityWebRequest.UnEscapeURL(query.Substring(namePos, valuePos - namePos - 1), encoding);
			}
			if (valueEnd < 0) {
				namePos = -1;
				valueEnd = query.Length;
			} else {
				namePos = valueEnd + 1;
			}
			var value = UnityWebRequest.UnEscapeURL(query.Substring(valuePos, valueEnd - valuePos), encoding);

			result.Add(name, value);
			if (namePos == -1)
				break;
		}
		return result;
	}
}
