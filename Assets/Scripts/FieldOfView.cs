using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FieldOfView : MonoBehaviour
{
    public float radius  = 10f;
    [Range(1,360)]public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    
    public bool CanSeePlayer;

    [SerializeField] GameObject playerRef;

    [SerializeField] GameObject eyeLevel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FOVCheck());
        
    }

    private IEnumerator FOVCheck(){
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while(true){
            yield return wait;
            FOV();
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


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        
    }
}
