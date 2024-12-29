using UnityEngine;

/// <summary>
/// Script to play a wake-up animation and reset the position before destruction.
/// </summary>
public class PlayWakeUpAnimation : MonoBehaviour
{
	public Animator animator;
	public AnimationClip specialAnimation;
	public AnimationClip defaultAnimation;

	public float destructionTime = 5.0f;

	/// <summary>
	/// Some validation and initialization.
	/// </summary>
	void Start()
	{
		if (animator == null)
		{
			Debug.LogError("Animator is not set.");
			return;
		}

		if (specialAnimation == null)
		{
			Debug.LogError("Special animation is not set.");
			return;
		}

		if (defaultAnimation == null)
		{
			Debug.LogError("Default animation is not set.");
			return;
		}

		// Play the special animation at the start
		animator.Play(specialAnimation.name);

		// Destroy this script after the set time
		Invoke("ResetPositionAndDestroy", destructionTime);
	}

	/// <summary>
	/// Play default animation.
	/// </summary>
	void Update()
	{
		// If the special animation is playing and has finished, play the default animation
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.IsName(specialAnimation.name) && stateInfo.normalizedTime >= 1)
		{
			animator.Play(defaultAnimation.name);
		}
	}

	/// <summary>
	/// Reset the position and destroy this script.
	/// </summary>
	void ResetPositionAndDestroy()
	{
		// Destroy this script component
		Destroy(this);
	}
}
