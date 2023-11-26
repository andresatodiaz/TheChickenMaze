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




    #region Readonly Properties
    public NavMeshAgent agent {private set; get;}
    #endregion

    

    // Start is called before the first frame update
    void Start()
    {

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
        if(health<=0f){
            gameObject.SetActive(false);
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
         

        if (eyeLevel.GetComponent<FieldOfView>().CanSeePlayer == true){
            Debug.Log("mirandolo");
            Debug.Log(transform.position.x-player.transform.position.x);
                if((transform.position.x-player.transform.position.x>=0 && transform.position.x-player.transform.position.x<=6f) || (transform.position.x-player.transform.position.x<0 && transform.position.x-player.transform.position.x>=-6f)
                && ((transform.position.z-player.transform.position.z>=0 && transform.position.x-player.transform.position.z<=6f) || (transform.position.x-player.transform.position.z<0 && transform.position.x-player.transform.position.z>=-6f))){
                    animator.SetBool("isShooting",true);
                    animator.SetBool("isRunning",false);
                    GameObject temp = Resources.Load<GameObject>("BulletCollision");
                    GameObject gameObjectReference = Instantiate(temp,explosion.transform.position,explosion.transform.rotation) as GameObject;
                    gameObjectReference.transform.parent = gameObject.transform;
                    gameObjectReference.transform.rotation= gameObject.transform.rotation;
                    Destroy(gameObjectReference, 0.1f);
                    player.GetComponent<playerMovement>().health-=0.01f;
                    onPersuit=true;
                    eyeLevel.GetComponent<FieldOfView>().onPersuit=true;
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
                    player.GetComponent<playerMovement>().health-=0.01f;
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
                    health-=15f;
                    onPersuit=true;
            }else if(player.GetComponent<playerMovement>().typeAttack==1 && player.GetComponent<playerMovement>().isAttacking){
                    health-=5f;
                    onPersuit=true;
            } 
        }
    }

}
