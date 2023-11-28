using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip BossScream, Slash, Stab, Blood, End, Cinematic, Shoot;
    AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        Shoot = Resources.Load<AudioClip>("Shoot");
        Cinematic = Resources.Load<AudioClip>("CinematicSound");
        End = Resources.Load<AudioClip>("End");
        Blood= Resources.Load<AudioClip>("BloodSound");
        Stab= Resources.Load<AudioClip>("Stab");
        Slash= Resources.Load<AudioClip>("Slash");
        BossScream= Resources.Load<AudioClip>("BossScream");
        
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = true;
        audioSrc.clip= Cinematic;
        audioSrc.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string sound)
    {
        switch(sound){
            case "TheEnd":
            audioSrc.clip=End;
            audioSrc.Play();
            break;
        }
    }

}
