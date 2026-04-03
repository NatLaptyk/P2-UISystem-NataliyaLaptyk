using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    [SerializeField] private Button m_RestartButton;
public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Awake()
    {
        if (m_RestartButton == null && !TryGetComponent(out m_RestartButton))
        {
            Debug.LogError($"Restart button on {gameObject.name} requires a Button component.");
        }
    }
   
    }
