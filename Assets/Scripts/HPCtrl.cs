using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HPCtrl : NetworkBehaviour
{
    [SerializeField] public int maxHP {get; private set; } = 100;
    public NetworkVariable<int> currentHP = new NetworkVariable<int>();
    private bool isDead;
    public Action<HPCtrl> onDie;
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }

        currentHP.Value = maxHP;
    }

    public void TakeDamage (int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoredHeath (int healValue)
    {
        ModifyHealth(
            healValue);
    }

    public void ModifyHealth(int value)
    {
        if (isDead)
        {
            return;
        }

        int newHP = currentHP.Value + value;
        currentHP.Value = Mathf.Clamp(newHP, 0 ,maxHP);

        if (currentHP.Value == 0)
        {
            onDie.Invoke(this);
            isDead = true;
        }
    }

}
