using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage;

    private ulong ownerClientId;

    // Update is called once per frame
    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null)
        {
            return;
        }

        if (collision.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (ownerClientId == netObj.OwnerClientId)
            {
                return;
            }

            if (collision.attachedRigidbody.TryGetComponent<HPCtrl>(out HPCtrl health))
            {
                health.TakeDamage(damage);
            }
        }
    }
}
