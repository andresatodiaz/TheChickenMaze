using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadingController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject LoadingVideo;
    public GameObject LoadingImages;
    public VideoPlayer videoPlay;

    void Start()
    {
        videoPlay.loopPointReached += VideoFinished;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void VideoFinished(VideoPlayer vp)
    {
        Debug.Log("TERMINO EL VIDEO");
        LoadingVideo.SetActive(false);
        LoadingImages.SetActive(true);
    }
}
