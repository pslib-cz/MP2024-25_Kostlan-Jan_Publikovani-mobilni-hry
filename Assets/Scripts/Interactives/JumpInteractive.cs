﻿using UnityEngine;
namespace Assets.Scripts.Interactives
{
	public class JumpInteractive: MonoBehaviour
	{
		private PlayerController2D playerController;

		private void Awake()
		{
			playerController = GameObject.FindAnyObjectByType<PlayerController2D>();
		}

		public void JumpPlayer()
		{
			playerController.PerformJump();
			Destroy(this);
		}
	}
}
