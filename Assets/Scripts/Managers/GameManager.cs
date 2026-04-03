using UnityEngine;

// Using C# events for OnHeatChanged, OnMatchCountChanged, OnBurnTimerChanged
// because they are code-driven, high-frequency updates (heat drains every frame)
// and don't need Inspector configuration — C# events are faster than UnityEvents
// for per-frame updates. 
// UnityEvents are better for designer-driven events that need Inspector hookup.
public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    // Fields
    [SerializeField] private float maxHeat = 1000f;
    [SerializeField] private float heatDrainRate = 1f;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private string revealLayerName = "Reveal";
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryUI inventoryUI;

    [Header("Match FX")]
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private Light m_MatchLight;
    [SerializeField] private Transform m_FxAnchor;

    private int revealLayer;
    private float currentHeat;
    private int matchCount;
    private float burnTimer;
    private bool isBurning;
    private bool _isDead;
    private ParticleSystem m_BurnLoopVfx;
    private MatchData m_LastLitMatch;

    public bool IsMatchLit => isBurning;

    // Events
    public event System.Action<float> OnHeatChanged;
    public event System.Action<int> OnMatchCountChanged;
    public event System.Action<float> OnBurnTimerChanged;
    public event System.Action<bool> OnBurnStateChanged;
    public event System.Action OnPlayerDied;

    private void Awake()
    {
        revealLayer = LayerMask.NameToLayer(revealLayerName);
        if (m_Camera == null)
            m_Camera = Camera.main;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Make sure light is off at game start
        if (m_MatchLight != null)
        m_MatchLight.enabled = false;

        currentHeat = maxHeat;
        OnHeatChanged?.Invoke(currentHeat);
    }

    private void Update()
    {
        currentHeat -= heatDrainRate * Time.deltaTime;
        currentHeat = Mathf.Clamp(currentHeat, 0f, maxHeat);
        OnHeatChanged?.Invoke(currentHeat);

          // Check for death
          if (currentHeat <= 0f && !_isDead)
          {
            Debug.Log("Death condition triggered!");
            _isDead = true;
            OnPlayerDied?.Invoke();
          }

        if (isBurning)
        {
            burnTimer -= Time.deltaTime;

            if (burnTimer <= 0f)
            {
                isBurning = false;
                burnTimer = 0f;
                OnBurnStateChanged?.Invoke(false);
                HideRevealLayer();

                if (m_MatchLight != null)
                    m_MatchLight.enabled = false;

                // Stop burn VFX
                if (m_BurnLoopVfx != null)
                {
                    m_BurnLoopVfx.Stop();
                    Destroy(m_BurnLoopVfx.gameObject, 1f);
                    m_BurnLoopVfx = null;
                }

                // Play extinguish VFX
                if (m_LastLitMatch != null && m_LastLitMatch.ExtinguishVfxPrefab() != null && m_FxAnchor != null)
                {
                    ParticleSystem extVfx = Instantiate(
                        m_LastLitMatch.ExtinguishVfxPrefab(),
                        m_FxAnchor.position,
                        m_FxAnchor.rotation);
                    extVfx.Play();
                    Destroy(extVfx.gameObject, 2f);
                }
            }

            OnBurnTimerChanged?.Invoke(burnTimer);
        }
    }

    public void AddMatch()
    {
        matchCount++;
        OnMatchCountChanged?.Invoke(matchCount);
    }

    public void LightMatch()
    {
        

        if (isBurning) return;

        if (inventoryUI == null)
        {
            Debug.LogError("GameManager: inventoryUI is not assigned!");
            return;
        }
        if (playerInventory == null)
        {
            Debug.LogError("GameManager: playerInventory is not assigned!");
            return;
        }

        MatchData selected = inventoryUI.SelectedMatch;

        if (selected == null)
        {
            Debug.LogWarning("No match selected!");
            return;
        }
        inventoryUI.CacheBurningSlot();

        bool removed = playerInventory.UseItem(selected);
        if (!removed)
        {
            Debug.LogWarning("Match not in inventory!");
            return;
        }

        isBurning = true;
        burnTimer = selected.BurnDuration();
        m_LastLitMatch = selected;

        OnBurnTimerChanged?.Invoke(burnTimer);
        OnBurnStateChanged?.Invoke(true);
        ShowRevealLayer();

        int total = 0;
        foreach (var entry in playerInventory.HeldItems)
            total += entry.Value;
        OnMatchCountChanged?.Invoke(total);

        PlayMatchFX(selected);
    }

    private void PlayMatchFX(MatchData matchToLight)
    {
        Debug.Log($"PlayMatchFX called with: {matchToLight.DisplayName()}");
        Debug.Log($"AudioSource: {m_AudioSource}, IgniteSfx: {matchToLight.IgniteSfx()}");
        Debug.Log($"MatchLight: {m_MatchLight}");
        Debug.Log($"BurnLoopVfx: {matchToLight.BurnLoopVfxPrefab()}, FxAnchor: {m_FxAnchor}");

        if (m_AudioSource != null && matchToLight.IgniteSfx() != null)
            m_AudioSource.PlayOneShot(matchToLight.IgniteSfx());

        if (m_MatchLight != null)
        {
            m_MatchLight.enabled = true;
            m_MatchLight.range = matchToLight.LightRange();
            m_MatchLight.intensity = matchToLight.LightIntensity();
        }

        if (matchToLight.BurnLoopVfxPrefab() != null && m_FxAnchor != null)
        {
            if (m_BurnLoopVfx != null) Destroy(m_BurnLoopVfx.gameObject);

            m_BurnLoopVfx = Instantiate(
                matchToLight.BurnLoopVfxPrefab(),
                m_FxAnchor.position,
                m_FxAnchor.rotation,
                m_FxAnchor);
            m_BurnLoopVfx.Play();
        }
    }

    private void ShowRevealLayer()
    {
        if (m_Camera == null || revealLayer == -1) return;
        m_Camera.cullingMask |= (1 << revealLayer);
    }

    private void HideRevealLayer()
    {
        if (m_Camera == null || revealLayer == -1) return;
        m_Camera.cullingMask &= ~(1 << revealLayer);
    }
    public void TriggerDeath()
{
    if (_isDead) return;
    _isDead = true;
    OnPlayerDied?.Invoke();
}

    [ContextMenu("Test: Add Match")]
    private void TestAddMatch() => AddMatch();

    [ContextMenu("Test: Light Match")]
    private void TestLightMatch() => LightMatch();
}