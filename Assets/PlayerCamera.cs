using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] GameObject player;
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;

    public float distance=20f;

    void Start()
    {
        target= player.transform;
    }

    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 1, -5);
    }
}
