using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    // Speed
    public float speed = 1f;
    
    // movement animations
    public Animator animator;

    // movement sound
    public AudioSource audioSource;

    private Vector2 topLeft = new Vector2(-3f, 3f);
    private Vector2 topRight = new Vector2(3f, 3f);
    private Vector2 bottomRight = new Vector2(3f, -3f);
    private Vector2 bottomLeft = new Vector2(-3f, -3f);
    
    // Path for PacStudent to move
    private Vector2[] path;
    private int currentTarget = 0;

    void Start()
    {
        path = new Vector2[] { topLeft, topRight, bottomRight, bottomLeft };
        transform.position = path[0];
    }

    void Update()
    {
        MovePacStudent();
    }

    // move PacStudent along the path
    void MovePacStudent()
    {
        transform.position = Vector2.MoveTowards(transform.position, path[currentTarget], speed * Time.deltaTime);

        // Check if PacStudent has reached the target corner
        if ((Vector2)transform.position == path[currentTarget])
        {
            currentTarget = (currentTarget + 1) % path.Length;
        }

        // Play movement animation if it's not already playing
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PacStudentIdle"))
        {
            animator.Play("PacStudentIdle");
        }

        // Play movement audio if it's not playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}