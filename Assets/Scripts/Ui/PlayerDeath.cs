using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerDeath : MonoBehaviour,IDataPersistance<GameData>
{
    [SerializeField] CanvasGroup canvasGroup;
    float respawnTimer;
    float deathStateTimer;
    public float fadeTime;
    Health health;
    [SerializeField] Transform respawnPoint;
    PerkManager perkManager;
    public int respawnsLeft;
    public float deathStateTimeSeconds;
    DeathStateToggler[] togglers;
    bool dead;

    int deaths;
    // Start is called before the first frame update
    void Awake()
    {
        perkManager = GetComponent<PerkManager>();
        health = GetComponent<Health>();
        health.OnDeath += Die;
        
    }

    public Transform body;

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
            if(deathStateTimer > deathStateTimeSeconds)
            {
                ReturnToBody();
            }
        }
    }


    void Die()
	{
        if(respawnsLeft > 0)
		{
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
        StartCoroutine(DoReturnToBody());

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
        
        transform.position = body.position;
        SetWorldState(true);
        dead = false;
        transform.rotation = body.rotation;
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
        body.position = transform.position;
        body.rotation = transform.rotation;
        transform.position = respawnPoint.position;
        SetWorldState(false);
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
        data.deaths = deaths;
	}
    void IDataPersistance<GameData>.LoadData(GameData data)
	{
        deaths = data.deaths;
	}
}
