using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_matchText;
    [SerializeField] private TMP_Text m_CountText;
    [SerializeField] private Image m_SlotBackground;
    [SerializeField] private Image m_BurnFill;

    private void Awake()
    {
        if (m_Icon == null)
            Debug.LogError("InventorySlotUI: m_Icon not assigned.", this);
        if (m_matchText == null)
            Debug.LogError("InventorySlotUI: m_matchText not assigned.", this);
        if (m_CountText == null)
            Debug.LogError("InventorySlotUI: m_CountText not assigned.", this);
        if (m_SlotBackground == null)
            Debug.LogError("InventorySlotUI: m_SlotBackground not assigned.", this);
        if (m_BurnFill == null)
            Debug.LogError("InventorySlotUI: m_BurnFill not assigned.", this);
    }

    public void SetData(MatchData matchData, int count)
    {
        if (matchData == null)
        {
            m_Icon.sprite = null;
            m_matchText.text = "";
            m_CountText.text = "";
            return;
        }

        m_Icon.sprite = matchData.Icon();
        m_matchText.text = matchData.DisplayName();
        m_CountText.text = count > 1 ? $"x{count}" : "";

        if (m_BurnFill != null)
        {
            m_BurnFill.sprite = matchData.Icon();
            m_BurnFill.fillAmount = 0f;
            m_BurnFill.gameObject.SetActive(false);
        }
    }

    public void SetHighlight(bool isActive)
    {
        if (m_SlotBackground == null) return;
        m_SlotBackground.color = isActive
            ? new Color(1f, 0.8f, 0f, 1f)
            : new Color(0.2f, 0.2f, 0.2f, 1f);
    }

    public void UpdateBurnProgress(float normalizedProgress)
    {
        Debug.Log($"Slot UpdateBurnProgress: normalized={normalizedProgress}, BurnFill null={m_BurnFill == null}, BurnFill active={m_BurnFill?.gameObject.activeSelf}");
        if (m_BurnFill == null) return;
        m_BurnFill.gameObject.SetActive(normalizedProgress > 0f);
        m_BurnFill.fillAmount = normalizedProgress;
    }
}