using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator demongirlAnimator;
    public GameObject minion;
    public GameObject Shield;
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
        demongirlAnimator.SetBool("GetHit", true);
        demongirlAnimator.SetBool("Die", true);
        bool switchLayer = shieldAnimator.GetBool("Phase2");
        demongirlAnimator.SetLayerWeight(1, switchLayer ? 1 : 0); 
        demongirlAnimator.SetLayerWeight(0, switchLayer ? 0 : 1); 
        shieldAnimator.SetBool("Phase2", true);
        Shield.SetActive(true);
        demongirlAnimator.SetBool("Spikes",true);
        
    }

   
}
