using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;
using Movement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    TextMeshProUGUI text;
    SpawnerManager spawnerManager;
    float respawnTimer;
    float deathStateTimer;
    public float fadeTime;
    public UnityEvent onRespawnEvents;
    Health health;
    [SerializeField] Transform respawnPoint;
    PerkManager perkManager;
    public int respawnsLeft;
    public float deathStateTimeSeconds;
    public Slider deathStateSlider;
    DeathStateToggler[] togglersA;
    public List<DeathStateToggler> togglersL;
    public bool dead { private set; get; }
    PlayerStats stats;
    [Range(0,1)] public float pointLoss;
    //public int deaths;
    [SerializeField] string onDeathText, onRegaintext, onLossText;
    ResurrectionBuy resurrectionBuy;
    EndGameScreen endGameTweening;
    PlayerInputt playerInputt;
    // Start is called before the first frame update
    void Awake()
    {
        perkManager = GetComponent<PerkManager>();
        health = GetComponent<Health>();
        stats = GetComponent<PlayerStats>();
        health.OnDeath += Die;
        body = FindObjectOfType<PlayerBodyInteract>();
        spawnerManager = FindObjectOfType<SpawnerManager>();
        text = canvasGroup.GetComponentInChildren<TextMeshProUGUI>();
        resurrectionBuy = FindObjectOfType<ResurrectionBuy>();
        endGameTweening = FindObjectOfType<EndGameScreen>();
        playerInputt = FindObjectOfType<PlayerInputt>();
    }

    PlayerBodyInteract body;

    private void Start()
    {
        togglersA = FindObjectsOfType<DeathStateToggler>(true);

        togglersL.AddRange(togglersA);

        SetWorldState(true);
    }

    private void Update()
    {
        if (dead)
        {
            deathStateTimer += Time.deltaTime;
            float sliderTimeLeft = 1f- Mathf.Clamp01(deathStateTimer / deathStateTimeSeconds);
            deathStateSlider.value = sliderTimeLeft;
            if(deathStateTimer > deathStateTimeSeconds)
            {
                ReturnToBody(true);
                text.text = onLossText;
                stats.points = Mathf.FloorToInt((float)stats.points * (1- pointLoss));
            }
        }
    }


    void Die()
	{
        playerInputt.IDied();
        stats.deaths++;
        stats.ResetKillStreak();
        if(respawnsLeft > 0)
		{
            //text.text = onDeathText;
            StopAllCoroutines();
            StartCoroutine(DoDie());
            respawnsLeft--;
        }
        else
		{
            endGameTweening.Open(false);
            playerInputt.enabled = false;
		}
        
	}

    public void EndGame()
    {
        endGameTweening.Open(true);
        playerInputt.enabled = false;
    }

    public void ReturnToBody(bool outOfTime)
    {
        dead = false;
        body.Hide();
        //text.text = onRegaintext;
        StopAllCoroutines();
        StartCoroutine(DoReturnToBody(outOfTime));
        resurrectionBuy.EnableEmission();
    }

    IEnumerator DoReturnToBody(bool outOfTime)
    {
        playerInputt.enabled = false;
        onRespawnEvents.Invoke();
        Time.timeScale = 0;
        while (respawnTimer < fadeTime)
        {
            respawnTimer += Time.unscaledDeltaTime;
            canvasGroup.alpha = respawnTimer / fadeTime;
            yield return null;
        }
        Time.timeScale = 1;

        if (outOfTime) transform.position = respawnPoint.position;
        else transform.position = body.body.transform.position;
        playerInputt.enabled = true;
        SetWorldState(true);
        spawnerManager.RunDefaultSpawning = true;
        transform.rotation = body.body.transform.rotation;
        while (respawnTimer >= 0)
        {
            respawnTimer -= Time.unscaledDeltaTime;
            canvasGroup.alpha = respawnTimer / fadeTime;
            yield return null;
        }
    }


    IEnumerator DoDie()
	{
        playerInputt.enabled = false;
        Time.timeScale = 0;
        while (respawnTimer < fadeTime)
		{
            respawnTimer += Time.unscaledDeltaTime;
            canvasGroup.alpha = respawnTimer / fadeTime;
            yield return null;
		}
        Time.timeScale = 1;
        SetWorldState(false);
        spawnerManager.DespawnAllActiveDemons();
		spawnerManager.RunDefaultSpawning = false;
		transform.rotation = respawnPoint.rotation;
        health.Respawn();
        perkManager.ClearPerks();
        body.body.transform.position = transform.position;
        body.body.transform.rotation = transform.rotation;
        body.Show();
        playerInputt.enabled = true;
        transform.position = respawnPoint.position;
        while (respawnTimer >=0)
        {
            respawnTimer -= Time.unscaledDeltaTime;
            canvasGroup.alpha = respawnTimer / fadeTime;
            yield return null;
        }
        dead = true;
        deathStateTimer = 0;
    }


    public void SetWorldState(bool alive)
    {
        foreach(DeathStateToggler toggler in togglersL)
        {
            toggler.Toggle(alive);
        }
    }

   
}
