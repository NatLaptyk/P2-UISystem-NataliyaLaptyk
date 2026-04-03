using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; 
    [SerializeField] private float jumpForce = 10f;
     private Rigidbody rb;
     [SerializeField] private LayerMask interactableLayer;
     [SerializeField] private LayerMask groundLayer;
     [SerializeField] private Transform groundCheck;

     [SerializeField] private float groundCheckRadius = 0.2f;
     [SerializeField] private float rayLength = 15f;
     [SerializeField] private float fallMultiplier = 2.5f;
     private PlayerInputActions playerInput;
     private InputAction moveAction;
     private InputAction jumpAction;
     private InputAction interactAction;
     private Vector2 moveInput;
     private Vector2 facingDirection = Vector2.right;
     private bool isGrounded;
     private Interactable currentInteractable;
     

     private void Awake()
    {
        playerInput = new PlayerInputActions();
        if (!TryGetComponent(out rb))
        {
            Debug.LogError($"PlayerMovement on {gameObject.name} requires a Rigidbody.");
        }
    }
     private void OnEnable()
    {
        if (playerInput == null) //guard clause
        {
            playerInput = new PlayerInputActions();
        }

        moveAction = playerInput.Player.Move;
        jumpAction = playerInput.Player.Jump;
        interactAction = playerInput.Player.Interact;

        moveAction.Enable();
        jumpAction.Enable();
        interactAction.Enable();

        jumpAction.performed += OnJump;
        interactAction.performed += OnInteract;
    }
    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        interactAction.Disable();

        jumpAction.performed -= OnJump;
        interactAction.performed -= OnInteract;
    }
    private void Update() 
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
           GameManager.Instance.LightMatch();
        } 
    }
    private void FixedUpdate()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
        Vector3 facingDirection3D = new Vector3(facingDirection.x, 0, 0);
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, 0f);

        if (moveInput != Vector2.zero)
        {
            facingDirection = moveInput;
            transform.rotation = Quaternion.Euler(0, moveInput.x > 0 ? 0 : 180, 0);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        Vector3 rayOrigin = transform.position + Vector3.up * 0.7f;
         bool didHit = Physics.Raycast(rayOrigin, facingDirection3D, out RaycastHit hit, rayLength, interactableLayer);
         Debug.DrawRay(rayOrigin, facingDirection3D * rayLength, Color.red);

     if (didHit)
        {
    Interactable newTarget = hit.collider.GetComponent<Interactable>();

    if (newTarget != currentInteractable)
    {
        if (currentInteractable != null)
        {
            currentInteractable.SetHighlight(false);
        }

        currentInteractable = newTarget;

        if (currentInteractable != null)
        {
            currentInteractable.SetHighlight(true);
        }
    }
        }
        else if (currentInteractable != null)
        {
            currentInteractable.SetHighlight(false);
            currentInteractable = null;
        }

        if (rb.linearVelocity.y < 0f)
       {
    rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
       }
    }
    
    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }
    private void OnInteract(InputAction.CallbackContext context)
{
        if (context.performed && currentInteractable != null)
    {
       
        if (TryGetComponent(out PlayerInventory inventory))
        {
            currentInteractable.Interact(inventory);
        }
        else
        {
            Debug.LogWarning("No PlayerInventory found!");
        }
    }
}

   
}
