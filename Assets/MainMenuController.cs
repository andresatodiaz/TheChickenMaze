using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject FirstCinematic;
    public VideoPlayer videoPlay;
    private bool Skip = false;
    private bool pressE = false;
    // Start is called before the first frame update
    void Start()
    {
        videoPlay.loopPointReached += VideoFinished;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("ENTER");
            if (Skip == false)
            {
                FirstCinematic.SetActive(false);
                MainMenu.SetActive(true);
                Skip = true;
                pressE = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pressE == true)
            {
                Debug.Log("Saltar a la siguiente escena");
                pressE = false;
            }
        }
        

    }
    void VideoFinished(VideoPlayer vp)
    {
        Debug.Log("TERMINO EL VIDEO");
        FirstCinematic.SetActive(false);
        MainMenu.SetActive(true);
        Skip = true;
        pressE = true;
    }
}
