using UnityEngine;

/// <summary>
/// Zapíná animaci vstávání a skript se sám zníčí.
/// </summary>
public class PlayWakeUpAnimation : MonoBehaviour
{
	public Animator animator;
	public AnimationClip specialAnimation;
	public AnimationClip defaultAnimation;
	public float destructionTime = 5.0f;

	void Start()
	{
		if (animator == null)
		{
			Debug.LogError("Animator není nastavený.");
			return;
		}

		if (specialAnimation == null)
		{
			Debug.LogError("Speciální animace není nastavená.");
			return;
		}

		if (defaultAnimation == null)
		{
			Debug.LogError("Defaultní animace není nastavená.");
			return;
		}

		animator.Play(specialAnimation.name);

		Invoke("ResetPositionAndDestroy", destructionTime);
	}

	void Update()
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.IsName(specialAnimation.name) && stateInfo.normalizedTime >= 1)
		{
			animator.Play(defaultAnimation.name);
		}
	}

	void ResetPositionAndDestroy()
	{
		Destroy(this);
	}
}