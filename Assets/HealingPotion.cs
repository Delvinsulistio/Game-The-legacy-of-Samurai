using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                healthValue = 60f; // Nilai penyembuhan
                player.Heal(healthValue);
                Destroy(gameObject); // Hancurkan potion setelah digunakan
            }
        }
    }
}
