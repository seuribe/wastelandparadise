using UnityEngine;
using System.Collections;
using System;

public class GameController : MonoBehaviour {

    public PoorSoulSpawner spawner;
    public PreacherController preacher;

    public float totalTime = 90;
    private float playTime;

    private int killedSouls = 0;
    private int savedSouls = 0;

    private const int STATE_NOT_STARTED     = 0;
    private const int STATE_PLAYING         = 1;
    private const int STATE_FINISHED        = 2;

    private int state = STATE_NOT_STARTED;

    public int GameState
    {
        get { return state; }
    }
    public bool IsPlaying
    {
        get { return state == STATE_PLAYING; }
    }

	// Use this for initialization
	void Start () {
        spawner.Pause();
	}

    void EndGame()
    {
        Debug.Log("End Game");
        state = STATE_FINISHED;
        spawner.Pause();
    }

	// Update is called once per frame
	void Update ()
    {
	    switch (state)
        {
            case STATE_NOT_STARTED:
                break;
            case STATE_PLAYING:
                playTime += Time.deltaTime;
                float timeLeft = totalTime - playTime;
                if (timeLeft <= 0)
                {
                    EndGame();
                }
                break;
            case STATE_FINISHED:
                break;
        }
	}

    private void StartPlaying()
    {
        Debug.Log("Start Playing");
        savedSouls = 0;
        killedSouls = 0;
        playTime = 0;
        spawner.Resume();
        state = STATE_PLAYING;
    }

    void OnGUI()
    {
        switch (state)
        {
            case STATE_NOT_STARTED: // display start button
                if (GUI.Button(new Rect(Screen.width/2 - 150, Screen.height/2 - 100, 300, 200), "START")) {
                    StartPlaying();
                }
                break;
            case STATE_PLAYING: // display time
                GUI.TextField(new Rect(10, 10, 150, 20), "Sent to heaven: " + savedSouls);
                GUI.TextField(new Rect(10, 40, 150, 20), "Sent to hell: " + killedSouls);
                GUI.TextField(new Rect(Screen.width - 200, 10, 180, 30), "Seconds left: " + (int)(totalTime - playTime));
                break;
            case STATE_FINISHED: // display results
                if (GUI.Button(new Rect(Screen.width/2 - 150, Screen.height/2 - 100, 300, 200), "START")) {
                    StartPlaying();
                }
                GUI.TextField(new Rect(10, 10, 150, 20), "Sent to heaven: " + savedSouls);
                GUI.TextField(new Rect(10, 40, 150, 20), "Sent to hell: " + killedSouls);
                break;
        }
    }

    internal void SoulSaved()
    {
        savedSouls++;
    }

    internal void SoulLost()
    {
        killedSouls++;
    }
}
