using UnityEngine;
using System.Collections;

public class PreacherController : MonoBehaviour {

    public KeyCode killKey = KeyCode.K;
    public KeyCode preachKey = KeyCode.P;
    public KeyCode reprimendKey = KeyCode.R;
    public KeyCode confessionKey = KeyCode.C;
    public KeyCode absolveKey = KeyCode.V;

    public Collider actionArea;

	// Use this for initialization
	void Start () {
	
	}

    private PoorSoulController availableSoul; 

    void OnTriggerEnter(Collider collider)
    {
        GameObject go = collider.gameObject;
        PoorSoulController soul = go.GetComponentInChildren<PoorSoulController>();
        if (soul != null)
        {
            availableSoul = soul;
            Debug.Log("Poor soul available!");
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (availableSoul != null)
        {
            Debug.Log("Poor soul lost");
        }
        availableSoul = null;
    }

    private void Kill()
    {
    }

    private void Preach()
    {
    }

    private void Reprimend()
    {
    }

    private void Confess()
    {
    }

    private void Absolve()
    {
    }

    private bool IsPoorSoulReachable()
    {
        return availableSoul != null;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(killKey))
        {
            Kill();
        }
        else if (Input.GetKeyDown(preachKey))
        {
            Preach();
        }
        else if (Input.GetKeyDown(reprimendKey))
        {
            Reprimend();
        }
        else if (Input.GetKeyDown(confessionKey))
        {
            Confess();
        }
        else if (Input.GetKeyDown(absolveKey))
        {
            Absolve();
        }
    }
}
