/*using UnityEngine;

public class ResolutionFixed : MonoBehaviour
{
    Camera cam;
    void Start()
    {
        // 시작할 때 카메라 컴포넌트를 가져옵니다.
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // 에디터에서 해상도를 바꾸면 실시간으로 감지해서 도화지를 다시 자릅니다.
        float targetAspect = 16.0f / 9.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;
            rect.width = 1.0f; rect.height = scaleHeight;
            rect.x = 0; rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth; rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f; rect.y = 0;
            cam.rect = rect;
        }
    }
}*/