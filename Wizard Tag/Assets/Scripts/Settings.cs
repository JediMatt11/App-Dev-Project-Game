using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider FOVSlider;
    public Button closeButton;
    public GameObject panel;

    public static Settings instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        panel.SetActive(false);
        volumeSlider = panel.transform.GetChild(2).gameObject.GetComponent<Slider>();
        FOVSlider = panel.transform.GetChild(1).gameObject.GetComponent<Slider>();
        closeButton = panel.transform.GetChild(3).gameObject.GetComponent<Button>();
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
        panel.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                panel.SetActive(!panel.activeSelf);
                Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = panel.activeSelf ? true : false;
            }
        }
    }

}