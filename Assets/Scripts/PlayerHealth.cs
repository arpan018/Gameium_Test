using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// TODO: Make generic health script with IHealthMonitor to call common functions.
    /// Get player / enemy by tag or enums
    /// </summary>

    public Image healthBar;

    private Player m_Player;
    private float health;
    private bool playerDied;

    void Start()
    {
        health = 1;
        healthBar.fillAmount = health;
        m_Player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            HitByBullet();
    }

    public void HitByBullet()
    {
        if (playerDied)
            return;

        health -= .1f;
        healthBar.DOFillAmount(health, .1f);
        if (health <= 0.01f)
        {
            // die
            playerDied = true;
            m_Player.playerKilled();
        }
    }
}
