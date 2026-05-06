/*using DG.Tweening;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class CheckPlayerUI : MonoBehaviour
{
    [SerializeField] private Image Fill;
    [SerializeField] private GameObject BGImage;
    [SerializeField] private UnityEvent _evt;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _color;

    private Material _fontMat;

    private void Awake()
    {
        _fontMat = new Material(_text.fontSharedMaterial);
    }

    private void Start()
    {
        _text.fontMaterial = _fontMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        _fontMat?.DOKill(false);
        _fontMat.DOFloat(4f, "_SpecularPower", 1.5f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        _evt?.Invoke();
                    });

        //Fill?.DOKill(false);
        //BGImage.SetActive(true);
        //Fill.DOFillAmount(1f,1.5f).SetEase(Ease.Linear)
        //    .OnComplete(() =>
        //    {
        //        _evt?.Invoke();
        //    });
    }

    private void OnTriggerExit(Collider other)
    {
        _fontMat?.DOKill(false);
        _fontMat.DOFloat(0f, "_SpecularPower", 0.4f).SetEase(Ease.Linear);
        //_fontMat.DOFloat(0f, "_SpecularPower", 0.2f).SetEase(Ease.Linear);
        //Fill.DOFillAmount(0f, 0.4f).SetEase(Ease.Linear)
        //    .OnComplete(() =>
        //    {
        //        BGImage.SetActive(false);
        //    });
    }
}*/