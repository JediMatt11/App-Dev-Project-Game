using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider FOVSlider;
    public Button closeButton;

    public static Settings instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PlayerPrefs.SetFloat("Volume", 0.75f);
            PlayerPrefs.SetFloat("FOV", 95f);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        FOVSlider = GameObject.Find("FOVSlider").GetComponent<Slider>();
        closeButton = GameObject.Find("CloseSettingsBtn").GetComponent<Button>();
        closeButton.onClick.AddListener(CloseSettings);
        
        float savedVolume = PlayerPrefs.GetFloat("Volume");
        float savedFOV = PlayerPrefs.GetFloat("FOV");
        volumeSlider.value = savedVolume;
        FOVSlider.value = savedFOV;

        ApplyVolume(savedVolume);
        ApplyFOV(savedFOV);

        volumeSlider.onValueChanged.AddListener(SetVolume);
        FOVSlider.onValueChanged.AddListener(SetFOV);
    }

    private void SetVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
        ApplyVolume(value);
    }

    private void SetFOV(float value)
    {
        PlayerPrefs.SetFloat("FOV", value);
        PlayerPrefs.Save();
        ApplyFOV(value);
    }

    private void ApplyVolume(float value)
    {
        AudioListener.volume = value;
    }

    private void ApplyFOV(float value)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }
        Camera mainCam = Camera.main;

        if (mainCam != null)
        {
            mainCam.fieldOfView = value;
        }
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
    }

}