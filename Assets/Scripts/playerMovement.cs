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
    private float RotationSpeed = 15f;
    private float MovementSpeed = 3;

    private bool isAttacking = false;
    private bool isRunning = false;
    private float attackTimer=0f;
    private Vector2 lastMovement; 

    [SerializeField] private GameObject player;
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
        
    }

    // Update is called once per frame
    void Update()
    {
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
        animator.SetBool("isAttackingLight",true);
        isAttacking=true;
        attackTimer=1.5f;
    }

    public void OnAttackHeavy(InputValue value){
        animator.SetBool("isAttackingHeavy",true);
        isAttacking=true;
        attackTimer=1.5f;
    }
}
