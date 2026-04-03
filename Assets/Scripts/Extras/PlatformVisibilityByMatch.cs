using UnityEngine;

public class PlatformVisibilityByMatch : MonoBehaviour
{
    [SerializeField] private Renderer[] renderersToToggle;
    [SerializeField] private Collider[] collidersToToggle;

    private void Reset()
    {
        renderersToToggle = GetComponentsInChildren<Renderer>(true);
        collidersToToggle = GetComponentsInChildren<Collider>(true);
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnBurnStateChanged += OnBurnStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnBurnStateChanged -= OnBurnStateChanged;
    }

    private void Start()
    {
        // Subscribe here as fallback since GameManager.Instance
        // may be null during OnEnable at scene start
         if (GameManager.Instance != null)
    {
        GameManager.Instance.OnBurnStateChanged += OnBurnStateChanged;
        Debug.Log($"PlatformVisibilityByMatch on {gameObject.name}: subscribed successfully");
    }
    else
        Debug.LogError($"PlatformVisibilityByMatch on {gameObject.name}: GameManager null in Start!");

    SetVisibility(false);
    }

    private void OnBurnStateChanged(bool isLit)
    {
        Debug.Log($"PlatformVisibilityByMatch on {gameObject.name}: OnBurnStateChanged={isLit}");
        SetVisibility(isLit);
    }

    private void SetVisibility(bool visible)
    {
        foreach (var r in renderersToToggle)
            if (r != null) r.enabled = visible;

        foreach (var c in collidersToToggle)
            if (c != null) c.enabled = visible;
    }
}