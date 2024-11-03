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
        StartCoroutine(PowerPillEffect());  // Start the Power Pill effect
    }

    private IEnumerator PowerPillEffect()
    {
        // Set all ghosts to Scared state and start scared music
        foreach (Animator ghostAnimator in ghostAnimators)
        {
            if (ghostAnimator != null)
            {
                ghostAnimator.SetTrigger("Scared");
            }
        }

        if (scaredMusic != null)
        {
            scaredMusic.Play();
        }

        // Show timer UI and set the timer
        ghostTimerUI.gameObject.SetActive(true);
        timer = powerPillDuration;

        while (timer > 0)
        {
            ghostTimerUI.text = "Time Left: " + Mathf.Ceil(timer).ToString();

            // Decrease the timer
            timer -= Time.deltaTime;

            // Switch to recovering state with 3 seconds left
            if (timer <= 3f && timer > 0)
            {
                foreach (Animator ghostAnimator in ghostAnimators)
                {
                    if (ghostAnimator != null)
                    {
                        ghostAnimator.SetTrigger("Recovering");
                    }
                }
            }

            yield return null;
        }

        // After the timer runs out, reset the ghosts to their normal state
        foreach (Animator ghostAnimator in ghostAnimators)
        {
            if (ghostAnimator != null)
            {
                ghostAnimator.SetTrigger("Walking");
            }
        }

        // Hide timer UI and stop scared music
        ghostTimerUI.gameObject.SetActive(false);

        if (scaredMusic != null)
        {
            scaredMusic.Stop();
        }
    }
}