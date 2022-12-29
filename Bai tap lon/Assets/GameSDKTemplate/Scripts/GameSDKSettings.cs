using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSDKSettings {
    public const string applovinSDKKey = "ehSDH0DZmAq1wZ8lg9CArzAulgcEJT2htBR-Krwog96y6qCEtm47gZiVtwGo0q-EdB4Rf6ozX9f0hkFPLJPinv";

#if UNITY_ANDROID
#if ENV_PROD
    public const string ADMOB_APP_OPEN_ID = "ca-app-pub-9082660478786368/9445237804";
#else
    public const string ADMOB_APP_OPEN_ID = "ca-app-pub-3940256099942544/3419835294";
#endif
    public const string APPLOVIN_BANNER_ID = "46a1bec2114f9fe3";
    public const string APPLOVIN_REWARDED_ID = "dd5f30a353acf291";
    public const string APPLOVIN_INTER_ID = "49a20584c7b74802";
    public const string ironSourceKey = "1408f4591";
    public const string adjustAppId = "kfz0ab96eznk";
#else
#if ENV_PROD
    public const string ADMOB_APP_OPEN_ID = "ca-app-pub-9082660478786368/8504239960";
#else
    public const string ADMOB_APP_OPEN_ID = "ca-app-pub-3940256099942544/5662855259";
#endif
    public const string APPLOVIN_BANNER_ID = "b8c4b2ae5fec08e9";
    public const string APPLOVIN_REWARDED_ID = "a47d6c26d8543422";
    public const string APPLOVIN_INTER_ID = "211e49953d1edb34";
    public const string ironSourceKey = "147a4dac1";
    public const string adjustAppId = "a030mrrg4utc";

#endif
    public static string appleAppId = "1616129914";

}
