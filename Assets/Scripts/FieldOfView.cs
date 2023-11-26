using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FieldOfView : MonoBehaviour
{
    public float radius  = 100f;
    [Range(1,360)]public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    
    public bool CanSeePlayer;

    [SerializeField] GameObject playerRef;

    [SerializeField] GameObject eyeLevel;
    private Vector3 targetDirection;

    public bool onPersuit=false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FOVCheck());
        
    }

    private IEnumerator FOVCheck(){
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while(true){
            yield return wait;
            //FOV();
            FOV2();
        }
    }

    private void FOV(){
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position,radius, targetLayer);
            
        if(rangeCheck.Length != 0){
            Transform target = rangeCheck[0].transform;

            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Debug.Log(directionToTarget);

            if(Vector3.Angle(transform.position,directionToTarget) < angle/2){
                
                float distanceToTarget=Vector3.Distance(transform.position,target.position);

                if(!Physics.Raycast(transform.position,directionToTarget,distanceToTarget,obstructionLayer)){
                    CanSeePlayer=true;
                    Debug.Log("uno");
                }else{
                    Debug.Log("dos");
                    CanSeePlayer=false;
                }

            }else{
                CanSeePlayer=false;
                Debug.Log("tres");
            }
        }else if(CanSeePlayer){
            CanSeePlayer=false;
        }
    }  

    private void FOV2(){
        // Get the angle between the forward direction and the target direction
        if(onPersuit){
            targetDirection = playerRef.transform.forward;
            Debug.Log("onPersuit");
        }else{
            targetDirection = transform.forward;
        }
        
        float angleToTarget = Vector3.Angle(transform.forward, targetDirection);

        // Check if the target is within the field of view
        if (angleToTarget < angle )
        {
            // Cast a ray from the observer to the target
            RaycastHit hit;
            if (Physics.Raycast(transform.position, targetDirection, out hit, Mathf.Infinity))
            {   
                Debug.DrawRay(transform.position, targetDirection * hit.distance, Color.yellow);
                // Check if the raycast hit the target
                if (hit.collider.gameObject == playerRef)
                {
                    // The target is in sight!
                    Debug.Log("Target in sight!");
                    CanSeePlayer=true;
                }else{
                    CanSeePlayer=false;
                }
            }else{
                CanSeePlayer=false;
            }
        }else{
            CanSeePlayer=false;
        }
       
    } 


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        
    }
}
