//using UnityEngine;

//public static class AndroidVibrator
//{

//    public static bool isVibrationEnabled = true;


//#if UNITY_ANDROID && !UNITY_EDITOR
//    private static AndroidJavaObject vibrator = null;
//    private static AndroidJavaClass vibrationEffectClass = null;
//    private static int defaultAmplitude = -1;
//    private static bool isInitialized = false;
//#endif

//    private static void Init()
//    {
//#if UNITY_ANDROID && !UNITY_EDITOR
//        if (isInitialized) return;

//        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
//        {
//            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
//        }

//        // 안드로이드 API 레벨 확인 (API 26 이상부터 VibrationEffect 사용)
//        using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
//        {
//            int apiLevel = version.GetStatic<int>("SDK_INT");
//            if (apiLevel >= 26)
//            {
//                vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
//                defaultAmplitude = vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
//            }
//        }
        
//        isInitialized = true;
//#endif
//    }

//    /// <summary>
//    /// 지정된 시간(밀리초)만큼 안드로이드 기기에 진동을 발생시킵니다.
//    /// </summary>
//    /// <param name="milliseconds">진동 시간 (예: 50)</param>
//    public static void Vibrate(long milliseconds)
//    {

//        if (!isVibrationEnabled) return;
//#if UNITY_ANDROID && !UNITY_EDITOR
//        Init();

//        if (vibrator != null && vibrator.Call<bool>("hasVibrator"))
//        {
//            if (vibrationEffectClass != null) // API 26 이상
//            {
//                AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, defaultAmplitude);
//                vibrator.Call("vibrate", vibrationEffect);
//            }
//            else // API 26 미만
//            {
//                vibrator.Call("vibrate", milliseconds);
//            }
//        }
//#else
//        // 유니티 에디터나 다른 플랫폼에서는 로그만 출력하거나 무시합니다.
//        Debug.Log($"[Vibrator] {milliseconds}ms 진동 발생 (에디터 전용 로그)");
//#endif
//    }

//    public static void Cancel()
//    {
//#if UNITY_ANDROID && !UNITY_EDITOR
//        if (isInitialized && vibrator != null)
//        {
//            vibrator.Call("cancel");
//        }
//#endif
//    }
//}