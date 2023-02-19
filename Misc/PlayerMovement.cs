using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Script for controling player movement, animations, and sprites
public class PlayerMovement : MonoBehaviour
{
    // Movement
    private float movespeed;
    private float runspeed;
    private float walkspeed;
    private Vector2 input;
    private string[] directions = {"forward", "forward_side", "side", "backward_side", "backward"};

    // Components
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Collisions
    public ContactFilter2D contactFilter;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private float collisionOffset;

    // Sprite head
    [SerializeField]
    private Sprite[] headDirections;
    private SpriteRenderer headRenderer;
    private Sprite[] headgearDirections;
    private SpriteRenderer headgearRenderer;

    public bool canMove;

    private void Awake() {
        headRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        headgearRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movespeed = walkspeed;
        runspeed = 6.0f;
        walkspeed = 4.0f;
        collisionOffset = 0.02f;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove) {
            return;
        }

        // Hold shift to run
        if (Input.GetKey("left shift") || Input.GetKey("right shift")) {
            animator.speed = 1.5f;
            movespeed = runspeed;
        } else {
            animator.speed = 1f;
            movespeed = walkspeed;
        }

        // On input, set animations and move player
        if (input != Vector2.zero) {
            animator.SetBool("idle", false);
            bool success = TryMove(input);

            if (!success) {
                success = TryMove(new Vector2(input.x, 0));
                if (!success) {
                    success = TryMove(new Vector2(0, input.y));
                }
            }

            ReadInputY();
        } else {
            animator.SetBool("idle", true);
        }

        // Flip player sprite depending on input
        FlipSprite();
    }

    /**
     Move player while using raycast to check for collisions.
     Returns true on no collision, false on collision.
    */
    private bool TryMove(Vector2 direction) {
        if (direction != Vector2.zero) {
            int count = rigidBody.Cast(
            direction, 
            contactFilter, // What the raycast can collide into and what it can ignore
            castCollisions, // List of collisions where we can store the results of the ray cast
            movespeed * Time.fixedDeltaTime + collisionOffset); // Length of the ray we are casting

            if (count == 0) {
                rigidBody.MovePosition(rigidBody.position + direction * movespeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        }
        return false;
    }

    /**
     Flips sprite to reflect the correct input direction.
    */
    private void FlipSprite() {
        if (input.x > 0) {
            spriteRenderer.flipX = true;
            headRenderer.flipX = true;
            headgearRenderer.flipX = true;
        } else if (input.x < 0) {
            spriteRenderer.flipX = false;
            headRenderer.flipX = false;
            headgearRenderer.flipX = false;
        }
    }

    /**
     Sets animator parameters to reflect Y input direction.
    */
    private void ReadInputY() {
        if (input.y == -1) {
            SetDirection("forward");
        } else if (input.y < -0.4f) {
            SetDirection("forward_side");
        } else if (input.y == 0) {
            SetDirection("side");
        } else if (input.y < 0.5f) {
            SetDirection("backward_side");
        } else {
            SetDirection("backward");
        }
    }

    /**
     Sets the given direction to true in the animator, all other directions are set to false.
    */
    private void SetDirection(string direction) {
        for (int i = 0; i < 5; i++) {
            if (directions[i] != direction) {
                animator.SetBool(directions[i], false);
            } else {
                animator.SetBool(directions[i], true);
                headRenderer.sprite = headDirections[i];
                if (headgearDirections != null) {
                    headgearRenderer.sprite = headgearDirections[i];
                }
            }
        }
    }

    /**
     Retrieves and updates the input value
    */
    void OnMove(InputValue movementValue) {
        input = AdjustDiagonalMovement(movementValue.Get<Vector2>());
        input.Normalize(); // Normalize vector to allow constant movespeed
    }

    // QoL hotfix: if moving diagonally, adjust vector to match isometric grid
    Vector2 AdjustDiagonalMovement(Vector2 value) {
        if (FloatNearlyEqual(value.y, 0.71f)) {
            return new Vector2(value.x, 0.35f);
        } else if (FloatNearlyEqual(value.y, -0.71f)) {
            return new Vector2(value.x, -0.35f);
        }
        return value;
    }

    // Returns true if two floats are nearly equal (to the hundredths decimal)
    bool FloatNearlyEqual(float f1, float f2) {
        decimal dec = new decimal(f1 - f2);
        double d = (double)dec;
        if (Math.Abs(d) < 0.01) {
            return true;
        }
        return false;
    }

    // Sets sprites for headgear object
    public void WearHeadgear(Sprite[] gear) {
        if (gear == null) {
            headgearDirections = null;
            headgearRenderer.sprite = null;
            return;
        }
        headgearDirections = gear;
        for (int i = 0; i < 5; i++) {
            if (animator.GetBool(directions[i])) {
                headgearRenderer.sprite = headgearDirections[i];
            }
        }
    }
}
