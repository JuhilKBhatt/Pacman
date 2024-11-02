using System.Collections;
using UnityEngine;
using TMPro;

public class PowerPillController : MonoBehaviour
{
    public Animator[] ghostAnimators;       // Array of Ghost animators
    public AudioSource scaredMusic;         // Scared mode background music
    public TextMeshProUGUI ghostTimerUI;    // UI Text for displaying timer
    public float powerPillDuration = 10f;   // Total duration of power pill effect

    private float timer;                    // Internal timer for countdown

    private void Start()
    {
        // Ensure the timer UI is hidden initially
        ghostTimerUI.gameObject.SetActive(false);
    }

    // Called by PowerPill when PacStudent collides with it
    public void ActivatePowerPill()
    {
        // Start the Power Pill effect
        StartCoroutine(PowerPillEffect());
    }

    private IEnumerator PowerPillEffect()
    {
        // Activate Scared state and play scared music
        foreach (Animator ghostAnimator in ghostAnimators)
        {
            ghostAnimator.SetTrigger("Scared");
        }
        scaredMusic.Play();

        // Show timer UI and set the timer
        ghostTimerUI.gameObject.SetActive(true);
        timer = powerPillDuration;

        while (timer > 0)
        {
            // Update the timer UI
            ghostTimerUI.text = "Time Left: " + Mathf.Ceil(timer).ToString();
            
            // Decrease the timer
            timer -= Time.deltaTime;

            // Switch to recovering state with 3 seconds left
            if (timer <= 3f && timer > 0)
            {
                foreach (Animator ghostAnimator in ghostAnimators)
                {
                    ghostAnimator.SetTrigger("Recovering");
                }
            }

            yield return null;
        }

        // After the timer runs out, reset the ghosts to their normal state
        foreach (Animator ghostAnimator in ghostAnimators)
        {
            ghostAnimator.SetTrigger("Walking");
        }

        // Hide timer UI and stop scared music
        ghostTimerUI.gameObject.SetActive(false);
        scaredMusic.Stop();
    }
}