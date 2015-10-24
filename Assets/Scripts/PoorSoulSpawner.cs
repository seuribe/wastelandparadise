using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoorSoulSpawner : MonoBehaviour {

    public PreacherController player;
    // Do not spawn poor souls more often than these number of seconds
    public float minSpawnTime = 20;

    // Do not spawn more than this number of souls
    public int maxPoorSouls = 50;

    private int numSouls = 0;

    public float spanwProbPerFrame = 0.01f;

    // prefab for spawning new poor souls
    public GameObject poorSoulPrefab;

    // Bounding space for spawning them
    public Bounds spawnBounds;
    private float lastSpawnSince;

	// Use this for initialization
	void Start () {
	
	}

    private bool paused = true;
    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }

    public void OneLess()
    {
        numSouls--;
    }

    private void Spawn()
    {
        Debug.Log("Spawn!");
        GameObject go = GameObject.Instantiate(poorSoulPrefab);
        go.transform.position = new Vector3(Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            Random.Range(spawnBounds.min.y, spawnBounds.max.y),
            Random.Range(spawnBounds.min.z, spawnBounds.max.z));

        PoorSoulController psc = go.GetComponentInChildren<PoorSoulController>();
        psc.player = player;
        psc.spawner = this;

        numSouls++;
    }

    private bool ShouldSpawn()
    {
        if (paused)
        {
            return false;
        }
        lastSpawnSince += Time.deltaTime;
        bool spawn = numSouls < maxPoorSouls && (lastSpawnSince > minSpawnTime) && Random.value < spanwProbPerFrame;
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
