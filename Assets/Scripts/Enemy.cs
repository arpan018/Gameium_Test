using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public Transform bulletPoint;
    public Transform bulletPrefab;
    [SerializeField] private float m_RateOfFire=.15f;
    [SerializeField] private bool doShoot;

    private NavMeshAgent m_navmeshAgent;
    private Rigidbody m_rigidBody;
    private bool m_DoFollowPlayer;
    private EnemyHealth m_EnemyHealth;

    private void OnEnable()
    {
        Player.PlayerDiedAction += playerdied;
    }

    private void OnDisable()
    {
        Player.PlayerDiedAction -= playerdied;
    }
    
    private void playerdied()
    {
        //Debug.Log($"player died action invoked on {gameObject.name}");
        m_DoFollowPlayer = false;
        m_navmeshAgent.enabled = false;
        doShoot = false;
    }

    public void setFollowTarget(Transform transform)
    {
        m_DoFollowPlayer = transform != null;
        doShoot = transform != null;
    }

    private void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        m_navmeshAgent = GetComponent<NavMeshAgent>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_EnemyHealth = GetComponent<EnemyHealth>();
        InvokeRepeating(nameof(gunFire), 1, m_RateOfFire);

        // add ranndom speed for each enemy
        m_navmeshAgent.speed = Random.Range(3f, 5f);
    }

    void Update()
    {
        if (m_DoFollowPlayer)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(playerTransform.position, out hit, 1f, NavMesh.AllAreas))
                m_navmeshAgent?.SetDestination(playerTransform.position);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            m_EnemyHealth.HitByBullet();
        }
    }

    public void EnemyDie()
    {
        m_DoFollowPlayer = false;
        m_navmeshAgent.enabled = false;
        
        // Issue: on enemy destroy, player ontriggerexit doesnt called with enemy trigger collider
        // TODO: try another way to get enemy destroy and stop bullet spawning
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);
        Destroy(gameObject,.5f);
    }

    private void gunFire()
    {
        if (!doShoot)
            return;

        Transform tempBulet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);
        tempBulet.GetComponent<Rigidbody>().AddForce(bulletPoint.forward * 40, ForceMode.Impulse);
        Destroy(tempBulet.gameObject, 5f);
    }
}
