/*using csiimnida._01_Code.SaveSystem.Interface;
using csiimnida._01_Code.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ClickObject : FloatEventActor
{
    [SerializeField] private float ObjClickVal = 5f;
    [SerializeField] private float SaveClickVal;
    [SerializeField] GameObject OutlineObj;
    [SerializeField] private UnityEvent _evt;

    public bool isHaveEvent;

    public UnityEvent onBreakEvent;

    private Collider _collider;

    private void Start()
    {
        OnChangeEvent += ClickObj;
        SaveClickVal = ObjClickVal;
        _collider = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        OnChangeEvent -= ClickObj;
    }
    public void Save(float value)
    {
        FloatData d = new FloatData()
        {
            FloatValue = value,
            ObjectName = gameObject.name
        };
        DataManager.Instance.Save(d);
        ClickObj(value);
    }
    private void ClickObj(float damage)
    {
        if (gameObject.activeSelf)
        {
            if (isHaveEvent)
                _evt?.Invoke();
            else
                ObjClickVal -= damage;
        }
    }

    public override void ReSet()
    {
        base.ReSet();
        ObjClickVal = SaveClickVal;
        _collider.enabled = true;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (ObjClickVal <= 0)
        {
            onBreakEvent?.Invoke();
            _collider.enabled = false;
            gameObject.SetActive(false);
        }
    }

    public float GetClickValue()
        => ObjClickVal;

    public void DetectRay(bool val)
    {
        if (OutlineObj == null) return;
        OutlineObj.SetActive(val);
    }
}*/