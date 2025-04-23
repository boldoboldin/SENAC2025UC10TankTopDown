using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShotCtrl : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform shotSpawPoint;
    [SerializeField] private GameObject shotPrefab;
    [SerializeField] private Animator cannonAnim;
    private Collider2D col2d;

    [SerializeField] private float shotSpd, fireRate;
    
    private bool shouldFire;
    private float timer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }

        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }

        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        col2d = GetComponent<Collider2D>();
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (!shouldFire)
        {
            return;
        }

        PrimaryFireServerRpc(shotSpawPoint.position, shotSpawPoint.up);
        SpawnVirtualShot(shotSpawPoint.position, shotSpawPoint.up);

        timer = 1 / fireRate;
    }

    

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        GameObject shotInstace = Instantiate(shotPrefab, spawnPos, Quaternion.identity);
        shotInstace.transform.up = direction;
        Physics2D.IgnoreCollision(col2d, shotInstace.GetComponent <Collider2D>());

        cannonAnim.SetTrigger("atkInput");

        if(shotInstace.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * shotSpd;
        }

        SpawnVirtalShotClientRpc(spawnPos, direction);
    }


    [ClientRpc]
    private void SpawnVirtalShotClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (!IsOwner)
        {
            return;
        }

        SpawnVirtualShot(spawnPos, direction);
    }

    private void SpawnVirtualShot(Vector3 spawnPos, Vector3 direction)
    {
        GameObject shotInstace = Instantiate(shotPrefab, spawnPos, Quaternion.identity);
        shotInstace.transform.up = direction;
        Physics2D.IgnoreCollision(col2d, shotInstace.GetComponent <Collider2D>());

        if(shotInstace.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * shotSpd;
        }

    }
    
}
