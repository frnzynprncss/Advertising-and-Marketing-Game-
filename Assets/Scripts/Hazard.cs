using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float balanceDamage = 20f;
    public float slowAmount = 0.5f; // fraction to slow: 0.5 => 50% speed
    public float slowDuration = 2f;

    public void Hit(PlayerController player)
    {
        if (player.isProtected) return; // no effect while protected
        player.RestoreBalance(-balanceDamage); // restore negative to subtract
        player.ApplySlow(slowAmount, slowDuration);
        // Add camera shake, sfx here.
    }
}
