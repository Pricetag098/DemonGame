using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerDeath : MonoBehaviour,IDataPersistance<GameData>
{
    [SerializeField] CanvasGroup canvasGroup;
    float timer;
    public float fadeTime;
    Health health;
    [SerializeField] Transform respawnPoint;
    PerkManager perkManager;
    public int respawnsLeft;
    public float deathStateTimeSeconds;
    DeathStateToggler[] togglers;

    int deaths;
    // Start is called before the first frame update
    void Awake()
    {
        perkManager = GetComponent<PerkManager>();
        health = GetComponent<Health>();
        health.OnDeath += Die;
        
    }

    private void Start()
    {
        togglers = FindObjectsOfType<DeathStateToggler>(true);
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

    IEnumerator DoDie()
	{
        Time.timeScale = 0;
        while (timer < fadeTime)
		{
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = timer / fadeTime;
            yield return null;
		}
        Time.timeScale = 1;
        transform.position = respawnPoint.position;
        SetWorldState(false);
        transform.forward = respawnPoint.forward;
        while (timer >=0)
        {
            timer -= Time.unscaledDeltaTime;
            canvasGroup.alpha = timer / fadeTime;
            yield return null;
        }
        health.Respawn();
        perkManager.ClearPerks();

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
