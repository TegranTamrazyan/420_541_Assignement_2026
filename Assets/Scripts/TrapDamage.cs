using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapDamage : MonoBehaviour
{
    private bool gracePeriodActive = false;
    private float gracePeriodDuration = 1.0f; // Duration of the grace period in seconds
    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player, ignore everything else
        if (other.CompareTag("Player") && !gracePeriodActive)
        {
            GameManager.Instance.RemoveHealth(1);
            gracePeriodActive = true;

            if(GameManager.Instance.Health <= 0)
            {
                GameManager.Instance.LevelReload();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        GracePeriod();
    }

    public void GracePeriod()
    {
        if (gracePeriodActive)
        {
            gracePeriodDuration -= Time.deltaTime;
            if (gracePeriodDuration <= 0)
            {
                gracePeriodActive = false;
                gracePeriodDuration = 1.0f;
            }
        }
    }
}
