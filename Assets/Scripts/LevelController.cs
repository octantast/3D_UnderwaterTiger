using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    private float initialLaunch;
    private float mode; // 0 - infinity, 1+ - levels
    private int howManyLevelsDone;
    private float volume;

   public Color32 enableButton;
   [HideInInspector] public List<Image> buttons;

   [HideInInspector] public GameObject settingsButton;
   [HideInInspector] public GameObject startButtons;
   [HideInInspector] public GameObject levelButtons;
   [HideInInspector] public GameObject loadingScreen;
   [HideInInspector] public GameObject settings;
   [HideInInspector] public Image loading;

    // music
   [HideInInspector] public AudioSource ambient;
    [HideInInspector] public AudioSource tapSound;
    [HideInInspector] public Slider audioSlider;

    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }
    void Start()
    {
        initialLaunch = PlayerPrefs.GetFloat("initialLaunch");
        if (initialLaunch == 0)
        {
            PlayerPrefs.SetFloat("initialLaunch", 1);
            volume = 1;
            PlayerPrefs.SetFloat("volume", volume);
            PlayerPrefs.Save();
        }
        else
        {
            volume = PlayerPrefs.GetFloat("volume");
        }
        audioSlider.value = volume;

        howManyLevelsDone = PlayerPrefs.GetInt("howManyLevelsDone");
        for (int i = 0; i <= howManyLevelsDone; i++)
        {
            buttons[i].color = enableButton;
        }

        settings.SetActive(false);
        loadingScreen.SetActive(false);
        startButtons.SetActive(true);
        levelButtons.SetActive(false);

        asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        asyncOperation.allowSceneActivation = false;
    }

    private void Update()
    {
        if (loadingScreen.activeSelf == true)
        {
            ambient.volume -= 0.1f;
            tapSound.volume -= 0.1f;
            if (loading.fillAmount < 0.9f)
            {
                loading.fillAmount = Mathf.Lerp(loading.fillAmount, 1, Time.deltaTime * 2);
            }
            else
            {
                asyncOperation.allowSceneActivation = true;
            }
        }
        else
        {
            tapSound.volume = audioSlider.value;
            ambient.volume = audioSlider.value;
        }
    }
   public void StartGame(float mode)
    {
        playSound(tapSound);
        if (mode <= howManyLevelsDone + 1)
        {
            loading.fillAmount = 0;
            loadingScreen.SetActive(true);
            startButtons.SetActive(false);
            levelButtons.SetActive(false);
            settingsButton.SetActive(false);
            PlayerPrefs.SetFloat("mode", mode);
            PlayerPrefs.Save();
        }
    }

   public void ShowLevels()
    {
        playSound(tapSound);
        loadingScreen.SetActive(false);
        startButtons.SetActive(false);
        levelButtons.SetActive(true);

    }
    public void HideLevels()
    {
        playSound(tapSound);
        settings.SetActive(false);
        loadingScreen.SetActive(false);
        startButtons.SetActive(true);
        levelButtons.SetActive(false);
    }
   public void Settings()
    {
        playSound(tapSound);
        if (!settings.activeSelf)
        {
            Time.timeScale = 0;
            settings.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            settings.SetActive(false);
            PlayerPrefs.SetFloat("volume", audioSlider.value);
            PlayerPrefs.Save();
        }
    }

    public void playSound(AudioSource sound)
    {
        sound.Play();
    }

}
