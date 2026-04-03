using UnityEngine;
using TMPro;

public class HeatTimerUI : MonoBehaviour
{   [SerializeField] private TMP_Text m_HeatText;
    [SerializeField] private string prefix = "Heat: ";

    private void Awake()
    {
        if(m_HeatText == null && !TryGetComponent (out m_HeatText))
        {
            Debug.LogWarning($"HeatTimerUI on {gameObject.name} has no reference to TMP_Text component. Attempting to use one on the same GameObject.");
            enabled = false;
        }
    }
    
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHeatChanged += UpdateHeatText;
        }
        UpdateHeatText(0);        
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHeatChanged -= UpdateHeatText;
        }
    }

    private void UpdateHeatText (float currentHeat)
    {
        m_HeatText.text = prefix + currentHeat.ToString("F0");
    }
// The "F0" part is a format specifier — it tells ToString exactly how to format the number:
// F means Fixed point (regular decimal number)
//0 means 0 decimal places
    
}
