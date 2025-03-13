using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
	/// <summary>
	/// Abstraktní třída pro obecného nepřítele.
	/// </summary>
	public abstract class EnemyBase : MonoBehaviour
	{
		[SerializeField] protected int health;
		internal protected AudioSource audioSource;
		[SerializeField] protected AudioClip deathClip;

		public virtual void TakeDamage(int damage)
		{

		}

		public bool isAttack
		{
			get;
			set;
		}

		protected void PlaySound(AudioClip clip)
		{
			if (audioSource != null && clip != null)
			{
				audioSource.clip = clip;
				audioSource.Play();
			}
		}

		protected void PlayDeathSound()
		{
			if (audioSource != null && deathClip != null)
			{
				audioSource.PlayOneShot(deathClip);
				StartCoroutine(DisableAudioSourceAfterClip(deathClip.length));
			}
		}

		private IEnumerator DisableAudioSourceAfterClip(float clipLength)
		{
			yield return new WaitForSeconds(clipLength);
			audioSource.enabled = false;
		}
	}
}
