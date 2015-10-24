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

    private PoorSoulController GetReachableSoul()
    {
        return null;
    }

    private void Kill()
    {
        if (!IsPoorSoulReachable())
        {
            return;
        }

        Debug.Log("Kill");
        if (availableSoul.Absolved)
        {
            savedSouls++;
            conversionPower = Mathf.Clamp(conversionPower + savedSoulPowerGain, minConversionPower, maxConversionPower);
        }
        else
        {
            killedSouls++;
            conversionPower = Mathf.Clamp(conversionPower + lostSoulPowerGain, minConversionPower, maxConversionPower);
        }
        availableSoul.Die();
    }

    private void Confess()
    {
        if (!IsPoorSoulReachable())
        {
            return;
        }

        availableSoul.Confess();
    }

    private void Absolve()
    {
        if (!IsPoorSoulReachable())
        {
            return;
        }
        availableSoul.Absolve();
    }

    private bool IsPoorSoulReachable()
    {
        return availableSoul != null;
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

    public void Harm()
    {
        Debug.Log("Harm!");
        conversionPower = Mathf.Clamp(conversionPower + powerHarmed * Time.deltaTime, 0.25f, 1f);
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 150, 20), "Sent to heaven: " + savedSouls);
        GUI.TextField(new Rect(10, 40, 150, 20), "Sent to hell: " + killedSouls);
        GUI.TextField(new Rect(10, 70, 150, 20), "Conversion Power: " + conversionPower);
    }
}
