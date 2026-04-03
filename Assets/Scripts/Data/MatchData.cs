using UnityEngine;
public enum MatchType { None, NormalMatch, BrightMatch, FragileMatch, SpecialMatch }

[CreateAssetMenu(menuName = "MatchGirl/Match Data", fileName = "MatchData_")]
public class MatchData : ScriptableObject

{
    // Fields use [SerializeField] private with public read-only methods
    
    [Header("Identity")]
    [SerializeField] private string displayName = "Match";
    public string DisplayName() {return displayName;}
    [SerializeField] private MatchType matchType = MatchType.NormalMatch;
    public MatchType Type() { return matchType; }

    [SerializeField] private Sprite icon;
    public Sprite Icon() { return icon; }

    [Header("Burn")]
    [Min(0.1f)] [SerializeField] private float burnDuration = 6f;
    public float BurnDuration() {return burnDuration;}
   
    [Header("Light")]
    [Min(0f)] [SerializeField] private float lightRange = 12f;
    public float LightRange() {return lightRange;}
    [Min(0f)] [SerializeField] private float lightIntensity = 8f;
    public float LightIntensity() {return lightIntensity;}

    [Header("Warmth While Burning")]
    // Reduces cold while burning (cold units per second)
    [SerializeField] private float warmthPerSecond = 10f;
    public float WarmthPerSecond() {return warmthPerSecond;}

    [Header("Lingering Heat Buff")]
    // Adds to "heat buffer" on ignition; buffer continues helping after match ends
    [SerializeField] private float heatBufferGain = 25f;
    public float HeatBufferGain() {return heatBufferGain;}
    
    [Header("FX")]
    [SerializeField] private AudioClip igniteSfx;
    public AudioClip IgniteSfx() {return igniteSfx;}
    [SerializeField] private AudioClip extinguishSfx;
    public AudioClip ExtinguishSfx() {return extinguishSfx;}
    [SerializeField] private ParticleSystem igniteVfxPrefab;
    public ParticleSystem IgniteVfxPrefab() {return igniteVfxPrefab;}
    [SerializeField] private ParticleSystem burnLoopVfxPrefab;
     public ParticleSystem BurnLoopVfxPrefab() {return burnLoopVfxPrefab;}
    [SerializeField] private ParticleSystem extinguishVfxPrefab;
    public ParticleSystem ExtinguishVfxPrefab() {return extinguishVfxPrefab;}
}
