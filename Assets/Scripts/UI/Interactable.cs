using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
   [SerializeField] private Color highlightColor = Color.blue;
   [SerializeField] private MatchData matchData;
   [SerializeField] private TextMeshPro interactionPrompt;
   private Color originalColor; 
   private Renderer objectRenderer;
    private void Awake()
    {
        // Hide prompt at start
       if (interactionPrompt != null)
           interactionPrompt.gameObject.SetActive(false);

     if (!TryGetComponent(out objectRenderer))
        {
            Debug.LogWarning($"Interactable on {gameObject.name} has no Renderer component.");
        }
    
    else
        {
           originalColor = objectRenderer.material.color;   
        }
    if (matchData == null)
        {
            Debug.LogWarning($"Interactable on {gameObject.name} has no reference to MatchData.");
        }
        
        
     }
    public void SetHighlight(bool isActive)
    {
        if (objectRenderer == null) return;
        if (isActive)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetColor("_EmissionColor", highlightColor);
        }
        else
        {
            objectRenderer.material.color = originalColor;
            objectRenderer.material.SetColor("_EmissionColor", Color.black);
        }
        if (interactionPrompt != null)
            interactionPrompt.gameObject.SetActive(isActive);
    }
    public void Interact(PlayerInventory inventory)
    {
             
         if (matchData != null)
        {
            GameManager.Instance.AddMatch();
            inventory.PickUp(matchData);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Interactable on {gameObject.name} has no MatchData assigned.");
        } 
    }
  }