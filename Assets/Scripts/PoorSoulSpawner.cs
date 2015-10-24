using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoorSoulSpawner : MonoBehaviour {

    private List<PoorSoulController> souls = new List<PoorSoulController>();
    // Do not spawn poor souls more often than these number of seconds
    public float minSpawnTime = 20;

    // Do not spawn more than this number of souls
    public int maxPoorSouls = 50;

    public float spanwProbPerFrame = 0.01f;

    // prefab for spawning new poor souls
    public GameObject poorSoulPrefab;

    // Bounding space for spawning them
    public Bounds spawnBounds;
    private float lastSpawnSince;

	// Use this for initialization
	void Start () {
	
	}

    private void Spawn()
    {
        Debug.Log("Spawn!");
        GameObject go = GameObject.Instantiate(poorSoulPrefab);
        go.transform.position = new Vector3(Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            Random.Range(spawnBounds.min.y, spawnBounds.max.y),
            Random.Range(spawnBounds.min.z, spawnBounds.max.z));

        PoorSoulController psc = go.GetComponentInChildren<PoorSoulController>();
        if (psc != null)
        {
            souls.Add(psc);
        }
    }

    private bool ShouldSpawn()
    {
        lastSpawnSince += Time.deltaTime;
        bool spawn = souls.Count < maxPoorSouls && (lastSpawnSince > minSpawnTime) && Random.value < spanwProbPerFrame;
        if (spawn)
        {
            lastSpawnSince = 0;
        }
        return spawn;
    }

	// Update is called once per frame
	void Update () {
        if (ShouldSpawn())
        {
            Spawn();
        }
	}
}
