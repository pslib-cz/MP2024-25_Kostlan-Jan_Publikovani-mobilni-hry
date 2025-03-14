﻿using Assets.Scripts.Interfaces;
using UnityEngine;

/// <summary>
/// Nepřítel, který pronásleduje hráče a nemá takedamage.
/// </summary>
public class Kraken : EnemyBase
{
	private PlayerController2D controller;
	public float speed = 2f;

	private void Awake()
	{
		controller = FindFirstObjectByType<PlayerController2D>();

		if (controller == null)
		{
			Debug.LogError("PlayerController2D nebyl nalezen ve scéně.");
		}

		isAttack = true;
	}

	private void Start()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
	}

	void Update()
	{
		Vector3 currentPosition = transform.position;
		Vector3 playerPosition = controller.transform.position;
		Vector3 targetPosition = new Vector3(playerPosition.x, currentPosition.y, currentPosition.z);
		transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
	}
}
