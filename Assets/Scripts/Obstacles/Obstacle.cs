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
            if (pc.isDead)
            {
                // Zresetuj licznik hasPlayedSound w klasie Poop po śmierci gracza
                Poop[] poopObjects = FindObjectsOfType<Poop>();
                foreach (Poop poop in poopObjects)
                {
                    poop.HasPlayedSound = false;
                }
                return;
            }

            if (!pc.isInAir) pc.Die();
            Debug.Log("hit");
        }
    }
}
