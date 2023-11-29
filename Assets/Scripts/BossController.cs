using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
public class BossController : MonoBehaviour
{
    [SerializeField] GameObject eyeLevel;
    [SerializeField] GameObject blood;

    [SerializeField] GameObject player;

    [SerializeField] GameObject bossInfo;

    public Animator animator;

    public float health = 100f;

    public float damage = 5f;

    private bool onPersuit=false;

     private float checkingAngle;

    public float angle = 0.1f;
    public float duration=1f;

    private bool isAttacking;

    private float attackTimer=1.2f;

    [SerializeField] public GameObject healthBar;

    public static AudioClip BossScream;
    AudioSource audioSrc;


    Quaternion rotation1;
    Quaternion rotation2;

     public int soundwait = 10;
	bool keepPlaying=true;

    #region Readonly Properties
    public UnityEngine.AI.NavMeshAgent agent {private set; get;}
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        BossScream= Resources.Load<AudioClip>("BossScream");
        StartCoroutine(SoundOut());
        bossInfo.SetActive(false);
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
        animator = GetComponent<Animator>();

        checkingAngle=angle;

        rotation1 = Quaternion.Euler(
            new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                transform.eulerAngles.z
            )
        );
        rotation2 = Quaternion.Euler(
            new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y+60f,
                transform.eulerAngles.z
            )
        );
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<Slider>().value=health/100f;

        if(health<=0f){
            GameObject temp = Resources.Load<GameObject>("Explosion");
            GameObject gameObjectReference = Instantiate(temp,gameObject.transform.position,gameObject.transform.rotation) as GameObject;
            Destroy (gameObjectReference, 1.0f);
            gameObject.SetActive(false);
        }

        if(attackTimer>0f){
            attackTimer-=Time.deltaTime;
        }else{
            attackTimer=0f;
            isAttacking=false;
        }

        /* if(eyeLevel.GetComponent<FieldOfView>().CanSeePlayer==false && onPersuit!=true){

             var factor = Mathf.PingPong(Time.time / duration, 1);
            // Optionally you can even add some ease-in and -out
            factor = Mathf.SmoothStep(0, 1, factor);

            // Now interpolate between the two rotations on the current factor
            transform.rotation = Quaternion.Slerp(rotation1, rotation2, factor);  

        }  */ 
        if(onPersuit){
            Vector3 newDir = Quaternion.Euler(0, 2.0f, 0) * transform.forward;
            transform.forward=newDir;  
        }
              
        if(player.GetComponent<playerMovement>().IceTimer<=0f){
            if (eyeLevel.GetComponent<FieldOfView>().CanSeePlayer == true){
                bossInfo.SetActive(true);
                animator.SetBool("isRunning",true);
                animator.SetBool("isAttacking",false);
                agent.SetDestination(
                            new Vector3(
                                player.transform.position.x,
                                player.transform.position.y,
                                player.transform.position.z
                            )
                        );
                onPersuit=true;
                eyeLevel.GetComponent<FieldOfView>().onPersuit=true;
            }else{
                if(!isAttacking){
                    if(!onPersuit){
                    animator.SetBool("isRunning",false);
                    animator.SetBool("isAttacking",false);
                    }else{
                        animator.SetBool("isRunning",true);
                        animator.SetBool("isAttacking",false);
                    }
                }
            }

            if(onPersuit){
                if(!isAttacking){
                    agent.SetDestination(
                            new Vector3(
                                player.transform.position.x,
                                player.transform.position.y,
                                player.transform.position.z
                            )
                );

                animator.SetBool("isRunning",true);
                animator.SetBool("isAttacking",false);
                }
            }
        }

        
    }

    void OnCollisionEnter(Collision other)
    {
       
        if((other.transform.CompareTag("Sword") || other.transform.CompareTag("Shield") ) && player.GetComponent<playerMovement>().isAttacking){
             Debug.Log(other.transform.position);
            GameObject temp = Resources.Load<GameObject>("Blood");
            GameObject gameObjectReference = Instantiate(temp,gameObject.transform.position,gameObject.transform.rotation) as GameObject;
            gameObjectReference.transform.parent = gameObject.transform;
            gameObjectReference.transform.position =  new Vector3(
                gameObject.transform.position.x,
                other.transform.position.y+0.3f,
                gameObject.transform.position.z
            );
            gameObjectReference.transform.rotation= Quaternion.Inverse(other.transform.rotation);

            if(player.GetComponent<playerMovement>().typeAttack==2 && player.GetComponent<playerMovement>().isAttacking){
                    if(player.GetComponent<playerMovement>().EuforiaTimer<=0f){
                        health-=2f;
                        onPersuit=true;
                    }else{
                        health-=4f;
                        onPersuit=true;
                    }
                    
            }else if(player.GetComponent<playerMovement>().typeAttack==1 && player.GetComponent<playerMovement>().isAttacking){
                    
                    if(player.GetComponent<playerMovement>().EuforiaTimer<=0f){
                        health-=1f;
                        onPersuit=true;
                    }else{
                        health-=2f;
                        onPersuit=true;
                    }
            } 
        }

        if(other.transform.CompareTag("Player")){
            onPersuit=true;
            isAttacking=true;
            attackTimer=1.2f;
            animator.SetBool("isRunning",false);
            animator.SetBool("isAttacking",true);
            player.GetComponent<playerMovement>().health-=0.1f;
        }
    }
    IEnumerator SoundOut()
	{
		while (keepPlaying){
        	GetComponent<AudioSource>().PlayOneShot(BossScream);  
			yield return new WaitForSeconds(soundwait);
		}
	}
}
