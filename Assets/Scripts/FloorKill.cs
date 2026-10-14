using UnityEngine;

public class FloorKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player, ignore everything else
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LevelReload();
        }
    }
}
