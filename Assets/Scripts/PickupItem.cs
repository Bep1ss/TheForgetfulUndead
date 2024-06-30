using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class PickupItem : MonoBehaviour
{
    public AudioClip pickupSound; // The sound to play when the pickup is collected
    private GameObject unlock;

    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("healthPickup"))
            {
                // Try to get the PlayerHealth component from the player
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    // Increase player's health
                    playerHealth.GainHealth(20);

                    // Destroy the pickup item
                    Destroy(gameObject);
                }
            }
            else if (gameObject.CompareTag("ammoPickup"))
            {
                Weapon weapon = other.GetComponentInChildren<Weapon>();
                weapon.reloadAmmunition += 60;
                
                // Destroy the pickup item
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("rifle"))
            {
                // Enable use of the 2nd gun.
                Character character;
                character = other.gameObject.GetComponent<Character>();
                character.unlockGun = true;

                // Destroy the pickup item
                Destroy(gameObject);
            }

            // Play the pickup sound
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            
        }
    }
}
