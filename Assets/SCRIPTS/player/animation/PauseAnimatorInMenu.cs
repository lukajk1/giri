using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimatorInMenu : MonoBehaviour
{
    public Animator animator;
    public GameState gameState;

    private bool isPaused;

    void Start()
    {
        isPaused = false;
        animator.enabled = false;
    }

    void Update()
    {
        bool shouldBePaused = gameState.MenusOpen > 0;

        if (shouldBePaused != isPaused)
        {
            isPaused = shouldBePaused;
            animator.speed = isPaused ? 0 : 1;
        }
    }
}
