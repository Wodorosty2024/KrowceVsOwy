using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController pc;
        if (col.TryGetComponent<PlayerController>(out pc))
        {
            if (pc.isDead) return;
            if (!pc.isInAir) pc.Die(true, gameObject);
            Debug.Log("hit");
        }        
    }
}
