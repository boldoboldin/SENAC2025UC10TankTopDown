using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CannonCtrl : NetworkBehaviour
{
    [SerializeField] private Transform cannonTransform;
    [SerializeField] private InputReader inputReader;

    [SerializeField] private float rotationSpd;

    // Update is called once per frame
    void Update()
    {
        Vector2 aimScreenPos = inputReader.aimPosition;
        Vector2 aimWorldPos = Camera.main.ScreenToWorldPoint(aimScreenPos);
        Vector2 direction = aimWorldPos - (Vector2)cannonTransform.position;

        float previousAngle = cannonTransform.eulerAngles.z;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float currentAngle = Mathf.LerpAngle(previousAngle, 
        
        
        targetAngle, rotationSpd * Time.deltaTime);

        cannonTransform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
