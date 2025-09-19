using UnityEngine;
using UnityEngine.UI; // Needed for the UI Sliders

public class PlayerController : MonoBehaviour
{
    // --- Movement Variables ---
    public float moveSpeed = 5f;
    private float horizontalInput;
    private Rigidbody2D rb;

    // --- Balance System Variables ---
    public Slider balanceBarSlider; // Reference to the UI Slider
    public float maxBalance = 100f;
    private float currentBalance;
    public float balanceDrainRate = 5f; // How fast balance drains naturally
    public float hazardBalanceDamage = 25f; // Balance lost on hitting a hazard

    // --- Protection System Variables ---
    public Slider protectionBarSlider;
    public float maxProtection = 100f;
    private float currentProtection;
    public float protectionGainPerPickup = 25f;
    public bool isImmune = false; // Is the player currently immune?
    public float immunityDuration = 3f; // How long immunity lasts

    // --- Danger State Variables ---
    public bool inDangerState = false;
    public float dangerStateSlowFactor = 0.5f; // e.g., 0.5 = 50% speed
    private float originalMoveSpeed;

    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject (your pagoda)
        rb = GetComponent<Rigidbody2D>();
        originalMoveSpeed = moveSpeed;

        // Initialize Bars
        currentBalance = maxBalance;
        currentProtection = 0f; // Start with empty protection bar

        // Update the UI sliders
        if(balanceBarSlider != null)
        {
            balanceBarSlider.maxValue = maxBalance;
            balanceBarSlider.value = currentBalance;
        }
        if(protectionBarSlider != null)
        {
            protectionBarSlider.maxValue = maxProtection;
            protectionBarSlider.value = currentProtection;
        }
    }

    void Update()
    {
        // 1. Get Player Input
        horizontalInput = Input.GetAxis("Horizontal"); // Returns value between -1 (left) and 1 (right)

        // 2. Handle Balance Drain
        DrainBalance();

        // 3. Check for Loss Condition (Balance depleted)
        if (currentBalance <= 0)
        {
            GameOver("You lost your balance!");
        }
    }

    void FixedUpdate()
    {
        // Move the player's pagoda using physics
        MovePlayer();
    }

    void MovePlayer()
    {
        // Calculate movement velocity
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        // Apply the movement to the Rigidbody
        rb.velocity = movement;
    }

    void DrainBalance()
    {
        // Drain balance over time, faster if in a danger state
        float drainAmount = balanceDrainRate * Time.deltaTime;
        if (inDangerState)
        {
            drainAmount *= 2f; // Drain twice as fast in danger state
        }
        currentBalance -= drainAmount;

        // Update the UI Slider
        if(balanceBarSlider != null)
        {
            balanceBarSlider.value = currentBalance;
        }
    }

    // This function is called when this object collides with another object that has a Collider2D
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if it's a Pickup (White Handkerchief)
        if (other.CompareTag("Pickup"))
        {
            CollectPickup(other.gameObject);
        }

        // 2. Check if it's a Hazard (Rock, Current, Boat)
        // Only check hazards if the player is NOT immune
        else if (other.CompareTag("Hazard") && !isImmune)
        {
            HitHazard();
        }
    }

    void CollectPickup(GameObject pickup)
    {
        // Destroy the pickup object
        Destroy(pickup);

        // Increase protection bar
        currentProtection += protectionGainPerPickup;
        // Clamp it so it doesn't exceed the maximum
        currentProtection = Mathf.Clamp(currentProtection, 0f, maxProtection);

        // Update UI
        if(protectionBarSlider != null)
        {
            protectionBarSlider.value = currentProtection;
        }

        // Check if protection bar is now full, activate immunity
        if (currentProtection >= maxProtection)
        {
            StartCoroutine(ActivateImmunity()); // Start the immunity coroutine
        }
    }

    // Coroutines allow us to easily handle timed events like immunity
    System.Collections.IEnumerator ActivateImmunity()
    {
        isImmune = true;
        // Optional: Visual cue, like changing player color
        // GetComponent<SpriteRenderer>().color = Color.blue;

        // Wait for the immunity duration
        yield return new WaitForSeconds(immunityDuration);

        // After waiting, end immunity
        isImmune = false;
        currentProtection = 0f; // Reset protection bar
        protectionBarSlider.value = 0f; // Update UI
        // GetComponent<SpriteRenderer>().color = Color.white; // Revert visual cue
    }

    void HitHazard()
    {
        // 1. Damage Balance
        currentBalance -= hazardBalanceDamage;
        if(balanceBarSlider != null) balanceBarSlider.value = currentBalance;

        // 2. Apply Danger State (Slow down)
        if (!inDangerState)
        {
            moveSpeed *= dangerStateSlowFactor;
            inDangerState = true;
            // Start a coroutine to reset the danger state after a time
            StartCoroutine(ResetDangerState(2f)); // Reset after 2 seconds
        }

        // Optional: Play a sound effect, screen shake, etc.
        Debug.Log("Hit a Hazard! Balance: " + currentBalance);
    }

    System.Collections.IEnumerator ResetDangerState(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveSpeed = originalMoveSpeed; // Reset speed
        inDangerState = false;
    }

    void GameOver(string message)
    {
        // This is a simple implementation. You can expand this later.
        Debug.Log("Game Over: " + message);
        Time.timeScale = 0; // Freeze the game
        // Here you would typically show a "You Lost" screen with a restart button.
    }
}