using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] GameObject endTitle;
    [SerializeField] GameObject endStats;
    [SerializeField] GameObject overlay;
    [SerializeField] Slider timeSlider;

    [SerializeField] float titleFadeTime;
    [SerializeField] float titleOnScreenTime;
    [SerializeField] float titleMoveTime;
    [SerializeField] float statsFadeTime;
    [SerializeField] float statsOnScreenTime = 10;

    [SerializeField] Vector3 titleEndPos;

    [SerializeField] string deathText;
    [SerializeField] string finishedGameText;

    [SerializeField] CanvasGroup playerHUD;

    [SerializeField] CanvasGroup pauseMenu;


    Vector3 titleOrigin;

    RectTransform titleRectTransform;

    CanvasGroup endTitleCanvas;
    CanvasGroup endStatsCanvas;
    CanvasGroup overlayCanvas;
    CanvasGroup thisCanvas;

    PlayerStats stats;

    EventSystem eventSystem;

    [SerializeField] TextMeshProUGUI pointsText, killsText, headShotsText, bloodGainText, deathsText,roundText, deadText;

    private void Awake()
    {
        thisCanvas = GetComponent<CanvasGroup>();
        endTitleCanvas = endTitle.GetComponent<CanvasGroup>();
        endStatsCanvas = endStats.GetComponent<CanvasGroup>();
        overlayCanvas = overlay.GetComponent<CanvasGroup>();
        stats = FindObjectOfType<PlayerStats>();
        titleRectTransform = endTitle.GetComponent<RectTransform>();

        titleOrigin = titleRectTransform.localPosition;

        eventSystem = EventSystem.current;
    }

    private void Start()
    {
        titleRectTransform.localPosition = titleOrigin;

        endTitleCanvas.alpha = 0;
        endStatsCanvas.alpha = 0;
        overlayCanvas.alpha = 0;
    }

    void Update()
    {
        // First, check if the mouse is over a UI element
        if (IsPointerOverUIObject())
        {
            Debug.Log("Hovering over UI element: " + GetUIElementUnderCursor());
        }
        else
        {
            // If not over UI, then do a raycast to detect 3D objects
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hovering over GameObject: " + hit.collider.gameObject.name);
            }
            else
            {
                Debug.Log("No object under the cursor.");
            }
        }
    }

    // Function to check if the pointer is over a UI object
    private bool IsPointerOverUIObject()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // Function to get the UI element under the cursor (optional, for additional info)
    private string GetUIElementUnderCursor()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            return results[0].gameObject.name;  // Return the first UI element's name
        }

        return "None";
    }

    [ContextMenu("Tween")]
    public void Open(bool finishedGame)
    {
        Time.timeScale = 0;

        pauseMenu.gameObject.SetActive(false);

        if (finishedGame )
        {
            deadText.text = finishedGameText;
        }
        else
        {
            deadText.text = deathText;
        }
        timeSlider.value = 0;

        // Chag
        roundText.text = "surviving " + (SpawnerManager.currentRound) + " rounds";
        pointsText.text = stats.pointsGained.ToString();
        killsText.text = stats.kills.ToString();
        headShotsText.text = stats.headshotKills.ToString();
        bloodGainText.text = stats.GetComponent<PlayerAbilityCaster>().bloodSpent.ToString();
        deathsText.text = stats.deaths.ToString();


        Sequence on = DOTween.Sequence();
        on.SetUpdate(true);
        on.Append(overlayCanvas.DOFade(1, titleFadeTime + titleMoveTime + titleOnScreenTime + statsFadeTime).SetUpdate(true));
        on.Append(endTitleCanvas.DOFade(1, titleFadeTime));
        on.AppendInterval(titleOnScreenTime);
        on.AppendCallback(() => titleRectTransform.DOLocalMove(titleEndPos, titleMoveTime).SetUpdate(true));
        on.AppendInterval(titleMoveTime);
        on.Append(endStatsCanvas.DOFade(1, statsFadeTime));
        on.AppendCallback(() => 
        { 
            endStatsCanvas.interactable = true;
            thisCanvas.interactable = true;
            endStatsCanvas.blocksRaycasts = true;
            thisCanvas.blocksRaycasts = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            playerHUD.interactable = false;
            playerHUD.blocksRaycasts = false;
        });
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
