using UnityEngine;
using System.Collections;

public class PreacherController : MonoBehaviour {

    public KeyCode killKey = KeyCode.K;
    public KeyCode absolveKey = KeyCode.V;
    public KeyCode confessionKey = KeyCode.C;

    public KeyCode reprimendKey = KeyCode.R;
    public KeyCode exorciseKey = KeyCode.E;
    public KeyCode preachKey = KeyCode.P;
    public KeyCode holyBathKey = KeyCode.H;

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

    private void Convert(ConvertionActions action)
    {
        availableSoul.Convert(action);
    }

	// Update is called once per frame
	void Update () {
        if (!IsPoorSoulReachable())
        {   // no soul available
            return;
        }
        if (Input.GetKeyDown(killKey))
        {
            Kill();
        }
        else if (Input.GetKeyDown(preachKey))
        {
            Convert(ConvertionActions.Preach);
        }
        else if (Input.GetKeyDown(reprimendKey))
        {
            Convert(ConvertionActions.Reprimend);
        }
        else if (Input.GetKeyDown(exorciseKey))
        {
            Convert(ConvertionActions.Exorcise);
        }
        else if (Input.GetKeyDown(holyBathKey))
        {
            Convert(ConvertionActions.HolyBath);
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
