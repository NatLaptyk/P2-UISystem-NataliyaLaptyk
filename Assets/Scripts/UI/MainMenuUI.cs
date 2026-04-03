using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPanel; 
    [SerializeField] private Toggle soundToggle;
    void Awake() 
    {
        if (SettingsPanel !=null)
        {
            SettingsPanel.SetActive (false);
        }
        else
        {
            Debug.LogWarning($"MainMenuUI on {gameObject.name}: Settings panel refrence is not set in the inspector.");
        }

        if (soundToggle == null && !TryGetComponent (out soundToggle))
        {
            Debug.LogError($"Settings panel on {gameObject.name} requires a Toggle component.");
        }

    }

    public void OnStartClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Scene");
    }

    public void OnSettingsClicked()
    {
        bool isActive = SettingsPanel.activeSelf;
        SettingsPanel.SetActive (!isActive);   
    }
       public void OnBackClicked()
    {
        SettingsPanel.SetActive(false);
    }
       
       public void OnExitClicked()
    {
        Application.Quit();
    }

    public void OnSoundToggleChanged (bool isOn)
    {
        AudioListener.pause = !isOn;
    }

}
