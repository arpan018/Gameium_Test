using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public bool isRagdoll { get; private set; }

    private Rigidbody rigidBody;
    private Animator animator;
    private Avatar avatar;
    // Edit: Arpan, 17-Aug-2021
    public float forceToRagdoll = 50f;

    /// <summary>
    /// Add any colliders you DON't want the ragdoll system to handle in this list
    /// By default we are adding the root collider (if it exists) to the list so
    /// DO NOT add it manually. (See SetRagdollParts function)
    /// </summary>
    private List<Collider> collidersToExclude = new List<Collider>();
    private List<Collider> ragdollColliders = new List<Collider>();

    private void Awake()
    {
        SetRagdollParts();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Sets the ragdoll parts - called in Awake.
    /// Collects all ragdoll colliders and sets relevant
    /// physics properties. On initialization, ragdoll physics is OFF
    /// </summary>
    private void SetRagdollParts()
    {
        Collider rootCollider = GetComponent<Collider>();
        if (rootCollider != null)
        {
            collidersToExclude.Add(rootCollider);
        }

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        avatar = animator?.avatar;
        isRagdoll = false;

        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach(Collider c in colliders)
        {
            if (!collidersToExclude?.Contains(c) ?? true)
            {
                SetPhysics(c, false);
                ragdollColliders.Add(c);
            }
        }
    }

    /// <summary>
    /// Use this function to enable/disable ragdoll physics for 
    /// multiple characters
    /// </summary>
    /// <param name="ragdolls">Pass multiple RagdollController objects you would like to enable/disable physics for</param>
    /// <param name="on">If set to <c>true</c> Ragdoll Physics is enabled</param>
    public static void ToggleRagdollForRagdollController(bool on, params RagdollController[] ragdolls)
    {
        foreach(RagdollController rdc in ragdolls)
        {
            rdc.ToggleRagdoll(on);
        }
    }

    /// <summary>
    /// Use this function to enable/disable ragdoll physics for an instance of 
    /// RagdollController
    /// </summary>

    public void ToggleRagdoll(bool on)
    {
        SetRigidBody(on);
        SetAnimator(on);

        foreach (Collider c in collidersToExclude)
        {
            c.enabled = !on;
        }

        foreach (Collider c in ragdollColliders)
        {
            SetPhysics(c, on);
        }

        isRagdoll = on;
    }

    private void SetPhysics(Collider c, bool on)
    {
        c.isTrigger = !on;
        // c.attachedRigidbody.velocity = Vector3.zero;
        c.attachedRigidbody.isKinematic = !on;
        // Edit: Arpan, 17-Aug-2021
        c.attachedRigidbody.AddForce(-transform.forward * forceToRagdoll, ForceMode.Impulse);
    }

    private void SetRigidBody(bool on)
    {
        if (rigidBody)
        {
            rigidBody.useGravity = !on;
            rigidBody.velocity = Vector3.zero;
        }
    }

    private void SetAnimator(bool on)
    {
        if (animator)
        {
            animator.enabled = !on;
            //animator.avatar = on ? null : avatar;
        }
    }
}
