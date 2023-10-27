using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class LoadingBar : MonoBehaviour
{
    public GameObject loadingImage;
    public GameObject inputAwaiterImage;

    public Slider slider;
    public float loadTime;
    float timer;
    [TextArea(2,5)]
    public string toolTipText;

    public TextMeshProUGUI textObject;

    AsyncOperation operation;
    [SerializeField] InputActionProperty inputAction;
    public bool isLoading = false;
    private void Awake()
    {
        textObject.text = toolTipText;
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
}
