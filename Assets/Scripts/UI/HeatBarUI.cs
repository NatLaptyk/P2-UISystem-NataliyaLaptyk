using UnityEngine;
using UnityEngine.UI;

public class HeatBarUI : MonoBehaviour
{
    [SerializeField] private Slider m_HeatSlider;

    private void Start()
    {
        if (m_HeatSlider == null && !TryGetComponent (out m_HeatSlider))
        {
            Debug.LogError($"HeatBarUI on {gameObject.name} requires a Slider component.");
        }
         
        m_HeatSlider.value = 1000f;
        m_HeatSlider.minValue = 0f;
        m_HeatSlider.maxValue = 1000f;
        m_HeatSlider.interactable = false;

       if (GameManager.Instance != null)
       GameManager.Instance.OnHeatChanged += UpdateHeatBar;
    }
    private void OnDisable()
    {
      if(GameManager.Instance != null)
      GameManager.Instance.OnHeatChanged -= UpdateHeatBar;
    }
    private void UpdateHeatBar(float currentHeat)
    {
      m_HeatSlider.value = currentHeat;
    }

}
