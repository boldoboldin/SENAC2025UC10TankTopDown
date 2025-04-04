using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksCtrl : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    private Transform tankTransform;
    private Rigidbody2D rb; 

    
    [SerializeField] private float moveSpd;
    [SerializeField] private float rotationSpd;
    private Vector2 previousMoveInput;
    
    // Start is called before the first frame update
    void Start()
    {
        inputReader.MoveEvent += HandleMove;
        
        tankTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = previousMoveInput.x * -rotationSpd * Time.deltaTime;

        tankTransform.Rotate(0f, 0f, rotation);
    }

    void FixedUpdate()
    {
        rb.velocity = (Vector2)tankTransform.up * previousMoveInput.y * moveSpd;
    }

    private void HandleMove(Vector2 moveInput)
    {
        previousMoveInput = moveInput;
    }
}
