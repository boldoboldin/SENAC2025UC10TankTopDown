using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TracksCtrl : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Animator tracksAnim;
    private Transform tankTransform;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpd;
    [SerializeField] private float rotationSpd;
    private Vector2 previousMoveInput;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            return;
        }
        
        inputReader.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner)
        {
            return;
        }

        inputReader.MoveEvent -= HandleMove;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        tankTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
        {
            return;
        }
        
        float rotation = previousMoveInput.x * -rotationSpd * Time.deltaTime;

        tankTransform.Rotate(0f, 0f, rotation);
    }

    void FixedUpdate()
    {
        if(!IsOwner)
        {
            return;
        }

        rb.velocity = (Vector2)tankTransform.up * previousMoveInput.y * moveSpd;
    }

    private void HandleMove(Vector2 moveInput)
    {
        previousMoveInput = moveInput;

        tracksAnim.SetFloat("moveInput", moveInput.magnitude);
    }
}
