using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SFB;



public class FileManager : MonoBehaviour
{
    
    public VideoPlayer videoPlayer;
    public string Path;

    private ExtensionFilter[] extensions = new[] {
        new ExtensionFilter("Videos", "mp4"),
        new ExtensionFilter("Other", "")

    };
    public void OpenFileBrowser()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open file", "", extensions,false);
        //GetVideo();
        videoPlayer.url = paths[0];
    } 


    void GetVideo()
    {
        if(Path != null)
        {
            UpdateVideo();
        }
    }

    void UpdateVideo()
    {
        videoPlayer.url = Path;
    }

    
}
