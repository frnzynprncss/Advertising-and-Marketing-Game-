using UnityEngine;

public enum PickupType { Protection, Guidance, Oar }

public class Pickup : MonoBehaviour
{
    public PickupType type = PickupType.Protection;
    public float value = 25f; // protection fill or balance restore
    public float guidanceDuration = 5f;
    public float guidanceSpeedMultiplier = 1.25f;

    public void Collect(PlayerController player)
    {
        switch (type)
        {
            case PickupType.Protection:
                player.AddProtection(value);
                break;
            case PickupType.Guidance:
                player.ApplyGuidance(guidanceSpeedMultiplier, guidanceDuration);
                break;
            case PickupType.Oar:
                player.RestoreBalance(value);
                break;
        }
        // Sounds/particles can be triggered here via AudioManager
    }
}
