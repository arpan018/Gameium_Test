using UnityEngine;

/// <summary>
/// Assigned on player gameobject to find nearby enemies, lookat and enable auto fire 
/// </summary>
public class EnemyDetector : MonoBehaviour
{

    [SerializeField]
    private Player m_Player;

    void Start()
    {
        m_Player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
            m_Player.setFollowTarget(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy"))
            m_Player.setFollowTarget(null);
    }
}
