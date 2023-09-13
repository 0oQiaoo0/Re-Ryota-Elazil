using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartGame : MonoBehaviour
{
    public VideoPlayer _videoPlayer;

    public GameObject startBg;

    public GameObject mainBgm;
    public AudioSource doorBgm;

    public Image mask;

    void Start()
    {
        _videoPlayer.loopPointReached += OnVideoOver;
        startBg.gameObject.SetActive(false);
    }

    public void OnStartGame()
    { 
        doorBgm.gameObject.SetActive(true);
        doorBgm.Play();

     
        StartCoroutine(DelayStart());
    }

    private IEnumerator ShowMask(float time)
    {
        mask.gameObject.SetActive(true);
        float totalTime = time;
        while (time > 0)
        {
            var color = mask.color;
            color.a = Mathf.Lerp(1,0,time / totalTime);
            mask.color = color;
            time -= Time.deltaTime;
            yield return null;
        }
        
    }

    private IEnumerator DelayStart()
    {       
        StartCoroutine(ShowMask(doorBgm.clip.length));
        yield return new WaitForSeconds(doorBgm.clip.length);
        
        
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            SceneManager.SetActiveScene(scene);
        };
        SceneManager.LoadScene("gameScene");
        
    }
    
    public void OnExitGame()
    {
        Application.Quit();
    }
    
    private void OnDestroy()
    {
        if (_videoPlayer == null)
            return;
        
        _videoPlayer.loopPointReached -= OnVideoOver;
    }

    public void OnVideoOver(VideoPlayer v)
    {
        startBg.gameObject.SetActive(true);
        _videoPlayer.gameObject.SetActive(false);
    }

 
}
