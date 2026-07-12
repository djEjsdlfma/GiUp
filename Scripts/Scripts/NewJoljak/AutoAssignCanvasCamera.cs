/*using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class AutoAssignCanvasCamera : MonoBehaviour
{
    void Start()
    {
        Canvas myCanvas = GetComponent<Canvas>();

        // 씬에 태그가 'MainCamera'로 설정된 유일한 카메라를 찾아서 자동으로 연결합니다.
        if (myCanvas.worldCamera == null)
        {
            myCanvas.worldCamera = Camera.main;
        }
    }
}*/