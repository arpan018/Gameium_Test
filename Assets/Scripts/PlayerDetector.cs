using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assigned on enemy object(s) to find player and enable navmesh, auto fire
/// </summary>
public class PlayerDetector : MonoBehaviour
{
    [SerializeField]
    private Enemy m_enemy;
    
    void Start()
    {
        m_enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            m_enemy.setFollowTarget(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            m_enemy.setFollowTarget(null);
    }
}
