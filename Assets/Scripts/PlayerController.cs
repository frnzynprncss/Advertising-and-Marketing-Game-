using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 3f;
    public float steerSpeed = 120f; // degrees per second

    [Header("Balance")]
    public float balance = 100f;
    public float maxBalance = 100f;
    public float maxSafeTilt = 15f; 
    public float balanceLossRate = 10f; 
    public float balanceRecoverRate = 8f; 

    [Header("Protection")]
    public float protection = 0f;
    public float maxProtection = 100f;
    public float protectionDuration = 5f;

    [Header("State")]
    public bool isProtected = false;


    Rigidbody2D rb;
    float slowFactor = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
        BalanceTick();
    }

    void FixedUpdate()
    {
        // forward motion (transform.up is forward)
        rb.velocity = transform.up * forwardSpeed * (1f - slowFactor);
    }

    void HandleInput()
    {
        float steerInput = 0f;

#if UNITY_EDITOR || UNITY_STANDALONE
        steerInput = Input.GetAxis("Horizontal"); // -1 .. 1 for testing
#else
        // Touch steering: left-half screen = left; right-half = right
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            steerInput = (t.position.x < Screen.width * 0.5f) ? -1f : 1f;
        }
        else
        {
            // accelerometer fallback
            steerInput = Input.acceleration.x;
        }
#endif
        float rotation = -steerInput * steerSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation + rotation);
    }

    void BalanceTick()
    {
        float tilt = NormalizeAngle(transform.eulerAngles.z);
        float absTilt = Mathf.Abs(tilt);

        if (absTilt > maxSafeTilt)
        {
            float over = (absTilt - maxSafeTilt) / (90f - maxSafeTilt); // normalized
            balance -= over * balanceLossRate * Time.deltaTime;
        }
        else
        {
            balance = Mathf.Min(balance + balanceRecoverRate * Time.deltaTime, maxBalance);
        }

        // clamp and check lose
        balance = Mathf.Clamp(balance, 0f, maxBalance);
        if (balance <= 0f)
        {
            GameManager.Instance.Lose("Image dropped");
        }
    }

    float NormalizeAngle(float a)
    {
        while (a > 180f) a -= 360f;
        while (a <= -180f) a += 360f;
        return a;
    }

    // called by Pickup/Hazard scripts
    public void AddProtection(float amount)
    {
        protection = Mathf.Clamp(protection + amount, 0f, maxProtection);
        if (protection >= maxProtection && !isProtected)
            StartCoroutine(ActivateProtection());
    }

    IEnumerator ActivateProtection()
    {
        isProtected = true;
        protection = 0f;
        // TODO: visual cue (glow)
        yield return new WaitForSeconds(protectionDuration);
        isProtected = false;
    }

    public void RestoreBalance(float amount)
    {
        balance = Mathf.Clamp(balance + amount, 0f, maxBalance);
    }

    public void ApplyGuidance(float speedMultiplier, float duration)
    {
        StartCoroutine(TempSpeedBoost(speedMultiplier, duration));
    }

    IEnumerator TempSpeedBoost(float mul, float dur)
    {
        float old = forwardSpeed;
        forwardSpeed *= mul;
        yield return new WaitForSeconds(dur);
        forwardSpeed = old;
    }

    public void ApplySlow(float slow, float duration)
    {
        StopCoroutine("ClearSlow");
        slowFactor = slow;
        StartCoroutine(ClearSlow(duration));
    }

    IEnumerator ClearSlow(float dur)
    {
        yield return new WaitForSeconds(dur);
        slowFactor = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickup"))
        {
            var p = other.GetComponent<Pickup>();
            if (p != null) p.Collect(this);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Hazard"))
        {
            var h = other.GetComponent<Hazard>();
            if (h != null) h.Hit(this);
        }
        else if (other.CompareTag("Destination"))
        {
            GameManager.Instance.Win();
        }
    }
}
