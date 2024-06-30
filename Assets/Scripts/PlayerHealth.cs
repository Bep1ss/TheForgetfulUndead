using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public Image healthBar;
    public Image BloodScreenEffect; // Changed from GameObject to Image to manipulate the alpha
    public AudioClip hitSound;
    public AudioClip hurtSound;
    public float hitSoundVolume = 1.0f; // Volume for hit sound, adjustable in the Inspector
    public float hurtSoundVolume = 1.0f; // Volume for hurt sound, adjustable in the Inspector

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health to max at start
        UpdateHealthBar(); // Update the health bar at the start
        SetBloodScreenEffectAlpha(0); // Ensure the BloodScreenEffect is fully transparent at the start
    }

    public void GainHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Ensure health does not exceed max
        }
        UpdateHealthBar(); // Update the health bar after gaining health
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0; // Ensure health does not drop below 0
        }
        UpdateHealthBar(); // Update the health bar after taking damage
        AudioSource.PlayClipAtPoint(hitSound, transform.position, hitSoundVolume);
        AudioSource.PlayClipAtPoint(hurtSound, transform.position, hurtSoundVolume);
        StartCoroutine(FadeBloodScreenEffect()); // Start the coroutine to show and fade the BloodScreenEffect
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth; // Update the health bar fill amount
    }

    private void SetBloodScreenEffectAlpha(float alpha)
    {
        Color color = BloodScreenEffect.color;
        color.a = alpha;
        BloodScreenEffect.color = color;
    }

    private IEnumerator FadeBloodScreenEffect()
    {
        SetBloodScreenEffectAlpha(1); // Set the BloodScreenEffect alpha to fully visible

        float duration = 2.0f; // Duration of the fade
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsed / duration); // Interpolate alpha from 1 to 0
            SetBloodScreenEffectAlpha(alpha);
            yield return null; // Wait for the next frame
        }

        SetBloodScreenEffectAlpha(0); // Ensure the BloodScreenEffect is fully transparent at the end
    }
}
