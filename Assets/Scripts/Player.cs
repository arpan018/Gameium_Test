using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    //public Animator anim;
    public float moveSpeed;
    public float rotationSpeed;
    public Transform bulletPoint;
    public Transform Body;
    public Transform bulletPrefab;
    public DynamicJoystick dJS;

    [SerializeField] private float m_RateOfFire = .15f;
    [SerializeField] private float m_aimSpeed = 3f;
    [SerializeField] private float m_rotationSpeed = 3f;
    [SerializeField] private bool doShoot;

    private bool m_DoFollowPlayer;
    private Transform m_FollowPlayer;
    private bool doMove;
    private float xRot;
    private Rigidbody selfRb;
    private bool isGameFinished;
    private PlayerHealth m_health;

    public static UnityAction PlayerDiedAction;

    void Start()
    {
        selfRb = GetComponent<Rigidbody>();
        m_health = GetComponent<PlayerHealth>();
        InvokeRepeating(nameof(gunFire), 1, m_RateOfFire);
    }

    void Update()
    {
        if (isGameFinished)
            return;

        if (Input.GetMouseButtonDown(0))
            doMove = true;
        else if (Input.GetMouseButtonUp(0))
            doMove = false;

        // joystick rotate
        if (doMove)
        {
            float rotationY = Mathf.Atan2(dJS.Horizontal, dJS.Vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotationY, 0.0f);
        }

        if (m_DoFollowPlayer)
            LookAtEnemy();
    }

    private void FixedUpdate()
    {
        if (!doMove)
            return;
        selfRb.MovePosition(selfRb.position + transform.forward * moveSpeed * Time.deltaTime);
    }

    private void gunFire()
    {
        if (!doShoot)
            return;

        Transform tempBulet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);
        tempBulet.GetComponent<Rigidbody>().AddForce(bulletPoint.forward * 40, ForceMode.Impulse);
        Destroy(tempBulet.gameObject, 3f);
    }

    public void playerKilled()
    {
        Debug.Log("Player died / Player reached finish line");
        isGameFinished = true;
        doMove = false;
        doShoot = false;
        PlayerDiedAction?.Invoke();
    }

    internal void Finished()
    {
        isGameFinished = true;
        doMove = false;
        doShoot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            // player hit by bullet, decrease health
            m_health.HitByBullet();
        }
    }

    internal void setFollowTarget(Transform transform)
    {
        m_DoFollowPlayer = transform != null;
        doShoot = transform != null;
        m_FollowPlayer = transform;
    }

    private void LookAtEnemy()
    {
        if (m_FollowPlayer != null)
        {
            Vector3 target = m_FollowPlayer.position - transform.position;
            float step = m_aimSpeed * Time.deltaTime;
            Vector3 dir = Vector3.RotateTowards(transform.forward, target, step, 0.0f);
            Debug.DrawRay(transform.position, dir, Color.red, 100);
            dir.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * m_rotationSpeed);
        }
    }
}
