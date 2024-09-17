using DG.Tweening;
using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

	[SerializeField] CanvasGroup playerHUDCanvas;
    public bool open {  get; private set; }
    CanvasGroup canvasGroup;
	[SerializeField] private SettingsMenu settingsMenu;
	[SerializeField] private CanvasGroup pauseMenuParent;
    [SerializeField] float openTime;

    [SerializeField] InputActionProperty openAction;

	[SerializeField] List<AudioSource> musicSources;

	AudioSource activeSource;

	PlayerInputt playerInput;
	

    [ContextMenu("Open")]
    public void Open()
    {
        if (open)
            return;
        DOTween.Kill(this,true);
		open = true;
		Sequence s = DOTween.Sequence(this);
		s.SetUpdate(true);
		s.Append(canvasGroup.DOFade(1, openTime));
		s.AppendCallback(() =>
		{
			playerHUDCanvas.interactable = false;
			playerHUDCanvas.blocksRaycasts = false;
			playerHUDCanvas.alpha = 0;

			activeSource = null;

            canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			Time.timeScale = 0;
            Time.timeScale = 0;
			Time.fixedDeltaTime = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

			foreach (AudioSource source in musicSources)
			{
				if (source.isPlaying)
				{
					source.Pause();
					activeSource = source;
				}
			}
			
			playerInput.enabled = false;
		});
	}
	[ContextMenu("Close")]
	public void Close()
    {
        if(!open) 
            return;
		CloseSettingsMenu();
        DOTween.Kill(this,true);
        open = false;
        Sequence s = DOTween.Sequence(this);
        s.SetUpdate(true);
		s.AppendCallback(() =>
		{
            playerHUDCanvas.interactable = true;
            playerHUDCanvas.blocksRaycasts = true;
            playerHUDCanvas.alpha = 1;

            canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
			Time.timeScale = 1;
            Time.fixedDeltaTime = 0.01f;
            Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			if(activeSource != null)
			{
				activeSource.UnPause();
			}

            playerInput.enabled = true;
        });
		s.Append(canvasGroup.DOFade(0, openTime));
        
	}

    public void Toggle()
    {
        if(open)
            Close();
        else
            Open();
    }
    void ToggleAction(InputAction.CallbackContext context)
    {
        Toggle();
    }
	// Start is called before the first frame update
	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
        open = true;
        Close();
        openAction.action.performed += ToggleAction;
		playerInput = FindObjectOfType<PlayerInputt>();
	}
	private void OnEnable()
	{
        openAction.action.Enable();
	}
	private void OnDisable()
	{
		openAction.action.Disable();
	}
	private void OnDestroy()
	{
		openAction.action.performed -= ToggleAction;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
	void CloseSettingsMenu()
	{
        pauseMenuParent.alpha = 1;
        pauseMenuParent.interactable = true;
        settingsMenu.Close();
	}

	public void Quit()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}
}
