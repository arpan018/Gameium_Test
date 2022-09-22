using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public ParticleSystem[] confetis;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            System.Array.ForEach(confetis, item => item.Play());
            other.GetComponentInParent<Player>().playerKilled();
        }
    }
}
