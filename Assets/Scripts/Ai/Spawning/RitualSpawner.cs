using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class RitualSpawner : MonoBehaviour
{
    public Ritual ritual = null;
    [SerializeField] int completionPoints;

    [Header("Display Variables")]
    public bool RitualActive;
    public bool ritualComplete;
    public int currentDemons;
    public int demonsLeft;
    public int demonsToSpawn;

    [Header("Spawn Points")]
    [SerializeField] Transform SpawnPoint;
    private List<Spawner> spawnPoints = new List<Spawner>();

    [Header("Demons")]
    public Queue<DemonType> DemonQueue = new Queue<DemonType>();
    private List<DemonFramework> ActiveDemons = new List<DemonFramework>();

    [Header("Blockers")]
    [SerializeField] Transform BlockerObjects;
    private List<Transform> blockers = new List<Transform>();
    private List<RitualWall> blockerWalls = new List<RitualWall>();

    [Header("Audio")]
    [SerializeField] SoundPlayer soundPlayerStart;
    [SerializeField] SoundPlayer soundPlayerFail;
    [SerializeField] SoundPlayer soundPlayerComplete;
    [SerializeField] SoundPlayer soundPlayerMusic;
    [SerializeField] float musicFadeOutTime;

    [Header("Ritual Completion")]
    [SerializeField] GameObject completion;
    [SerializeField] GameObject orbHolder;
    [HideInInspector] public bool IncrementRitual;
    [SerializeField] GameObject book;

    private SpawnerManager manager;
    private DemonSpawner demonSpawner;
    private Health playerHealth;
    private Timer ritualTimer;

    //private float timer;

    private void Awake()
    {
        manager = FindObjectOfType<SpawnerManager>();
        spawnPoints = HelperFuntions.GetAllChildrenSpawnersFromParent(SpawnPoint);
        blockers = HelperFuntions.GetAllChildrenTransformsFromParent(BlockerObjects);

        foreach (Transform t in blockers)
        {
            blockerWalls.Add(t.GetComponent<RitualWall>());
        }

        ritualComplete = GamePrefs.RitualsComplete;
    }

    private void Start()
    {
        if(ritualComplete == true)
        {
            OnComplete(manager);
        }

        foreach(RitualWall wall in blockerWalls)
        {
            wall.Fall();
        }
        demonSpawner = manager._DemonSpawner;
    }

    private void Update()
    {
        if(RitualActive == true && ritual == true)
        {
            if(playerHealth.dead == true)
            {
                OnFailed(manager);
                return;
            }

            Spawning(demonSpawner, manager);
        }
    }

    public void InitaliseRitual()
    {
        book.SetActive(true);
        if(RitualActive == false && ritualComplete == false)
        {
            RitualActive = true;
            ActiveDemons.Clear();

            //SetDemonQueue(ritual.ritualWave);

            DemonQueue = ritual.DemonWave;
            int count = DemonQueue.Count;
            demonsLeft = count;
            demonsToSpawn = count;

            if(playerHealth == null) playerHealth = manager.player.GetComponent<Health>();

            foreach (RitualWall wall in blockerWalls)
            {
                wall.Rise();
            }

            soundPlayerStart.Play();
            soundPlayerMusic.Play();
            soundPlayerMusic.looping = true;

            ritualTimer = new Timer(ritual.TimeBetweenSpawns);
        }
    }

    public void Spawning(DemonSpawner spawner, SpawnerManager sm)
    {
        if(ritualTimer.TimeGreaterThan && RitualActive == true && ritualComplete == false)
        {
            if (HelperFuntions.IntGreaterThanOrEqual(ritual.MaxDemonsAtOnce, currentDemons))
            {
                //timer = 0;

                if (demonsLeft <= 0 && currentDemons <= 0 && demonsToSpawn <= 0)
                {
                    OnComplete(sm);
                    
                    return; 
                }

                if (DemonQueue.Count <= 0) { return; }

                int toSpawn = ritual.MaxDemonsAtOnce - currentDemons;

                if (toSpawn >= ritual.SpawnsPerTick) { toSpawn = ritual.SpawnsPerTick; }

                if (demonsToSpawn < toSpawn) { toSpawn = demonsToSpawn; }

                spawnPoints = HelperFuntions.ShuffleList(spawnPoints);

                foreach (var spawnPoint in spawnPoints) { spawnPoint.Visited = false; }

                if (toSpawn > 0)
                {
                    for (int i = 0; i < toSpawn; i++)
                    {
                        if (spawner.SpawnDemonRitual(spawnPoints, this, sm, ActiveDemons))
                        {
                            currentDemons++;
                            demonsToSpawn--;
                        }
                    }
                }
            }
        }
    }

    public void DespawnAllActiveDemons()
    {
        int count = ActiveDemons.Count;

        for (int i = 0; i < count; i++)
        {
            ActiveDemons[i].OnDespawn();
        }

        ActiveDemons.Clear();
    }

    public void OnComplete(SpawnerManager sm)
    {
        ritualComplete = true;
        RitualActive = false;

        foreach (RitualWall wall in blockerWalls)
        {
            wall.Fall();
        }

        soundPlayerComplete.Play();

        completion.SetActive(false);
        book.SetActive(false);
        if (orbHolder != null)
        {
            orbHolder.GetComponent<Bounce>().Escape();
        }

        sm.FinalRitual();
        sm.TpPlayerOnEnd();

        sm.RunDefaultSpawning = true;
        sm.SetCurrentRitual(null);

        if(IncrementRitual == true) sm.IncrementRitualIndex();

        PlayerStats p = FindObjectOfType<PlayerStats>();
        p.GainPoints(completionPoints);

        soundPlayerMusic.looping = false;

        Sequence fade = DOTween.Sequence();
        AudioSource musicSource = soundPlayerMusic.GetComponent<AudioSource>();
        fade.Append(DOTween.To(() => musicSource.volume, x => musicSource.volume = x, 0, musicFadeOutTime));
        fade.AppendCallback(() => soundPlayerMusic.Stop());
    }

    public void OnFailed(SpawnerManager sm)
    {
        sm.RunDefaultSpawning = true;
        sm.TpPlayerOnEnd();

        RitualActive = false;
        //ritual = null;
        currentDemons = 0;
        demonsLeft = 0;
        demonsToSpawn = 0;

        foreach (RitualWall wall in blockerWalls)
        {
            wall.Fall();
        }

        soundPlayerFail.Play();

        soundPlayerMusic.looping = false;

        soundPlayerMusic.Stop();

        DespawnAllActiveDemons();

        if (IncrementRitual == true) sm.SetCurrentRitual(null);
    }

    void SetDemonQueue(Wave wave)
    {
        List<DemonType> demons = new List<DemonType>();

        int _base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, ritual.demonsToSpawn));
        int _littleGuy = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.LittleGuyPercentage, ritual.demonsToSpawn));

        int counter = ritual.demonsToSpawn;

        AddTypeToList(wave.Base, _base, demons);

        counter -= _base;

        AddTypeToList(wave.LittleGuy, _littleGuy, demons);

        counter -= _littleGuy;

        AddTypeToList(wave.Base, counter, demons);

        demons = HelperFuntions.ShuffleList(demons);

        DemonQueue = HelperFuntions.AddListToQueue(demons);
    }

    void AddTypeToList(DemonType type, int amount, List<DemonType> list)
    {
        for (int i = 0; i < amount; i++)
        {
            list.Add(type);
        }
    }

    public void AddDemonBackToQueue(DemonType type)
    {
        DemonQueue.Enqueue(type);
    }

    public int DemonCount
    {
        get { return DemonQueue.Count; }
    }

}
