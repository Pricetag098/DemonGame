using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
public class LoadingBar : MonoBehaviour
{
    public GameObject loadingImage;
    public GameObject inputAwaiterImage;
    public CanvasGroup[] controlDisplays;
    public int controlIndex = 0;

    public Slider slider;
    public float loadTime;
    float timer;

    public float toolTipTimer;
    public float controllerTimer;

    [TextArea(2, 5)]
    public List<string> potentialToolTips;
    public float toolTipDuration;
    public float toolTipFadeDuration;

    [TextArea(2,5)]
    public string toolTipText;

    public TextMeshProUGUI textObject;

    AsyncOperation operation;
    [SerializeField] InputActionProperty inputAction;
    public bool isLoading = false;


    private void Awake()
    {
        textObject.text = "Shooting demons grants you their blood, channel it to use it against them... rinse and repeat";
        slider.value= 0;
        slider.maxValue = 1;
        inputAction.action.performed += DoLoad;
        inputAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading) 
        { 
            Loading();
            if (slider.value >= slider.maxValue) 
            { 
                isLoading = false;
                EndLoading();
            }
        }

        toolTipTimer += Time.deltaTime;
        controllerTimer += Time.deltaTime;

        if (toolTipTimer > (toolTipDuration + toolTipFadeDuration))
        {
            StartCoroutine(CycleNextToolTip());
            toolTipTimer = 0;
        }

        if (controlIndex > 1)
        {
            controlIndex= 0;
        }
    }

    void DoLoad(InputAction.CallbackContext context)
    {
        operation.allowSceneActivation = true;
    }

    void EndLoading()
    {
        inputAction.action.Enable();

        loadingImage.SetActive(false);
        inputAwaiterImage.SetActive(true);
    }

    public void StartLoading()
    {
        operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;
        isLoading = true;
    }

    public void Loading()
    {
        
        if (timer/loadTime <= operation.progress +.1f)
        {
            timer += Time.deltaTime;
        }
        slider.value = timer/loadTime;
    }

    public IEnumerator CycleNextToolTip()
    {
        textObject.DOFade(0, toolTipFadeDuration);

        yield return new WaitForSeconds(toolTipFadeDuration);

        int index = Random.Range(0, potentialToolTips.Count - 5);
        string lastTip = potentialToolTips[index];
        textObject.text = lastTip;
        textObject.DOFade(1, toolTipFadeDuration);
        potentialToolTips.RemoveAt(index);
        potentialToolTips.Add(lastTip);

        yield return new WaitForSeconds(toolTipDuration);

    }

}
