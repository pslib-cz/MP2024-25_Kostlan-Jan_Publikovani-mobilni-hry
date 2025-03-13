using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemies
{
	/// <summary>
	/// Nepřítel, který používá navMeshAgenta a pronásleduje cíl.
	/// </summary>
	public class EnemyNavMesh: EnemyBase
	{
		[SerializeField] private Transform target;
		private NavMeshAgent agent;

		private void Awake()
		{
			isAttack = true;
		}

		private void Start()
		{
			agent = GetComponent<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updateUpAxis = false;
		}

		private void Update()
		{
			agent.SetDestination(target.position);
		}
	}
}
