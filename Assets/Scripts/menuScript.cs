using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {

    public Transform pauseCanvas;
    public Transform startCanvas;
    public Transform inGameCanvas;
    public Transform winLoseCanvas;
    AudioSource As;
    public AudioClip a1;
    private bool paused = false;

    void Start()
    {
        As = gameObject.GetComponent<AudioSource>();

        As.clip = a1;
        
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && winLoseCanvas.gameObject.activeInHierarchy == false)
        {
            pause();
        }
        if (Input.GetKeyDown(KeyCode.M) && !paused)
        {
            As.Pause();
            paused = !paused;
        }
        else if(Input.GetKeyDown(KeyCode.M) && paused)
        {
            As.UnPause();
            paused = !paused;
        }
	}
    public void pause()
    {
        if (pauseCanvas.gameObject.activeInHierarchy == false)
        {
            pauseCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            As.Pause();
        }
        else
        {
            pauseCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            As.UnPause();
        }

    }
    public void resume()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        As.UnPause();
    }
    public void start()
    {
        startCanvas.gameObject.SetActive(false);
        inGameCanvas.gameObject.SetActive(true);
        Time.timeScale = 1;
        As.PlayDelayed(0.3f);
    }
    public void reStart(){

        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);

    }
    public void close()
    {
        Application.Quit();
    }
}
