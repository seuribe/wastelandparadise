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
    public Animator handsAnimator;

    public string reprimendAnimation = "no-no";
    public string exorciseAnimation = "no-no";
    public string preachAnimation = "no-no";
    public string holyBathAnimation = "no-no";

    private int killedSouls = 0;
    private int savedSouls = 0;

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
        Debug.Log("Kill");
        if (availableSoul.Absolved)
        {
            savedSouls++;
        }
        else
        {
            killedSouls++;
        }
        availableSoul.Die();
    }

    private void Confess()
    {
        availableSoul.Confess();
    }

    private void Absolve()
    {
        availableSoul.Absolve();
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
            handsAnimator.SetTrigger(preachAnimation);
        }
        else if (Input.GetKeyDown(reprimendKey))
        {
            Convert(ConvertionActions.Reprimend);
            handsAnimator.SetTrigger(reprimendAnimation);
        }
        else if (Input.GetKeyDown(exorciseKey))
        {
            Convert(ConvertionActions.Exorcise);
            handsAnimator.SetTrigger(exorciseAnimation);
        }
        else if (Input.GetKeyDown(holyBathKey))
        {
            Convert(ConvertionActions.HolyBath);
            handsAnimator.SetTrigger(holyBathAnimation);
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
