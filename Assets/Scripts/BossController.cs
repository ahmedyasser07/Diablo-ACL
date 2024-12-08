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
    public bool shield;
    public bool aura;
    public bool defend;
    private int time;
    public bool minionsDead;
    public bool phase2;

    void Start()
    {
        if (demongirlAnimator == null || spikesAnimator == null || shieldAnimator == null)
        {
            Debug.LogError("Animator references are missing!");
        }
        Shield.SetActive(false);
        shield=false;
        aura=false;
        defend=false;
        StartCoroutine(CheckPhaseEvery3Seconds());
        time=0;
        minionsDead=true;
        phase2=false;
    }


    void Update()
    {
        if (!phase2)
    {
        PutShieldUp();  // Put shield up only once when Phase2 starts
        phase2 = true;  // Mark that Phase2 is active
        defend=false;
    }
    }

    IEnumerator CheckPhaseEvery3Seconds()
    {
        while (true) 
        {
            //Phase 2
           if(phase2){
               if((!defend)&&(time%2==1)){
                PutAuraUp();
               }
               else if(((!defend)||(time%2==0))&&(!aura)){
                Spikes();
               }
           }
           
           //Phase 1
           else if(!phase2){
            if((!defend)&&(time%2==1)&&(minionsDead)&&(!phase2)){
                SummonMinions();
               }
               else if((time%2==0)&&(!phase2)){
                Jump();
               }
           }
            time+=3;
            yield return new WaitForSeconds(3f);

        }
    }

    public void GetHit(){
        demongirlAnimator.Play("hit01");
    }

    public void DieOn(){
        demongirlAnimator.SetBool("Die", true);
        demongirlAnimator.Play("die00");
    }
    public void DieOff(){
        demongirlAnimator.SetBool("Die", false);
        bool switchLayer = true;
        demongirlAnimator.SetLayerWeight(1, switchLayer ? 1 : 0); // Layer 2 (Phase2)
        demongirlAnimator.SetLayerWeight(0, switchLayer ? 0 : 1); // Base Layer
        PutShieldUp();
    }

    public void Jump(){
        demongirlAnimator.Play("idlecombat00");
    }

    public void Spikes(){
        demongirlAnimator.Play("idlecombat00");
        spikesAnimator.Play("upAll");
    }

    public void PutShieldDown(){
        shield=false;
        shieldAnimator.SetBool("ShieldDown", true);
        Shield.SetActive(false);
    }
    public void PutShieldUp(){
        Shield.SetActive(true);
        shield=true;
        shieldAnimator.SetBool("ShieldDown", false);
        shieldAnimator.SetBool("Phase2", true);
    }

    public void PutAuraDown(){
        aura=false;
        defend=false;
        Aura.SetActive(false);
    }
    public void PutAuraUp(){
        aura=true;
        defend=true;
        Aura.SetActive(true);
    }
    
    public void SummonMinions(){
        minionsDead=false;
        defend=true;
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
