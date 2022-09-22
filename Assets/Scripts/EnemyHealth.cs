using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealth : MonoBehaviour
{
    public Image healthBar;

    private float health;
    private bool isDied;
    private Enemy m_Enemy;

    void Start()
    {
        health = 1;
        healthBar.fillAmount = health;
        m_Enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            HitByBullet();
    }

    public void HitByBullet()
    {
        if (isDied)
            return;

        health -= .1f;
        healthBar.DOFillAmount(health, .1f);
        if (health <= 0.01f)
        {
            isDied = true;
            m_Enemy.EnemyDie();
        }
    }
}
