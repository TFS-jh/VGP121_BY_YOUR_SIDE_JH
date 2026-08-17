using UnityEngine;

public class PickupObj : MonoBehaviour
{
    public enum PickupType
    {
        Triple,
        PSpeed,
        Coin
    }

    [SerializeField] private PickupType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (type)
            {
                case PickupType.Triple:
                    player.maxJumpCount = 3;
                    player.baseSpeed = 8;
                    Debug.Log("Picked up Triple Jump!");
                    break;
                case PickupType.PSpeed:
                    player.maxJumpCount = 1;
                    player.baseSpeed = 12;
                    Debug.Log("Picked up P Speed!");
                    break;
                case PickupType.Coin:
                    player.IncrementCoinCounter(1);
                    Debug.Log("Picked up 1 Coin!");
                    break;
            }
            // Destroy the pickup after it has been collected
            Destroy(gameObject);
        }
    }
}
