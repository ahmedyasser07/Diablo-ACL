using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator demongirlAnimator;
    public GameObject Minion;
    public Transform Boss;
    private Transform[] spawnPoints;
    public GameObject Shield;
    public GameObject Aura;
    public Animator spikesAnimator;
    public Animator shieldAnimator;
    void Start()
    {
        if (demongirlAnimator == null || spikesAnimator == null || shieldAnimator == null)
        {
            Debug.LogError("Animator references are missing!");
        }
        Shield.SetActive(false);

        
    }


    // Update is called once per frame
    void Update()
    {
       // SummonMinions();
        
    }

    public void GetHitOn(){
        demongirlAnimator.SetBool("GetHit", true);
    }
    public void GetHitOff(){
        demongirlAnimator.SetBool("GetHit", false);
    }

    public void DieOn(){
        demongirlAnimator.SetBool("Die", true);
    }
    public void DieOff(){
        bool switchLayer = true;
        demongirlAnimator.SetLayerWeight(1, switchLayer ? 1 : 0); // Layer 2 (Phase2)
        demongirlAnimator.SetLayerWeight(0, switchLayer ? 0 : 1); // Base Layer
        PutShieldUp();
        demongirlAnimator.SetBool("Die", false);
    }

    public void JumpOn(){
        demongirlAnimator.SetBool("Jumo", true);
    }
    public void JumpOff(){
        demongirlAnimator.SetBool("Jumo", false);
    }

    public void SpikesOn(){
        demongirlAnimator.SetBool("Spikes", true);
        spikesAnimator.SetBool("upAll", true);
    }
    public void SpikesOff(){
        demongirlAnimator.SetBool("Spikes", false);
        spikesAnimator.SetBool("upAll", false);
    }

    public void PutShieldDown(){
        shieldAnimator.SetBool("ShieldDown", true);
        Shield.SetActive(false);
    }
    public void PutShieldUp(){
        shieldAnimator.SetBool("ShieldDown", false);
        Shield.SetActive(true);
        shieldAnimator.SetBool("Phase2", true);
    }

    public void PutAuraDown(){
        Aura.SetActive(false);
    }
    public void PutAuraUp(){
        Aura.SetActive(true);
    }
    
    public void SummonMinions(){
       if (Minion == null)
          {
            Debug.LogError("Minion Prefab is not assigned!");
            return;
           }

        Vector3 mainPosition = Boss.position;
        Vector3[] spawnOffsets = new Vector3[]
        {
            new Vector3(2f, 0f, 2f),  // Front-right
            new Vector3(-2f, 0f, 2f), // Front-left
            new Vector3(0f, 0f, -2f)  // Behind
        };

        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPosition = mainPosition + spawnOffsets[i];
            GameObject minion = Instantiate(Minion, spawnPosition, Quaternion.identity);
            minion.name = $"Minion_{i + 1}";
        }
    }
   
}
