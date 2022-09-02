using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float MinSpawnTime;
    public float MaxSpawnTime;

    public Player Target;
    public Enemy SpawnOrigin;

    public Transform[] SpawnPoints;

    private void Start()
    {
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinSpawnTime, MaxSpawnTime));

            var obj = GameObject.Instantiate(SpawnOrigin).GetComponent<Enemy>();

            obj.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
            obj.init(Target, 100);

            obj.gameObject.SetActive(true);
        }
    }

}
