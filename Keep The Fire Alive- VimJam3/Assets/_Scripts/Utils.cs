using UnityEngine;

public static class Utils
{
    private static Camera _mainCamera;

    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            return _mainCamera;
        }
    }

    public static string EndGameLostText { get; set; }

    public static string TimeLeft { get; set; }
}