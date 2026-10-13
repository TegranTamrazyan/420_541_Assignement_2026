using UnityEngine;

public class JumpBoostPickup : MonoBehaviour
{
    [SerializeField] private GameObject coinCollider;
    [SerializeField] private MeshRenderer coinVisuals;
    [SerializeField] private MeshRenderer coinTextFront;
    [SerializeField] private MeshRenderer coinTextBack;
    [SerializeField] private float boostDuration = 30f;
    [SerializeField] private float respawnTokenDuration = 30f;
    [SerializeField] private float tempBoostDuration;
    [SerializeField] private float tempRespawnTokenDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isBoostActive = false;
    private bool isRespawnTokenActive = false;
    private Collider player; 

    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player, ignore everything else
        if (other.CompareTag("Player"))
        {
            player = other; 
            other.GetComponent<PlayerMovement>().ActivateJumpBoost();
            isBoostActive = true;
            isRespawnTokenActive = true;

            print("Jump Boost Activated!");

            coinCollider.GetComponent<Collider>().enabled = false;
            coinVisuals.GetComponent<MeshRenderer>().enabled = false;
            coinTextFront.GetComponent<MeshRenderer>().enabled = false;
            coinTextBack.GetComponent<MeshRenderer>().enabled = false;

            SoundManager.PlaySound(SoundType.Collect, 0.5f);
        }
    }
    private void Update()
    {
        if (isBoostActive)
        {
            tempBoostDuration -= Time.deltaTime;
            if (tempBoostDuration <= 0f)
            {
                player.GetComponent<PlayerMovement>().DeactivateJumpBoost();
                isBoostActive = false;
                print("Jump Boost Deactivated!");
            }
        }
        if (isRespawnTokenActive)
        { 
            tempRespawnTokenDuration -= Time.deltaTime;
            if (tempRespawnTokenDuration <= 0f)
            {
                coinCollider.GetComponent<Collider>().enabled = true;
                coinVisuals.GetComponent<MeshRenderer>().enabled = true;
                coinTextFront.GetComponent<MeshRenderer>().enabled = true;
                coinTextBack.GetComponent<MeshRenderer>().enabled = true;
                isRespawnTokenActive = false;
                print("Jump Boost Coin Respawned!");
            }
        }
        if(!isBoostActive && !isRespawnTokenActive)
        {
            tempBoostDuration = boostDuration;
            tempRespawnTokenDuration = respawnTokenDuration;
        }

    }
}

    
