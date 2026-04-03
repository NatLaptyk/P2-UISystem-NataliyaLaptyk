using UnityEngine;

// KillZone.cs
// Triggers the death screen when the player enters this trigger zone.
// Uses GameManager.Instance to fire OnPlayerDied — no direct coupling to UI.
public class KillZone : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out Collider col) && !col.isTrigger)
        {
            Debug.LogWarning($"KillZone on {gameObject.name}: Collider should be set to Is Trigger.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("KillZone: GameManager.Instance is null!");
            return;
        }

        GameManager.Instance.TriggerDeath();
    }
}