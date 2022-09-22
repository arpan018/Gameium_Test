using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private Vector3 offset;
    public float smothVal=0.3f;
    private Vector3 velocity = Vector3.zero;
    public bool useDamp;

    // void Start () 
    // {
    //     offset = transform.position - target.transform.position;
    // }

    // void LateUpdate ()
    void FixedUpdate ()
    {
        if (useDamp)
            // if ((transform.position - target.position).sqrMagnitude > 0.01 * 0.01)
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smothVal);
        else
            transform.position = target.transform.position;// + offset;
    }
}
