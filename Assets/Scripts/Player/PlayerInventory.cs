using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Using UnityEvent instead of C# event for OnInventoryChanged
// because the game is small so performance is not a concern,
// and easier setup is preferred for a solo developer.
public class PlayerInventory : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxCapacity = 4; // max unique match types

    [Header("Events")]
    [SerializeField] private UnityEvent onInventoryChanged;
    public UnityEvent OnInventoryChanged => onInventoryChanged;

    private Dictionary<MatchData, int> m_heldItems = new Dictionary<MatchData, int>();
    public Dictionary<MatchData, int> HeldItems => m_heldItems;

    private void Awake()
    {
        m_heldItems = new Dictionary<MatchData, int>();
    }
    
    public bool PickUp(MatchData matchData)
    {
        if (matchData == null) return false;

        // If already have this type, just increment count
        if (m_heldItems.ContainsKey(matchData))
        {
            m_heldItems[matchData]++;
            OnInventoryChanged?.Invoke();
            return true;
        }

        // New type — check capacity
        if (m_heldItems.Count >= _maxCapacity)
        {
            Debug.LogWarning("Inventory full — cannot add new match type.");
            return false;
        }

        m_heldItems[matchData] = 1;
        OnInventoryChanged?.Invoke();
        return true;
    }

    [ContextMenu("Use first match")]
    public bool UseFirst()
    {
        if (m_heldItems.Count == 0)
        {
            return false;
        }
        // Get first entry
        foreach (KeyValuePair<MatchData, int> entry in m_heldItems)
        {
            if (entry.Value > 1)
                m_heldItems[entry.Key]--;
            else
                m_heldItems.Remove(entry.Key);

            OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }
    public bool UseItem(MatchData matchData)
{
    if (matchData == null) return false;

    if (!m_heldItems.ContainsKey(matchData))
    {
        Debug.LogWarning($"[Inventory] {matchData.DisplayName()} not in inventory.");
        return false;
    }

    if (m_heldItems[matchData] > 1)
        m_heldItems[matchData]--;
    else
        m_heldItems.Remove(matchData);

    OnInventoryChanged?.Invoke();
    return true;
}
}