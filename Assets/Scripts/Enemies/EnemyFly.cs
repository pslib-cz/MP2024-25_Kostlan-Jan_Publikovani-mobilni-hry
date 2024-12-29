﻿using Assets.Scripts.Interfaces;
using UnityEngine;

/// <summary>
/// Létající nepřítel, který reaguje na hráčovo otočení na něj.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class EnemyFly : EnemyBase
{
    public Sprite alertedSprite; 
    private Sprite defaultSprite;
    public float alertedSpeed = 2f;
    public float defaultSpeed = 1f;
    private SpriteRenderer spriteRenderer; 
    public bool isAlerted;
    private bool facingRight = true;

    public Transform player;
    
    public float lifetime = 100.0f;

    public float verticalSpeed = 0.5f;
    public float moveScale = 3f;
    
    public GameObject panel;

    public AudioClip alertSound;
    public AudioClip music;

	private float baseY;
    
    private void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;

        baseY = transform.position.y;

        audioSource = GetComponent<AudioSource>();

        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();

        if (alertSound != null)
        {
            audioSource.PlayOneShot(alertSound);
        }
		panel.gameObject.SetActive(true);
        isAttack = true;
	}
    
    private void Update()
    {
        var playerSprite = player.GetComponent<PlayerController2D>().mFacingRight;
        var positionPlayer = player.position.x > transform.position.x;

        if (playerSprite != facingRight! && positionPlayer || playerSprite != facingRight && !positionPlayer)
        {
            AlertEnemy();
        }

        Vector2 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.Translate(direction * (Time.deltaTime * defaultSpeed));

        // Moving up, down with regualy interval
        float newY = baseY + Mathf.Sin(Time.time * verticalSpeed) * moveScale;
        transform.position = new Vector2(transform.position.x, newY);

        if ((positionPlayer && !facingRight) || (!positionPlayer && facingRight))
        {
            Flip();
        }
    }
    
    public void AlertEnemy()
    {
        isAlerted = true;
        spriteRenderer.sprite = alertedSprite;
        defaultSpeed = alertedSpeed;
        
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	public override void TakeDamage(int damage)
	{
        // Nemá vůbec možnost mu snížit zdraví, ale pro snažší implementaci to tak mám takhle.
	}
}
