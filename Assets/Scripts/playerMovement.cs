using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
public class playerMovement : MonoBehaviour
{
    private Vector3 direction = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    public Animator animator;
    private CharacterController characterController;
    private float RotationSpeed = 80f;
    private float MovementSpeed = 15;

    public float health = 1f;

    public bool isAttacking = false;

    public int typeAttack;
    public bool isRunning = false;
    public float attackTimer=0f;
    private Vector2 lastMovement; 

    [SerializeField] private GameObject player;

    [SerializeField] public GameObject healthBar;
    private Transform myCamera;

     /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myCamera = transform.Find("Main Camera");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar.GetComponent<Slider>().value=health;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<Slider>().value=health;
        Debug.Log(health);

        if(lastMovement.x==0 && lastMovement.y==0){
            isRunning=false;
        }
        // Movimiento
        characterController.Move(
            transform.forward * direction.normalized.z * Time.deltaTime * MovementSpeed
            + transform.right * direction.normalized.x * Time.deltaTime * MovementSpeed
        );

        // Rotacion Horizontal
        transform.Rotate(
            0f,
            rotation.y * RotationSpeed * Time.deltaTime,
            0f
        );
        var rotationAngle = -rotation.x * RotationSpeed * Time.deltaTime;
        
        characterController.Move(Vector3.down * 9.82f * Time.deltaTime);

        if(attackTimer>0f){
            attackTimer-=Time.deltaTime;
        }else{
            attackTimer=0f;
            animator.SetBool("isAttackingLight",false);
            animator.SetBool("isAttackingHeavy",false);
            isAttacking=false;
            typeAttack=0;
        }

        if(isRunning){
            animator.SetBool("isRunning",true);
        }else{
            animator.SetBool("isRunning",false);
        }

    }

    public void OnMove(InputValue value){
        if(!isAttacking){
            Debug.Log(value.Get<Vector2>());
            var data = value.Get<Vector2>();
            lastMovement=value.Get<Vector2>();
            direction = new Vector3(
                data.x,
                0f,
                data.y
            );
            rotation = new Vector3(
                data.y,
                data.x, // rotacion horizontal (sobre eje Y)
                0f
            );
            
            isRunning=true;
        }
        
    }


    private void OnLook(InputValue value)
    {
        var data = value.Get<Vector2>();
        rotation = new Vector3(
            data.y,
            data.x, // rotacion horizontal (sobre eje Y)
            0f
        );
    }

    public void OnAttackLight(InputValue value){
        if(!isRunning){
            animator.SetBool("isAttackingLight",true);
            isAttacking=true;
            attackTimer=1.5f;
            typeAttack=1;
        }
        
    }

    public void OnAttackHeavy(InputValue value){
        if(!isRunning){
            animator.SetBool("isAttackingHeavy",true);
            isAttacking=true;
            attackTimer=1.5f;
            typeAttack=2;
        }
        
    }
}
