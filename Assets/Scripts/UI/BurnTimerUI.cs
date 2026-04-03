using UnityEngine;
using TMPro;

public class BurnTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_BurnText;
    [SerializeField] private string prefix = "Burn: ";

    private void Awake()
    {
        if(m_BurnText == null && !TryGetComponent (out m_BurnText))
        {
            Debug.LogWarning($"BurnTimerUI on {gameObject.name} has no reference to TMP_Text component. Attempting to use one on the same GameObject.");
            enabled = false;
        }
    }
    
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBurnTimerChanged += UpdateBurnText;
        }
        UpdateBurnText(0);        
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBurnTimerChanged -= UpdateBurnText;
        }
    }

    private void UpdateBurnText (float burnTimer)
    {
        m_BurnText.text = prefix + burnTimer.ToString("F1");
    }
// The "F1" part is a format specifier — it tells ToString exactly how to format the number:
// F means Fixed point (regular decimal number)
//1 means 1 decimal place
    
}
