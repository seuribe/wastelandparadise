using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoorSoulController : MonoBehaviour {

    private float belief = 0;

    public float ascensionAccel = 0.5f;
    public float maxAscensionSpeed = 5;
    public float heavenHeight = 100;
    private Vector3 ascensionSpeed = new Vector3();

    public float beliefForConversion = 10;
    public float beliefLostPerSecond = 0.3f;

    public float preachReaction = 1;
    public float reprimendReaction = -1;
    public float exorciseReaction = 0.5f;
    public float holyBathReaction = -0.5f;

    public float followSpeed = 2;
    public float chaseSpeed = 5;
    public float minFollowDistance = 5;
    public float minChaseDistance = 2;
    public float maxChaseDistance = 35;
    public float harmDistance = 3;

    private bool confessed;
    private bool absolved;
    private bool dead;

    public PreacherController player;
    public PoorSoulSpawner spawner;

    private Vector3 absolveScale = new Vector3(1.25f, 1.25f, 1.25f);

    public GameObject halo;

    public Dictionary<ConvertionActions, float> reactions = new Dictionary<ConvertionActions,float>();

    public bool IsConverted
    {
        get {
            return belief >= beliefForConversion;
        }
    }

    public bool Confessed
    {
        get
        {
            return confessed;
        }
    }

    public bool Absolved
    {
        get
        {
            return absolved;
        }
    }

    public bool Dead
    {
        get
        {
            return dead;
        }
    }

    public void Die()
    {
        Debug.Log("Die");
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        dead = true;
        spawner.OneLess();
        // slowly go into the ground and then dissappear
        // start a ghost-soul floating towards heaven
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
        if (absolved || dead)
        {
            return;
        }
        float reaction = 0;
        if (reactions.TryGetValue(action, out reaction))
        {
            belief += reaction * player.conversionPower;
        }
    }

    public void Confess()
    {
        Debug.Log("Confess");
        if (!IsConverted || dead)
        {
            return;
        }
        confessed = true;
        halo.transform.localScale.Scale(absolveScale);
    }

    public void Absolve()
    {
        Debug.Log("Absolve");
        if (!confessed || dead)
        {
            return;
        }
        absolved = true;
        halo.transform.localScale.Scale(absolveScale);
    }


    void LookAtPlayer()
    {
        Debug.Log("Look At Player");
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(player.transform.position - transform.position), 0.1f);
    }
    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > minFollowDistance)
        {
            transform.position += (player.transform.position - transform.position).normalized * followSpeed * Time.deltaTime;
        }
    }

    void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < maxChaseDistance && distance > minChaseDistance)
        {
            transform.position += (player.transform.position - transform.position).normalized * chaseSpeed * Time.deltaTime;
        }
        if (distance < harmDistance)
        {
            player.Harm();
        }
    }
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            float direction = absolved ? 1 : -1;
            if (transform.position.y > heavenHeight || transform.position.y < -heavenHeight)
            {
                Debug.Log("Destroy dead body");
                Destroy(gameObject);
                return;
            }

            ascensionSpeed.y = Mathf.Clamp(ascensionSpeed.y + ascensionAccel * Time.deltaTime, 0, maxAscensionSpeed);
            transform.localPosition += ascensionSpeed * direction * Time.deltaTime;
        }
        LookAtPlayer();
        if (absolved)            // DO nothing, just await for death
        {
            return;
        }
        if (confessed) // Follow player waiting for absolution
        {
            FollowPlayer();
            return;
        }
        belief = Mathf.Clamp(belief, 0, belief - beliefLostPerSecond * Time.deltaTime);
        if (IsConverted)
        {
            // if player is around, follow him peacefully. If not, stay in place enjoying your inner peace
            FollowPlayer();
        }
        else
        {
            // Chase the player
            ChasePlayer();
        }
        float beliefRatio = Mathf.Clamp(belief, 0, beliefForConversion) / beliefForConversion;
        halo.GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(Color.red, Color.yellow, beliefRatio);
        halo.transform.localPosition = Vector3.Slerp(halo.transform.localPosition, new Vector3(0, 0.5f + (beliefRatio * 0.5f), 0), 0.1f);
    }

}
