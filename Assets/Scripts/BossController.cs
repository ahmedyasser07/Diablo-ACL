using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Animator anim;
    public GameObject minion;
    void Start()
    {
        anim = GetComponent<Animator>();
        minion = GetComponent<GameObject>();
    }


    // Update is called once per frame
    void Update()
    {

}
