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
    private GameObject minion;

    public Transform player;
    public bool IsDead { get; private set; } = false;
    public float attackRange;

    public int bossHealth;
    public int CurrentHP;
    public int shieldHP;
    private float rechargeTime = 10f; // Time to wait before shield recharges
    private float shieldDownTime = 0f; // Time when the shield was last down

    public bool shield;
    public bool aura;
    public bool defend;
    private int time;
    public bool minionsDead;
    public bool phase2;
    public bool newPhase;

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
        time=0;
        minionsDead=true;
        phase2=false;
        bossHealth = 50;
        shieldHP=50;
        CurrentHP = 50;
        newPhase=false;

        StartCoroutine(CheckPhaseEvery3Seconds());
        
    }


    void Update()
    {
       if(phase2){
            if(shield) return;

            if(Time.time - shieldDownTime >= rechargeTime){
                PutShieldUp();
            }

        }
    }

    IEnumerator CheckPhaseEvery3Seconds()
    {
        while (true) 
        {
            if(newPhase){
                yield return new WaitForSeconds(3f);
                newPhase=false;
                CurrentHP=bossHealth;
                Phase2Starts();
            }
            if(minion==null){
                yield return new WaitForSeconds(3f);
                minionsDead = true;
                defend=false;
            }
            TryToAttack();
            time+=3;
            yield return new WaitForSeconds(3f);

        }
    }

    public void TryToAttack(){
        if((!phase2)&&(minionsDead)){
            defend=false;
        }

        if(defend || IsDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange){
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

           //Phase 2 abilities
            if((phase2)&&(time%2==0)){
                Spikes();
            }
            if((phase2)&&(time%2==1)){
                PutAuraUp();
            }

            //Phase 1 abilities
            if((!phase2)&&(time%2==0)){
                Jump();
            }
            if((!phase2)&&(time%2==1)){
               SummonMinions();
            }
        }
        

    }

    //Animations 
    public void Die(){
        if(phase2){
            IsDead=true;
            demongirlAnimator.Play("die00 F");
            StartCoroutine(RemoveAfterAnimation());
        }
        if(!phase2){
        demongirlAnimator.Play("die00");
        newPhase=true;
        //yield return new WaitForSeconds(2f);
        }


    }
    public void Phase2Starts(){
        phase2=true;
        bool switchLayer = true;
        demongirlAnimator.SetLayerWeight(1, switchLayer ? 1 : 0); // Layer 2 (Phase2)
        demongirlAnimator.SetLayerWeight(0, switchLayer ? 0 : 1); // Base Layer
        PutShieldUp();
    }

    public void Jump(){
        demongirlAnimator.Play("idlecombat00");
        ApplyDamageToPlayer(20);
    }

    public void Spikes(){
        demongirlAnimator.Play("idlecombat00");
        spikesAnimator.Play("upAll");
        ApplyDamageToPlayer(30);
    }

    public void PutShieldDown(){
        shieldDownTime = Time.time; // Record the time when shield went down
        shield=false;
        shieldAnimator.SetBool("ShieldDown", true);
        Shield.SetActive(false);
    }
    public void PutShieldUp(){
        shieldHP = 50;
        Shield.SetActive(true);
        shield=true;
        shieldAnimator.SetBool("ShieldDown", false);
        shieldAnimator.SetBool("Phase2", true);
    }

    public void PutAuraDown(){
        aura=false;
        defend=false;
        Aura.SetActive(false);
        ApplyDamageToPlayer(15);
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
            new Vector3(2f, 1f, 2f),  // Front-right
            new Vector3(-2f, 1f, 2f), // Front-left
            new Vector3(0f, 0f, -2f)  // Behind
        };

        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPosition = mainPosition + spawnOffsets[i];
            minion = Instantiate(Minion, spawnPosition, Quaternion.identity);
            minion.name = $"Minion_{i + 1}";
            minion.SetActive(true);
        }
    }

    void ApplyDamageToPlayer(int damageAmount)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null && !IsDead)
        {
            playerStats.TakeDamage(damageAmount);
        }
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        if(CurrentHP==0) return;
        if(aura){
            PutAuraDown(); //remove aura
            return;
        }
        if(shield){
            if((shieldHP-amount)>0){
                shieldHP-=amount;
            }
            if((shieldHP-amount)==0){ 
                PutShieldDown(); //remove shield for 10 sec
            }
            if((shieldHP-amount)<0){
                PutShieldDown();
                CurrentHP = CurrentHP - (amount-shieldHP);
                CurrentHP = Mathf.Clamp(CurrentHP, 0, bossHealth);
                Debug.Log($"Boss took {amount} damage. CurrentHP: {CurrentHP}");
                
                //check if Boss is dead
                if (CurrentHP <= 0 && !IsDead)
                {
                    Die();

                    if(!phase2){
                    CurrentHP=bossHealth;
                    Phase2Starts();
                    }
                    
                }

            }
            return;
        }
        demongirlAnimator.Play("hit01");

        CurrentHP -= amount;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, bossHealth);
        Debug.Log($"Boss took {amount} damage. CurrentHP: {CurrentHP}");

        if (CurrentHP <= 0 && !IsDead)
        {
            Die();
        }

    }

    IEnumerator RemoveAfterAnimation()
    {
        yield return null;
        float animationDuration = 0f;
        if (demongirlAnimator != null)
            animationDuration = demongirlAnimator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationDuration + 2.0f);

        Destroy(gameObject);
    }
   
}
