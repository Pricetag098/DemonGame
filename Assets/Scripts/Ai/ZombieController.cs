using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public int wave;
    [SerializeField] private int zombiesCountThisWave;
    [SerializeField] private int maxZombies;
    [SerializeField] private int maxZombiesAtOnce;
    [SerializeField] private int zombieSpawnRadius;
    [SerializeField] private int zombiesToSpawnEachTime;
    [SerializeField] private int zombieDespawnRadius;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float timeInbetweenWaves;

    [HideInInspector] public int activeZombieCount;
    private float timer;
    private float betweenWaveTimer;

    public GameObject player;

    public List<GameObject> zombiesTypes = new List<GameObject>(); // list of zombie prefabs to spawn
    public List<GameObject> activeZombies = new List<GameObject>(); // currently spawned zombies
    [HideInInspector] public List<GameObject> zombiesToDestroy = new List<GameObject>(); // what zombies to destroy

    [SerializeField] private List<GameObject> spawners = new List<GameObject>(); // all spawners in the level
    private List<GameObject> activeSpawners = new List<GameObject>(); // all active spawners

    Dictionary<string, GameObject> zombies = new Dictionary<string, GameObject>(); // for storing all zombie types

    void Start()
    {
        wave = 1;
        zombiesCountThisWave = 7;
        activeZombieCount = zombiesCountThisWave;

        foreach (GameObject zombie in zombiesTypes)
        {
            if(zombie != null)
                zombies.Add(zombie.name, zombie);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        ActiveSpawners(player);
        ZombieDistance(player);
        DestroyZombies();
        SpawnZombies();
        NextWave();
    }

    void NextWave() // if no zombies left to spawn start next wave
    {
        if (activeZombies.Count <= 0 && activeZombieCount <= 0)
        {
            betweenWaveTimer += Time.deltaTime;
            if (betweenWaveTimer > timeInbetweenWaves)
            {
                EndOfWaveChecks();
                betweenWaveTimer = 0;
            }
        }
    }
    void EndOfWaveChecks() // update wave variables
    {
        wave++;
        zombiesCountThisWave += 2;
        if (zombiesCountThisWave > maxZombies)
        {
            zombiesCountThisWave = maxZombies;
        }
        activeZombieCount = zombiesCountThisWave;
    }

    void DestroyZombies() // destroy all zombies in the destroy list
    {
        int count = zombiesToDestroy.Count;
        if(count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject zombie = zombiesToDestroy[i];

                if (activeZombies.Contains(zombie))
                {
                    activeZombies.Remove(zombie);
                    Destroy(zombie);
                }
            }
            zombiesToDestroy.Clear();
        }
    }

    void SpawnZombies() //spawns zombie prefab
    {
        GameObject zombieToSpawn;
        bool isMiniBoss = false;
        for (int i = 0; i < zombiesToSpawnEachTime; i++)
        {
            int zombieCount = activeZombies.Count;
            int count = activeSpawners.Count;
            int miniBoss = Random.Range(0, 15);
            if (zombieCount < maxZombiesAtOnce && zombieCount < activeZombieCount && timer > spawnTimer)
            {
                int rand = Random.Range(0, count);

                if (miniBoss < 1 && !isMiniBoss && wave > 5) // could change this to a spawn zombie count so every 20 zombies is a miniboss
                {
                    zombieToSpawn = zombies["Mini Boss"];
                    isMiniBoss = true;
                }
                else
                {
                    //int index = Random.Range(0, zombies.Count);
                    //zombieToSpawn = zombies.Values.ElementAt(index);
                    //while (zombieToSpawn == zombies["Mini Boss"])
                    //{
                    //    index = Random.Range(0, zombies.Count);
                    //    zombieToSpawn = zombies.Values.ElementAt(index);
                    //}
                    zombieToSpawn = zombies["Zombie"];
                }

                GameObject obj = Instantiate(zombieToSpawn, activeSpawners[rand].transform.position, Quaternion.identity);
                obj.transform.SetParent(transform);
                activeZombies.Add(obj);

                if (count >= 3) // remove from list if 3 or more spawners are active
                    activeSpawners.RemoveAt(rand);
            }
        }

        if (timer > spawnTimer)
            timer = 0;
    }

    void ZombieDistance(GameObject player) // checks each zombie distance to player if too far despawn zombie
    {
        int count = activeZombies.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject zombie = activeZombies[i];

            Transform z = zombie.transform;
            Transform p = player.transform;

            Vector2 zombiePos = new Vector2(z.position.x, z.position.z);
            Vector2 playerPos = new Vector2(p.position.x, p.position.z);

            float distance = Vector2.Distance(playerPos, zombiePos);

            if (distance > zombieDespawnRadius)
            {
                zombiesToDestroy.Add(zombie);
            }
        }
    }

    void ActiveSpawners(GameObject player) // checks all spawners positions to see if the player is within range
    {
        int count = spawners.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject spawner = spawners[i];

            Transform s = spawner.transform;
            Transform p = player.transform;

            Vector2 spawnerPos = new Vector2(s.position.x, s.position.z);
            Vector2 playerPos = new Vector2(p.position.x, p.position.z);

            float dist = Vector2.Distance(playerPos, spawnerPos);

            if (dist < zombieSpawnRadius)
            {
                if (!activeSpawners.Contains(spawner))
                {
                    activeSpawners.Add(spawner);
                    spawner.SetActive(true);
                }
            }
            else
            {
                if (activeSpawners.Contains(spawner))
                {
                    activeSpawners.Remove(spawner);
                    spawner.SetActive(false);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, zombieSpawnRadius);
    }
}
