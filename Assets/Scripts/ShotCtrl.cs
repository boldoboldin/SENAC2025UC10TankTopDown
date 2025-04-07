using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCtrl : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform shotSpawPoint;
    [SerializeField] private GameObject shotPrefab;
    
        private Collider2D col2d;

    [SerializeField] private float shotSpd, fireRate;

    
    private bool shouldFire;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        inputReader.PrimaryFireEvent += HandlePrimaryFire;

        col2d = GetComponent<Collider2D>();
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (!shouldFire)
        {
            return;
        }

        PrimaryFire(shotSpawPoint.position, shotSpawPoint.up);

        timer = 1 / fireRate;
    }

    private void PrimaryFire(Vector3 spawnPos, Vector3 direction)
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
