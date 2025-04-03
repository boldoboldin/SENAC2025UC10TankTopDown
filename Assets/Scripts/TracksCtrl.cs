using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksCtrl : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    private Transform tankTransform;
    private Rigidbody2D rb; 
    
    // Start is called before the first frame update
    void Start()
    {
        tankTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
