using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoorSoulController : MonoBehaviour {

    private float belief = 0;

    public float beliefForConversion = 10;
    public float averagedReaction = 0;

    public float preachReaction = 1;
    public float reprimendReaction = -1;
    public float exorciseReaction = 0.5f;
    public float holyBathReaction = -0.5f;

    private bool confessed;
    private bool absolved;

    public GameObject halo;

    public Dictionary<ConvertionActions, float> reactions = new Dictionary<ConvertionActions,float>();

    public bool IsConverted
    {
        get {
            return belief < beliefForConversion;
        }
    }

    // Use this for initialization
	void Start ()
    {
        reactions.Add(ConvertionActions.Preach, preachReaction);
        reactions.Add(ConvertionActions.Reprimend, reprimendReaction);
        reactions.Add(ConvertionActions.Exorcise, exorciseReaction);
        reactions.Add(ConvertionActions.HolyBath, holyBathReaction);
    }

    public void Convert(ConvertionActions action)
    {
        if (absolved)
        {
            return;
        }
        float reaction = 0;
        if (reactions.TryGetValue(action, out reaction))
        {
            belief += reaction;
        }
    }

    public void Confess()
    {
        if (!IsConverted)
        {
            return;
        }
        confessed = true;
    }

    public void Absolve()
    {
        if (!confessed)
        {
            return;
        }
        absolved = true;
    }

	// Update is called once per frame
	void Update () {
        if (absolved)
        {
            // DO nothing, just await for death
        }
        else if (confessed)
        {
            // Follow player waiting for absolution
        }
        else if (IsConverted)
        {
            // 1. if player is around, follow him peacefully
            // 2. if not, stay in place enjoying your inner peace
        }
        else
        {
            // 1. if player is around, head for him
            // 2. if it's not around, mode randomly
        }
        float beliefRatio = Mathf.Clamp(belief, 0, beliefForConversion) / beliefForConversion;
        halo.GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(Color.red, Color.yellow, beliefRatio);
        halo.transform.localPosition = Vector3.Slerp(halo.transform.localPosition, new Vector3(0, 0.5f + (beliefRatio * 0.25f), 0), 0.5f);
    }

}
