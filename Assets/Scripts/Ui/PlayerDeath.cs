using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerDeath : MonoBehaviour,IDataPersistance<GameData>,IDataPersistance<SessionData>
{
    [SerializeField] CanvasGroup canvasGroup;
    TextMeshProUGUI text;
    SpawnerManager spawnerManager;
    float respawnTimer;
    float deathStateTimer;
    public float fadeTime;
    Health health;
    [SerializeField] Transform respawnPoint;
    PerkManager perkManager;
    public int respawnsLeft;
    public float deathStateTimeSeconds;
    public Slider deathStateSlider;
    DeathStateToggler[] togglers;
    bool dead;
    PlayerStats stats;
    [Range(0,1)] public float pointLoss;
    int deaths;
    [SerializeField] string onDeathText, onRegaintext, onLossText;
    ResurrectionBuy resurrectionBuy;
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
    }

    PlayerBodyInteract body;

    private void Start()
    {
        togglers = FindObjectsOfType<DeathStateToggler>(true);
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
                ReturnToBody();
                text.text = onLossText;
                stats.points = Mathf.FloorToInt((float)stats.points * (1- pointLoss));
            }
        }
    }


    void Die()
	{
        if(respawnsLeft > 0)
		{
            text.text = onDeathText;
            StopAllCoroutines();
            StartCoroutine(DoDie());
            respawnsLeft--;
        }
        else
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
        
	}

    public void ReturnToBody()
    {
        dead = false;
        body.Hide();
        text.text = onRegaintext;
        StopAllCoroutines();
        StartCoroutine(DoReturnToBody());
        resurrectionBuy.EnableEmission();
    }

    IEnumerator DoReturnToBody()
    {
        
        Time.timeScale = 0;
        while (respawnTimer < fadeTime)
        {
            respawnTimer += Time.unscaledDeltaTime;
            canvasGroup.alpha = respawnTimer / fadeTime;
            yield return null;
        }
        Time.timeScale = 1;
        
        transform.position = body.body.transform.position;
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
        Time.timeScale = 0;
        while (respawnTimer < fadeTime)
		{
            respawnTimer += Time.unscaledDeltaTime;
            canvasGroup.alpha = respawnTimer / fadeTime;
            yield return null;
		}
        Time.timeScale = 1;
        body.body.transform.position = transform.position;
        body.body.transform.rotation = transform.rotation;
        body.Show();
        transform.position = respawnPoint.position;
        SetWorldState(false);
        spawnerManager.DespawnAllActiveDemons();
		spawnerManager.RunDefaultSpawning = false;
		transform.rotation = respawnPoint.rotation;
        while (respawnTimer >=0)
        {
            respawnTimer -= Time.unscaledDeltaTime;
            canvasGroup.alpha = respawnTimer / fadeTime;
            yield return null;
        }
        health.Respawn();
        perkManager.ClearPerks();
        dead = true;
        deathStateTimer = 0;
    }


    public void SetWorldState(bool alive)
    {
        foreach(DeathStateToggler toggler in togglers)
        {
            toggler.Toggle(alive);
        }
    }

    void IDataPersistance<GameData>.SaveData(ref GameData data)
	{
        //data.deaths = deaths;
	}
    void IDataPersistance<GameData>.LoadData(GameData data)
	{
        deaths += data.deaths;
	}

    public void LoadData(SessionData data)
    {
        //throw new System.NotImplementedException();
    }

    public void SaveData(ref SessionData data)
    {
        data.deaths = deaths;
    }
}
