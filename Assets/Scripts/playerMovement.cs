using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using TMPro;
public class playerMovement : MonoBehaviour
{
    private Vector3 direction = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    public Animator animator;
    private CharacterController characterController;
    private float RotationSpeed = 80f;
    private float MovementSpeed = 15;

    public float health = 1f;

    public float euforia = 0f;

    public bool isAttacking = false;

    public int typeAttack;
    public bool isRunning = false;
    public float attackTimer=0f;
    private Vector2 lastMovement; 

    [SerializeField] private GameObject player;

    [SerializeField] public GameObject healthBar;
    [SerializeField] public GameObject euforiaBar;

    [SerializeField] public GameObject SoundManager;

    [SerializeField] public GameObject EndWall;
    [SerializeField] public Transform myCamera;

    [SerializeField] public GameObject IceEffect;
    [SerializeField] public GameObject EuforiaEffect;

    public float IceTimer;

    public float EuforiaTimer=0f;

    [SerializeField] TextMeshProUGUI m_Object;

    private Quaternion currentRotation;

    private int intEuforia;
    

    AudioSource audioSrc;
    public static AudioClip Slash, Stab, Crushed;

     /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        EuforiaEffect.SetActive(false);
        IceEffect.SetActive(false);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //myCamera = transform.Find("Main Camera");
        audioSrc = GetComponent<AudioSource>();
        Stab= Resources.Load<AudioClip>("Stab");
        Slash= Resources.Load<AudioClip>("Slash");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        healthBar.GetComponent<Slider>().value=health;
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<Slider>().value=health;
        euforiaBar.GetComponent<Slider>().value=euforia;
        m_Object.text=(euforia*100).ToString("R");

        if(EuforiaTimer>0f){
            EuforiaTimer-=Time.deltaTime;
        }else{
            EuforiaTimer=0f;
            EuforiaEffect.SetActive(false);
            MovementSpeed=15;
        }

        if(IceTimer>0f){
            IceTimer-=Time.deltaTime;
        }else{
            IceTimer=0f;
            IceEffect.SetActive(false);
        }

        if(lastMovement.x==0 && lastMovement.y==0){
            isRunning=false;
        }
        // Movimiento
        

        // Rotacion Horizontal
        if(attackTimer==0){
            characterController.Move(
            transform.forward * direction.normalized.z * Time.deltaTime * MovementSpeed
            + transform.right * direction.normalized.x * Time.deltaTime * MovementSpeed
            );
            transform.Rotate(
            0f,
            rotation.y * RotationSpeed * Time.deltaTime,
            0f
            );
            var rotationAngle = -rotation.y * RotationSpeed * Time.deltaTime;
            characterController.Move(Vector3.down * 9.82f * Time.deltaTime);
        }
        
        

        /* myCamera.Rotate(
            0f, //TODO: Clamp
            rotationAngle,
            0f
        ); */
        
        

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
            attackTimer=0.2f;
            typeAttack=1;
            audioSrc.PlayOneShot(Stab,0.5f);
        }
        
    }

    public void OnAttackHeavy(InputValue value){
        if(!isRunning){
            animator.SetBool("isAttackingHeavy",true);
            isAttacking=true;
            attackTimer=0.75f;
            typeAttack=2;
            audioSrc.PlayOneShot(Slash,0.5f);
        }
        
    }

    public void OnEuforia(InputValue value){
        if(euforia-0.5f>=0f){
            EuforiaTimer=10f;
            EuforiaEffect.SetActive(true);
            MovementSpeed=35;
            euforia-=0.5f;
        }
    }

    public void OnIce(InputValue value){
        if(euforia-0.2f>=0f){
            IceTimer=10f;
            IceEffect.SetActive(true);
            euforia-=0.2f;
        }
    }

    

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag("TheEnd")){
            //Time.timeScale = 0;
            SoundManager.GetComponent<SoundManagerScript>().PlaySound("TheEnd");
            EndWall.SetActive(false);
        }
    }

}
