using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SFB;
using Photon.Pun;



public class FileManager : MonoBehaviourPunCallbacks
{
    
    public VideoPlayer videoPlayer;
    public GameObject videoPlayerObject;

    private ExtensionFilter[] extensions = new[] {
        new ExtensionFilter("Videos", "mp4"),
        new ExtensionFilter("Other", "")

    };
    public void Start()
    {
        if (photonView.IsMine) LocalStart();
        else OtherStart();
    }

    void LocalStart()
    {
        videoPlayerObject = GameObject.FindGameObjectWithTag("videoPlayerObject");
        videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
    }

    void OtherStart()
    {
        videoPlayerObject = GameObject.FindGameObjectWithTag("videoPlayerObject");
        videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
    }
    public void OpenFileBrowser()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open file", "", extensions, false);
        if (paths != null)
        {
            videoPlayer.url = paths[0];
            photonView.RPC("ReceiveVideoUrl", RpcTarget.Others, videoPlayer.url);
        }
        if(paths == null)
        {
            Debug.Log("No se ha cargado ningun video");
        }


    } 
    //Falla aquí, a la hora de cambiar cosas, y tendre que ver si puede estar encendido, creo que es porque claro, el solo
    //identifica al primero, al segundo no le hace ni caso lo del Path = videoPlayer.url
    [PunRPC]
    private void ReceiveVideoUrl(string videoUrl)
    {
        Debug.Log($"RPC received with video URL: {videoUrl}");
        videoPlayer.url = videoUrl;
        
    }

    public void Update()
    {
        
        
    }

    public void StopVideo()
    { 
        videoPlayer.Stop();
        videoPlayer.url = null;
    }


}
