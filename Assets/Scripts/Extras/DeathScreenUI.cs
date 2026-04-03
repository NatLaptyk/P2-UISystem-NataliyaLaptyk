using UnityEngine;
using System.Collections;

public class DeathScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject m_DeathPanel;

    private void Awake()
    {
        if (m_DeathPanel == null)
            Debug.LogWarning($"DeathScreenUI on {gameObject.name}: m_DeathPanel not assigned.");
        else
            m_DeathPanel.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(SubscribeNextFrame());
    }

    private IEnumerator SubscribeNextFrame()
    {
        // Wait one frame — GameManager.Instance is guaranteed to exist after first frame
        yield return null;

        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerDied += ShowDeathScreen;
        else
            Debug.LogError("DeathScreenUI: GameManager still null after one frame!");
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerDied -= ShowDeathScreen;
    }

    private void ShowDeathScreen()
    {
            if (m_DeathPanel != null)
            m_DeathPanel.SetActive(true);
    }
}