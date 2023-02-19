using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to invoke an NPC animation on repeat
public class NPCAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private string triggerName;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("PlayAnimation", 2f, 5f);
    }

    // Plays an NPC animation
    private void PlayAnimation() {
        animator.SetTrigger(triggerName);
    }
}
