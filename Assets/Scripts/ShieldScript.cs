using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    AudioSource audioSrc;
    public static AudioClip Crushed;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume=0.3f;
        Crushed= Resources.Load<AudioClip>("Crushed");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag("Enemy") && player.GetComponent<playerMovement>().isAttacking){
            audioSrc.PlayOneShot(Crushed,2f);
        }
    }
}
