using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GeneralController : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    [HideInInspector] public float mode;
    private int howManyLevelsDone;

    private float volume;
    [HideInInspector] public Slider audioSlider;

    // environment
    [HideInInspector] public PlayerController player;
    [HideInInspector] public GameObject platform;
    [HideInInspector] public GameObject platformHolder;
    private Vector3 platformPlace;
    private GameObject newPlatform;
    [HideInInspector] public Platform lastPlatform;
    [HideInInspector] public Platform previousPlatform;
    [HideInInspector] public float initial;

    // buttons
    [HideInInspector] public GameObject settings;
    [HideInInspector] public GameObject loadingScreen;
    [HideInInspector] public Image loading;
    [HideInInspector] public bool pause;
    public Color32 enableButton;
    public Color32 normalButton;
    [HideInInspector] public List<Image> buttons;


    public int shieldCost;
    public int clawCost;

    //shield
    private float shieldTimer;
    public float shieldTimerMax = 60;
    private float clawTimer;
    public float clawTimerMax;
    [HideInInspector] public TMP_Text shieldCount;
    [HideInInspector] public TMP_Text clawsCount;
    [HideInInspector] public Image shieldScale;
    [HideInInspector] public Image clawsScale;

    // currency
    [HideInInspector] public TMP_Text pearlCount;
    private float pearls;
    private float newPearls;

    // points
    [HideInInspector] public TMP_Text pointCount;

    // levels
    private int levelShellsTarget;
    private int levelMetresTarget;
    private float levelTimer;
    [HideInInspector] public TMP_Text levelTimerCount;

    private void Start()
    {
        Time.timeScale = 1;
        asyncOperation = SceneManager.LoadSceneAsync("Preloader");
        asyncOperation.allowSceneActivation = false;

        mode = PlayerPrefs.GetFloat("mode");
        volume = PlayerPrefs.GetFloat("volume");
        pearls = PlayerPrefs.GetFloat("pearls");
        howManyLevelsDone = PlayerPrefs.GetInt("howManyLevelsDone");

        player.newPearls = pearls;
        newPearls = pearls;
        buttonCheck();
        shieldCount.text = shieldCost.ToString();
        clawsCount.text = clawCost.ToString();

        audioSlider.value = volume;
        player.sounds[0].Play();

        pause = false;
        settings.SetActive(false);
        loadingScreen.SetActive(false);
        InstantiatePlatform(new Vector3(0f, 0f, -70));

        player.shieldBubble.SetActive(false);
        player.lineRenderer.gameObject.SetActive(false);
        player.playAgain.SetActive(false);
        player.winScreen.SetActive(false);

        // levels
        if(mode == 0)
        {
            levelTimerCount.text = "";
        }
        else if (mode == 4 || mode == 6 || mode == 9)
        {
            levelTimer = mode * 8;
            player.verticalMinimum = 0.5f;
        }
        else if (mode == 1 || mode == 3 || mode == 7 || mode == 10)
        {
            if (mode == 1)
            {
                levelShellsTarget = 1;
            }
            else if (mode == 3)
            {
                levelShellsTarget = 2;
            }
            else if (mode == 7)
            {
                levelShellsTarget = 3;
            }
            else if (mode == 10)
            {
                levelShellsTarget = 3;
                player.verticalMinimum = 0.5f;
            }
            levelTimerCount.text = "FIND PEARLS: " + levelShellsTarget.ToString("0");
        }
        else if (mode == 2 || mode == 5 || mode == 8)
        {
            if (mode == 2)
            {
                levelMetresTarget = 70;
            }
            else if (mode == 5)
            {
                levelMetresTarget = 150;
            }
            else if (mode == 8)
            {
                levelMetresTarget = 300;
            }
            levelTimerCount.text = "SWIM THE DISTANCE: " + levelMetresTarget.ToString("0") +"M";
        }
        if(mode == 1)
        {
            player.blocked = true;
        }
    }


    private void Update()
    {
        // count
        if (pearls != player.newPearls) // got a new shell
        {
            if (levelShellsTarget > 0)
            {
                levelCheckShell();
            }
            pearls = player.newPearls;
            buttonCheck();
            PlayerPrefs.SetFloat("pearls", player.newPearls);
            PlayerPrefs.Save();
        }
        newPearls = Mathf.Lerp(newPearls, pearls, 3 * Time.deltaTime);
        pearlCount.text = newPearls.ToString("0");
        pointCount.text = "POINTS:\n" + player.points.ToString("0");

        // shield
        if (player.shield && shieldTimer > 0)
        {
            shieldTimer -= 1 * Time.deltaTime;
            shieldScale.fillAmount = shieldTimer / shieldTimerMax;
        }
        else if(player.shield || (!player.shield && shieldTimer != 0))
        {
            player.ShieldOff();
            shieldScale.fillAmount = 1;
            shieldTimer = 0;
        }

        // claw punch
        if (player.claws && clawTimer > 0)
        {
            clawTimer -= 1 * Time.deltaTime;
            clawsScale.fillAmount = clawTimer / clawTimerMax;
        }
        else if (player.claws || (!player.claws && clawTimer != 0))
        {
            player.claws = false;
            clawsScale.fillAmount = 1;
            shieldTimer = 0;
        }

        // music
        if (loadingScreen.activeSelf == true)
        {
            foreach (AudioSource audio in player.sounds)
            {
                audio.volume -= 0.1f;
            }
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
            foreach (AudioSource audio in player.sounds)
            {
                audio.volume = audioSlider.value;
            }
        }

        // levels
        if ((mode == 4 || mode == 6 || mode == 9) && !player.playAgain.activeSelf && !player.winScreen.activeSelf)
        {
            if(levelTimer > 0)
            {
                   levelTimer -= Time.deltaTime;
                levelTimerCount.text = "SURVIVE " + levelTimer.ToString("0") + "S";
            }
            else
            {
                player.winScreen.SetActive(true);
                winCount();
            }
        }
        else if(levelMetresTarget != 0)
        {
            float distance = levelMetresTarget - player.transform.position.z;
            levelTimerCount.text = "SWIM THE DISTANCE: " + distance.ToString("0");
            if (distance <= 0 && !player.winScreen.activeSelf)
            {
                player.winScreen.SetActive(true);
                winCount();
            }
        }
        
    }

    public void NextLevel()
    {
        player.sounds[3].Play();
        if (mode <= 9)
        {
            PlayerPrefs.SetFloat("mode", mode += 1);
            PlayerPrefs.Save();
            reloadScene();
        }
    }

    public void ExitMenu()
    {
        player.sounds[3].Play();
        player.swipesBlocked = true;
        loading.fillAmount = 0;
        loadingScreen.SetActive(true);
    }

    public void Settings()
    {
        player.swipesBlocked = true;
        player.sounds[3].Play();
        if (!pause)
        {
            Time.timeScale = 0;
            pause = true;
            settings.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pause = false;
            settings.SetActive(false);
            PlayerPrefs.SetFloat("volume", audioSlider.value);
            PlayerPrefs.Save();
        }
    }

    public void InstantiatePlatform(Vector3 lastPlatformPlace)
    {
        platformPlace = new Vector3(platformPlace.x, platformPlace.y, lastPlatformPlace.z + 50);
        newPlatform = Instantiate(platform, transform.position, Quaternion.identity, platformHolder.transform);
        newPlatform.transform.localPosition = platformPlace;
        newPlatform.GetComponent<Platform>().generalController = this;
    }
    public void DestroyPlatform()
    {
        if (lastPlatform)
        {
            Destroy(lastPlatform.shark);
            Destroy(lastPlatform.chosenShell);
            Destroy(lastPlatform.gameObject);
        }
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ShieldOn()
    {
        player.swipesBlocked = true;
        player.sounds[3].Play();
        if (pearls >= shieldCost && !player.shield)
        {
            player.newPearls -= shieldCost;
            shieldScale.fillAmount = 1;
            player.shieldBubble.SetActive(true);
            player.shield = true;
            shieldTimer = 60;
        }
    }
    public void ClawOn()
    {
        player.swipesBlocked = true;
        player.sounds[3].Play();
        if (pearls >= clawCost && !player.claws)
        {
            player.newPearls -= clawCost;
            player.claws = true;
            clawsScale.fillAmount = 1;
            clawTimerMax = Random.Range(20, 30);
            clawTimer = clawTimerMax;
        }
    }
    public void clawPressed()
    {
        player.swipesBlocked = true;
        player.punch = true;
        player.LineOn();
    }

    private void buttonCheck()
    {
        if (pearls < shieldCost)
        {
            buttons[1].color = enableButton;
        }
        else
        {
            buttons[1].color = normalButton;
        }
        if (pearls < clawCost)
        {
            buttons[0].color = enableButton;
        }
        else
        {
            buttons[0].color = normalButton;
        }
    }

    // for levels

    private void levelCheckShell()
    {
        levelShellsTarget -= 1;
        levelTimerCount.text = "FIND PEARLS: " + levelShellsTarget.ToString("0");
        // levels conditions
        if (levelShellsTarget == 0)
        {
            player.winScreen.SetActive(true);
            winCount();
        }
    }


    private void winCount()
    {
        if(mode > howManyLevelsDone)
        {
            PlayerPrefs.SetInt("howManyLevelsDone", (int)mode);
            PlayerPrefs.Save();
        }
    }
}
