using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton: Supaya script lain gampang akses script ini
    public static GameManager instance;

    [Header("Masukkan Object Gerbang di Sini")]
    public GameObject[] bossGates;

    private void Awake()
    {
        // Setup Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CloseGate()
    {
        foreach (GameObject gate in bossGates)
        {
            if (gate != null)
            {
                gate.SetActive(true);
            }
        }
    }

    // Fungsi ini akan dipanggil oleh Boss saat dia mati
    public void BossHasDied()
    {
        foreach (GameObject gate in bossGates)
        {
            if (gate != null)
            {
                // Bisa Destroy, atau SetActive(false)
                gate.SetActive(false); 
                // Destroy(gate); // Kalau mau dihancurkan selamanya
            }
        }
        Debug.Log("Boss Mati! Semua gerbang terbuka.");
    }
}