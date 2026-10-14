//using UnityEngine;
//using UnityEngine.Rendering;
//using DG.Tweening;

//public class DamageFeedbackManager : MonoBehaviour
//{
//    [Header("Post Processing Volume")]
//    // Vignette와 LensDistortion이 세팅된 '피격 전용' Volume 오브젝트
//    public Volume damageVolume;

//    [Header("Feedback Settings")]
//    public float attackDuration = 0.1f;
//    public float recoverDuration = 0.3f;

//    private void Start()
//    {
//        // 시작할 때 볼륨의 영향력을 0으로 꺼둡니다.
//        if (damageVolume != null)
//        {
//            damageVolume.weight = 0f;
//        }
//    }

//    public void PlayDamageFeedback()
//    {
//        if (damageVolume == null) return;

//        DOTween.Kill(damageVolume);

//        Sequence seq = DOTween.Sequence();
//        seq.SetId(damageVolume);

//        // 볼륨의 Weight(영향력)를 1(100%)까지 빠르게 올렸다가 0으로 내립니다.
//        seq.Append(DOTween.To(() => damageVolume.weight, x => damageVolume.weight = x, 1f, attackDuration).SetEase(Ease.OutQuad));
//        seq.Append(DOTween.To(() => damageVolume.weight, x => damageVolume.weight = x, 0f, recoverDuration).SetEase(Ease.InQuad));
//    }
//}