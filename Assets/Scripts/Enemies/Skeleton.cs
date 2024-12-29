using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Skeleton : EnemyBase
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 2f;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip walkClip;
        [SerializeField] private AudioClip showClip;

        private bool dead;
        private bool isWalkingRight;
        private Transform target;
        private Animator animator;
        private Rigidbody2D rb;
        private BoxCollider2D mainBoxCollider;
        private BoxCollider2D secondaryBoxCollider;

        [SerializeField] private bool mFacingRight = false;
        private bool canMove = false;

        private void Awake()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody2D>();

            BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
            if (colliders.Length > 1)
            {
                mainBoxCollider = colliders[0];
                secondaryBoxCollider = colliders[1];
            }
            else
            {
                Debug.LogError("Skeleton has less than two BoxCollider2D components.");
            }
        }

        private void Update()
        {
            if (canMove)
            {
                if (target.position.x > transform.position.x && mFacingRight)
                {
                    Flip();
                }
                else if (target.position.x < transform.position.x && !mFacingRight)
                {
                    Flip();
                }
                transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
        }

        private void Flip()
        {
            mFacingRight = !mFacingRight;

            var transform1 = transform;
            Vector3 theScale = transform1.localScale;
            theScale.x *= -1;
            transform1.localScale = theScale;
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && dead == false && canMove == true)
            {
                isAttack = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.GetContact(0).normal.Normalize();
        }

        public override void TakeDamage(int damage)
        {
            animator.SetBool("Dead", true);
            PlayDeathSound();
            dead = true;
            rb.gravityScale = 0;
            secondaryBoxCollider.enabled = false;
            mainBoxCollider.enabled = false;
            canMove = false;
        }

        public void Start()
        {
            audioSource.clip = showClip;
            audioSource.Play();
        }

        // Je to animace event na konci animace.
        public void EnableWalk()
        {
            audioSource.clip = walkClip;
            audioSource.Play();
            secondaryBoxCollider.enabled = true;
            mainBoxCollider.enabled = true;
            canMove = true;
        }

        public void EnableWakeUp()
        {
            animator.SetBool("WakeUp", true);
        }
    }
}