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

    public Animator handsAnimator;

    public GameController gameController;

    public float conversionPower = 1f;

    public string reprimendAnimation = "no-no";
    public string exorciseAnimation = "";
    public string preachAnimation = "";
    public string holyBathAnimation = "";

    public float convertionRadius = 5;

    public float savedSoulPowerGain = 0.1f;
    public float lostSoulPowerGain = -0.5f;
    public float maxConversionPower = 1f;
    public float minConversionPower = 0.25f;
    public float powerRecovery = 0.1f;
    public float powerHarmed = -0.1f;

	// Use this for initialization
	void Start () {
	
	}

    private PoorSoulController GetReachableSoul()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(GetCollisionRay(), out hit, convertionRadius))
        {
            Debug.Log("Reachable soul present: " + hit.collider);
            return hit.collider.gameObject.GetComponentInChildren<PoorSoulController>();
        }
        return null;
    }

    private void Kill()
    {
        Debug.Log("Kill");
        var soul = GetReachableSoul();
        if (soul == null)
        {
            Debug.Log("No reachable soul");
            return;
        }

        if (soul.Absolved)
        {
            Debug.Log("Kill: soul saved");
            gameController.SoulSaved();
            conversionPower = Mathf.Clamp(conversionPower + savedSoulPowerGain, minConversionPower, maxConversionPower);
        }
        else
        {
            Debug.Log("Kill: soul lost");
            gameController.SoulLost();
            conversionPower = Mathf.Clamp(conversionPower + lostSoulPowerGain, minConversionPower, maxConversionPower);
        }
        soul.Die();
    }

    private void Confess()
    {
        Debug.Log("Confess");
        var soul = GetReachableSoul();
        if (soul != null)
        {
            Debug.Log("Soul found");
            soul.Confess();
        }
    }

    private void Absolve()
    {
        Debug.Log("Absolve");
        var soul = GetReachableSoul();
        if (soul != null)
        {
            Debug.Log("Soul found");
            soul.Absolve();
        }
    }

    private void Convert(ConvertionActions action)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, convertionRadius);
        foreach (Collider collider in colliders)
        {
            PoorSoulController psc = collider.gameObject.GetComponentInChildren<PoorSoulController>();
            if (psc != null)
            {
                psc.Convert(action);
            }
        }
    }

	// Update is called once per frame
	void Update () {
        if (!gameController.IsPlaying)
        {
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

        conversionPower = Mathf.Clamp(conversionPower + powerRecovery * Time.deltaTime, 0.25f, 1f);
    }

    private Ray GetCollisionRay()
    {
        return new Ray(transform.position + new Vector3(0, -0.5f, 0), transform.forward * 2);
    }

    public void Harm()
    {
        conversionPower = Mathf.Clamp(conversionPower + powerHarmed * Time.deltaTime, 0.25f, 1f);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(GetCollisionRay());
    }

    void OnGUI()
    {
        if (gameController.IsPlaying)
        {
            GUI.TextField(new Rect(10, 70, 150, 20), "Conversion Power: " + conversionPower);
        }
    }
}
