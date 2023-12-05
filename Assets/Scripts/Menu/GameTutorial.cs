using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishTutorial()
    {
        GameManager.Instance.canvasBeenShownCurr = 1; // In GameManager: GameObject canvasGameInfo -> Number 1
    }
}
