using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to make pet follow player, sets animations based on position
public class Pet : MonoBehaviour
{
    private GameObject player;
    private float speed;
    private float distanceToPlayer;
    private Vector3 directionToPlayer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float followDistance;
    private PlaySound playSound;

    private void Awake() {
        player = GameObject.FindWithTag("Player");
        playSound = GameObject.Find("AudioPlayer").GetComponent<PlaySound>();
    }

    void Start() {
        speed = 2f;
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        directionToPlayer = player.transform.position - transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        followDistance = 2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer > followDistance) {
            animator.SetBool("idle", false);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            directionToPlayer = player.transform.position - transform.position;
            // Set animation relative to y travel direction
            if (directionToPlayer.y > 0) {
                animator.SetBool("backward", true);
                animator.SetBool("forward", false);
            } else if (directionToPlayer.y < 0) {
                animator.SetBool("backward", false);
                animator.SetBool("forward", true);
            }
            // Flip sprite
            if (directionToPlayer.x > 0) {
                spriteRenderer.flipX = true;
            } else if (directionToPlayer.x < 0) {
                spriteRenderer.flipX = false;
            }
        } else {
            animator.SetBool("idle", true);
        }
    }

    // Play sound and animation on click if player is close enough
    void OnMouseOver(){
        if (Input.GetMouseButtonDown(1)) {
            if (distanceToPlayer < 2f) {
                animator.SetTrigger("pet");
                playSound.Play(19);
            }
        }
    }
}
