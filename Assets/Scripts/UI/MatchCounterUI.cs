using UnityEngine;
using TMPro;

public class MatchCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_MatchText;
    [SerializeField] private string prefix = "Matches: ";

    private void Awake()
    {
        if(m_MatchText == null && !TryGetComponent (out m_MatchText))
        {
            Debug.LogWarning($"MatchCounterUI on {gameObject.name} has no reference to a TMP_Text component. Attempting to use in the same GameObject.");
            enabled = false;
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMatchCountChanged += UpdateMatchText;
        }
        UpdateMatchText(0);
    }

    private void OnDisable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnMatchCountChanged -= UpdateMatchText;
        }
    }

    private void UpdateMatchText(int matchCount)
    {
        m_MatchText.text = prefix + matchCount;
    }

}
