using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int canvasBeenShownCurr = 0;
    [SerializeField] int canvasBeenShownPrev = 0;
    [SerializeField] GameObject canvasGameTutorial; // Number 0
    [SerializeField] GameObject canvasGameInfo;     // Number 1
    [SerializeField] GameObject canvasOptionMenu;   // Number 2
    [SerializeField] GameObject canvasDeadMenu;     // Number 3

    public static GameManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    void Start()
    {
        ActivateCanvasNumber(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasBeenShownPrev != canvasBeenShownCurr){
            ActivateCanvasNumber(canvasBeenShownCurr);
        }

        if (canvasBeenShownCurr != 1){
            Time.timeScale = 0.01F;
        }
    }

    void ActivateCanvasNumber(int canvasNumber)
    {
        var canvas1 = canvasGameTutorial;
        var canvas2 = canvasGameInfo;
        var canvas3 = canvasOptionMenu;
        var canvas4 = canvasDeadMenu;

        canvasBeenShownPrev = canvasNumber;
        switch (canvasNumber)
        {
            case 0:
                // Activate canvas 1
                Debug.Log("Activating canvas "+ canvas1.name);
                canvas1.SetActive(true);
                canvas2.SetActive(false);
                canvas3.SetActive(false);
                canvas4.SetActive(false);
                break; // Break out of the switch statement

            case 1:
                // Activate canvas 1
                Debug.Log("Activating canvas "+ canvas2.name);
                canvas1.SetActive(false);
                canvas2.SetActive(true);
                canvas3.SetActive(false);
                canvas4.SetActive(false);
                break; // Break out of the switch statement

            case 2:
                // Activate canvas 1
                Debug.Log("Activating canvas "+ canvas3.name);
                canvas1.SetActive(false);
                canvas2.SetActive(false);
                canvas3.SetActive(true);
                canvas4.SetActive(false);
                break; // Break out of the switch statement

            case 3:
                // Activate canvas 1
                Debug.Log("Activating canvas "+ canvas4.name);
                canvas1.SetActive(false);
                canvas2.SetActive(false);
                canvas3.SetActive(false);
                canvas4.SetActive(true);
                break; // Break out of the switch statement

            default:
                // Invalid canvas number
                Debug.Log($"Invalid canvas number: {canvasNumber}");
                canvas1.SetActive(false);
                canvas2.SetActive(false);
                canvas3.SetActive(false);
                canvas4.SetActive(false);
                break; // Break out of the switch statement
        }
    }
}
