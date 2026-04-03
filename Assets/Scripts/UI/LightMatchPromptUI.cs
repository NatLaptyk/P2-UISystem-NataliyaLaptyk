using UnityEngine;
using TMPro;

public class LightMatchPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_PromptText;

    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnBurnStateChanged += OnBurnStateChanged;
        
        UpdatePrompt();
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnBurnStateChanged -= OnBurnStateChanged;
    }

    private void OnBurnStateChanged(bool isLit)
    {
        UpdatePrompt();
    }

    private void UpdatePrompt()
    {
        if (m_PromptText == null) return;
        // Show only when not currently burning
        bool shouldShow = !GameManager.Instance.IsMatchLit;
        m_PromptText.enabled = shouldShow;
    }
}