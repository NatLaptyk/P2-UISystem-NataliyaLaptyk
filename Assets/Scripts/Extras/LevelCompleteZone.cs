using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteZone : MonoBehaviour
{
    [SerializeField] private string endSceneName = "EndScene";

    private bool m_Triggered = false; // guard against multiple triggers

    private void Awake()
    {
        if (TryGetComponent(out Collider col) && !col.isTrigger)
        {
            Debug.LogWarning($"LevelCompleteZone on {gameObject.name}: Collider should be set to Is Trigger.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_Triggered) return;
        if (!other.CompareTag("Player")) return;

        m_Triggered = true;
        SceneManager.LoadScene(endSceneName);
    }
}