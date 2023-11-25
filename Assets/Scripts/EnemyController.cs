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
    [SerializeField] GameObject player;

    [SerializeField] GameObject blood;

    [SerializeField] GameObject eyeLevel;

    public GameObject target;

    private float distanceFromPlayer;
    public float distance=120f;
    
    private float checkingAngle;

    public float angle = 0.1f;

    private float checkingTimer=0.5f;


    private int direction=1;




    #region Readonly Properties
    public NavMeshAgent agent {private set; get;}
    #endregion

    

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        
        animator = GetComponent<Animator>();

        checkingAngle=angle;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(player.transform.position.x>=0){
            distanceFromPlayer = 1f;
        }else{
            distanceFromPlayer = -1f;
        }

        Ray ray;
        RaycastHit hit;
        
        if(!eyeLevel.GetComponent<FieldOfView>().CanSeePlayer){
            if(checkingTimer>0f ){
                checkingTimer-=Time.deltaTime;
                gameObject.transform.eulerAngles=new Vector3(
                gameObject.transform.eulerAngles.x,
                gameObject.transform.eulerAngles.y+checkingAngle,
                gameObject.transform.eulerAngles.z
                );
            }else if( checkingTimer<=0f){
                checkingTimer=0.5f;
                checkingAngle=-1f*angle;
            }
        } 
        

        if (eyeLevel.GetComponent<FieldOfView>().CanSeePlayer == true){
            Debug.Log("mirandolo");
                agent.SetDestination(
                    new Vector3(
                        player.transform.position.x+10f,
                        player.transform.position.y,
                        player.transform.position.z
                    )
                );
                
                animator.SetBool("isRunning",true);
        }else if(eyeLevel.GetComponent<FieldOfView>().CanSeePlayer == false){
                agent.SetDestination(
                    new Vector3(
                        gameObject.transform.position.x,
                        gameObject.transform.position.y,
                        gameObject.transform.position.z
                    )
                );
                animator.SetBool("isRunning",false);
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
        
        }
    }

}
