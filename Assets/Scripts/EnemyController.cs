using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public Animator animator;

    public float health = 100f;

    public float damage = 5f;
    [SerializeField] GameObject player;

    [SerializeField] GameObject blood;
    [SerializeField] GameObject explosion;

    [SerializeField] GameObject eyeLevel;

    public GameObject target;

    private float distanceFromPlayer;
    public float distance=120f;
    
    private float checkingAngle;

    public float angle = 0.1f;

    private float checkingTimer=0.5f;


    private int direction=1;

        // How long it takes to go from eulerAngles1 to eulerAngles2
    public float duration=1f;

    private bool onPersuit=false;

    Quaternion rotation1;
    Quaternion rotation2;

    public static AudioClip Blood, Shoot;
    AudioSource audioSrc;

    private bool isShooting;

    private float shootingTimer;

    private bool dying=false;

    private bool shooting=false;




    #region Readonly Properties
    public NavMeshAgent agent {private set; get;}
    #endregion

    

    // Start is called before the first frame update
    void Start()
    {
        Shoot = Resources.Load<AudioClip>("Shoot");

        Blood = Resources.Load<AudioClip>("Crushed");


        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = 0.2f;

        agent = GetComponent<NavMeshAgent>();
        
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
        if(health<30f){
            onPersuit=false;
            audioSrc.PlayOneShot(Blood,1f);
            
            if(health<=0){
                GameObject temp = Resources.Load<GameObject>("Explosion");
                GameObject gameObjectReference = Instantiate(temp,gameObject.transform.position,gameObject.transform.rotation) as GameObject;
                Destroy (gameObjectReference, 1.0f);
                player.GetComponent<playerMovement>().euforia+=0.05f;
                gameObject.SetActive(false);
            }
        }

        if(player.transform.position.x>=0){
            distanceFromPlayer = 1f;
        }else{
            distanceFromPlayer = -1f;
        }
        
        if(eyeLevel.GetComponent<FieldOfView>().CanSeePlayer==false && onPersuit!=true){

             var factor = Mathf.PingPong(Time.time / duration, 1);
            // Optionally you can even add some ease-in and -out
            factor = Mathf.SmoothStep(0, 1, factor);

            // Now interpolate between the two rotations on the current factor
            transform.rotation = Quaternion.Slerp(rotation1, rotation2, factor);  

        }  

        if(shootingTimer>0f){
            shootingTimer=-Time.deltaTime;
            audioSrc.PlayOneShot(Shoot,2f);
        }else if(health<30f){
            audioSrc.Stop();
        }else{
            audioSrc.Stop();
        }
        
         
        if(player.GetComponent<playerMovement>().IceTimer<=0f){
            if (eyeLevel.GetComponent<FieldOfView>().CanSeePlayer == true){
                Debug.Log("mirandolo");
                Debug.Log(transform.position.x-player.transform.position.x);
                    if((transform.position.x-player.transform.position.x>=0 && transform.position.x-player.transform.position.x<=6f) || (transform.position.x-player.transform.position.x<0 && transform.position.x-player.transform.position.x>=-6f)
                    && ((transform.position.z-player.transform.position.z>=0 && transform.position.x-player.transform.position.z<=6f) || (transform.position.x-player.transform.position.z<0 && transform.position.x-player.transform.position.z>=-6f))){
                        animator.SetBool("isShooting",true);
                        animator.SetBool("isRunning",false);
                        shooting=true;
                        GameObject temp = Resources.Load<GameObject>("BulletCollision");
                        GameObject gameObjectReference = Instantiate(temp,explosion.transform.position,explosion.transform.rotation) as GameObject;
                        gameObjectReference.transform.parent = gameObject.transform;
                        gameObjectReference.transform.rotation= gameObject.transform.rotation;
                        Destroy(gameObjectReference, 0.1f);
                        player.GetComponent<playerMovement>().health-=0.005f;
                        onPersuit=true;
                        eyeLevel.GetComponent<FieldOfView>().onPersuit=true;
                        shootingTimer=1.5f;
                        agent.isStopped = true;
                        agent.ResetPath();
                    }else{
                        Debug.Log("Perseguir");
                        agent.SetDestination(
                            new Vector3(
                                player.transform.position.x+2f,
                                player.transform.position.y,
                                player.transform.position.z
                            )
                        );
                        transform.forward=player.transform.forward*-1;
                        animator.SetBool("isRunning",true);
                        animator.SetBool("isShooting",false);
                        onPersuit=true;
                        eyeLevel.GetComponent<FieldOfView>().onPersuit=true;
                        shooting=false;
                    }
                    
            }else if(eyeLevel.GetComponent<FieldOfView>().CanSeePlayer == false){
                    if(onPersuit!=true){
                        animator.SetBool("isRunning",false);
                        animator.SetBool("isShooting",false);
                    }else{
                        animator.SetBool("isRunning",true);
                        animator.SetBool("isShooting",false);
                        agent.SetDestination(
                            new Vector3(
                                player.transform.position.x+2f,
                                player.transform.position.y,
                                player.transform.position.z
                            )
                        );
                        shooting=false;
                    }
            }

            if(onPersuit){
                
                if(((transform.position.x-player.transform.position.x>=0 && transform.position.x-player.transform.position.x<=6f) || (transform.position.x-player.transform.position.x<0 && transform.position.x-player.transform.position.x>=-6f)) 
                && ((transform.position.z-player.transform.position.z>=0 && transform.position.x-player.transform.position.z<=6f) || (transform.position.x-player.transform.position.z<0 && transform.position.x-player.transform.position.z>=-6f))){
                        animator.SetBool("isShooting",true);
                        animator.SetBool("isRunning",false);
                        GameObject temp = Resources.Load<GameObject>("BulletCollision");
                        GameObject gameObjectReference = Instantiate(temp,explosion.transform.position,explosion.transform.rotation) as GameObject;
                        gameObjectReference.transform.parent = gameObject.transform;
                        gameObjectReference.transform.rotation= gameObject.transform.rotation;
                        Destroy(gameObjectReference, 0.1f);
                        player.GetComponent<playerMovement>().health-=0.005f;
                        shootingTimer=1.5f;
                        agent.isStopped = true;
                        agent.ResetPath();
                }else{
                    Debug.Log("Perseguir");
                    agent.SetDestination(
                            new Vector3(
                                player.transform.position.x+2f,
                                player.transform.position.y,
                                player.transform.position.z
                            )
                    );
                    transform.forward=player.transform.forward*-1;
                    animator.SetBool("isRunning",true);
                    animator.SetBool("isShooting",false);
                    onPersuit=true;
                    eyeLevel.GetComponent<FieldOfView>().onPersuit=true;
                }
            }
        }
        
    }

    void Follow(){
        agent.SetDestination(
            new Vector3(
                player.transform.position.x+2f,
                player.transform.position.y,
                player.transform.position.z
            )
        );
        animator.SetBool("isRunning",true);
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
                        health-=60f;
                    }else{
                        health-=100f;
                    }
                    
                    onPersuit=true;
            }else if(player.GetComponent<playerMovement>().typeAttack==1 && player.GetComponent<playerMovement>().isAttacking){
                    
                    if(player.GetComponent<playerMovement>().EuforiaTimer<=0f){
                        health-=15f;
                    }else{
                        health-=100f;
                    }
                    onPersuit=true;
            } 
        }

        if(other.transform.CompareTag("Player")){
            onPersuit=true;
        }
    }

}
