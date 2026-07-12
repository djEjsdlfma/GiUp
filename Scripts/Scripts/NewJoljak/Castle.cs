/*using LSW._02._Scripts.Manager;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Ami.BroAudio;
using Moon._01.Script.Sounds;

public class Castle : MonoBehaviour
{
    public event Action OnCastleDied;

    public float maxHealth = 100;
    public float health;

    [SerializeField] private CastleUI _castleUI;
    [SerializeField] private PlayableDirector director;

    private Player _player;
    private float _repairAmount = 0f;
    private float _repairTimer;

    private bool isDead;

    void Awake()
    {
        ReferenceManager.Instance.Castle = this;
        health = maxHealth;
    }

    private void Start()
    {
        _player = ReferenceManager.Instance.Player;
        if (_player != null)
            _player.OnStatChanged += StatChanged;
        
        _castleUI.SetValue(health, maxHealth);
    }

    private void Update()
    {
        _repairTimer += Time.deltaTime;

        if(_repairAmount > 0 && _repairTimer >= 1)
        {
            _repairTimer = 0f;
            health += Mathf.Min(_repairAmount, maxHealth - health);
            _castleUI.SetValue(health, maxHealth);
        }

        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TakeDamage(500f);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            isDead = true;
            health = 0;
            _castleUI.SetValue(health, maxHealth);
            SoundPlayer.Instance.Play("CastleBreak");
            Die();
            return;
        }
        SoundPlayer.Instance.Play("PlayerHit");
        _castleUI.SetValue(health, maxHealth);
    }

    public void EndFall()
    {
        SoundPlayer.Instance.Play("EndFall");
    }

    public void ScoreSound()
    {
        ReferenceManager.Instance.StatUI.PlayScoreSound();
    }

    private void StatChanged(UpgradeType upgradeType, float amount)
    {
        switch (upgradeType)
        {
            case UpgradeType.WallMaxHp:
                maxHealth += amount;
                health += amount;
                _castleUI.SetValue(health, maxHealth);
                break;
            case UpgradeType.WallAutoRepair:
                _repairAmount += amount;
                break;
        }
    }

    void Die()
    {
        OnCastleDied?.Invoke();
        director.Play();
    }

    private void OnDestroy()
    {
        if (_player != null)
            _player.OnStatChanged -= StatChanged;
    }
}
*/