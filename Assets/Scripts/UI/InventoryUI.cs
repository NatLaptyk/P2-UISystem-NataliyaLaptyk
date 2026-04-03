using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotsParent;

    private List<InventorySlotUI> m_activeSlots = new List<InventorySlotUI>();
    private int m_selectedIndex = -1;
    private bool m_isBurning = false;
    private InventorySlotUI m_burningSlot = null;
    private MatchData m_burningMatchData = null;
    private float m_burningDuration = 1f;

    private void Awake()
    {
        if (_playerInventory == null)
            Debug.LogError("InventoryUI: _playerInventory not assigned.", this);
        if (_slotPrefab == null)
            Debug.LogError("InventoryUI: _slotPrefab not assigned.", this);
        if (_slotsParent == null)
            Debug.LogError("InventoryUI: _slotsParent not assigned.", this);
    }

    private void OnEnable()
    {
        if (_playerInventory != null)
            _playerInventory.OnInventoryChanged.AddListener(Refresh);
    }

    private void OnDisable()
    {
        if (_playerInventory != null)
            _playerInventory.OnInventoryChanged.RemoveListener(Refresh);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBurnTimerChanged -= UpdateBurnProgress;
            GameManager.Instance.OnBurnStateChanged -= OnBurnStateChanged;
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBurnTimerChanged += UpdateBurnProgress;
            GameManager.Instance.OnBurnStateChanged += OnBurnStateChanged;
        }
        else
        {
            Debug.LogError("InventoryUI: GameManager.Instance is null in Start!");
        }

        Refresh();
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectSlot(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectSlot(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectSlot(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectSlot(3);
    }

    private void SelectSlot(int index)
    {
        if (index < 0 || index >= m_activeSlots.Count) return;

        if (m_selectedIndex >= 0 && m_selectedIndex < m_activeSlots.Count)
            m_activeSlots[m_selectedIndex].SetHighlight(false);

        m_selectedIndex = index;
        m_activeSlots[m_selectedIndex].SetHighlight(true);
    }

    public MatchData SelectedMatch
    {
        get
        {
            if (m_selectedIndex < 0) return null;
            int i = 0;
            foreach (KeyValuePair<MatchData, int> entry in _playerInventory.HeldItems)
            {
                if (i == m_selectedIndex) return entry.Key;
                i++;
            }
            return null;
        }
    }

    public void CacheBurningSlot()
    {
        if (m_selectedIndex < 0 || m_selectedIndex >= m_activeSlots.Count) return;

        m_burningSlot = m_activeSlots[m_selectedIndex];
        m_burningMatchData = SelectedMatch;

        // Cache duration separately so it survives Refresh()
        if (m_burningMatchData != null)
            m_burningDuration = m_burningMatchData.BurnDuration();

        // Show fill at full immediately
        if (m_burningSlot != null)
            m_burningSlot.UpdateBurnProgress(1f);
    }

    public void Refresh()
    {
        int previousIndex = m_selectedIndex;
        m_activeSlots.Clear();
        m_selectedIndex = -1;

        foreach (Transform child in _slotsParent)
        {
            if (m_burningSlot != null && child == m_burningSlot.transform)
                continue;
            Destroy(child.gameObject);
        }

        if (m_burningSlot != null)
        {
            m_burningSlot.transform.SetSiblingIndex(0);
            m_activeSlots.Add(m_burningSlot);
            m_selectedIndex = 0;
        }

        foreach (KeyValuePair<MatchData, int> entry in _playerInventory.HeldItems)
        {
            GameObject slot = Instantiate(_slotPrefab, _slotsParent);
            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();
            if (slotUI != null)
            {
                slotUI.SetData(entry.Key, entry.Value);
                m_activeSlots.Add(slotUI);
            }
            else
            {
                Debug.LogError("InventoryUI: Slot prefab missing InventorySlotUI component.", this);
            }
        }

        if (m_burningSlot == null && previousIndex >= 0 && previousIndex < m_activeSlots.Count)
        {
            m_selectedIndex = previousIndex;
            m_activeSlots[m_selectedIndex].SetHighlight(true);
        }
    }

    private void UpdateBurnProgress(float burnTimer)
    {
        if (m_burningSlot == null) return;
        if (m_selectedIndex < 0 || m_selectedIndex >= m_activeSlots.Count) return;

        float normalized = m_burningDuration > 0f ? burnTimer / m_burningDuration : 0f;
        m_activeSlots[m_selectedIndex].UpdateBurnProgress(normalized);
    }

    private void OnBurnStateChanged(bool isLit)
    {
        
        m_isBurning = isLit;

        if (isLit)
        {
            CacheBurningSlot();
        }
        else
        {
            foreach (var slot in m_activeSlots)
                slot.UpdateBurnProgress(0f);

            if (m_burningSlot != null && !m_activeSlots.Contains(m_burningSlot))
                Destroy(m_burningSlot.gameObject);

            m_burningSlot = null;
            m_burningMatchData = null;
            Refresh();
        }
    }
}