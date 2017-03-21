using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace AISystem
{
	public class MoveToTarget : Action 
	{
		public SharedVector3 target;
		public SharedVector3 direction;

		private Character _owner;
       		private Animator animator;
		private CharacterController characterController;
       		private NavMeshAgent agent;

		public override void OnAwake ()
		{
			_owner = gameObject.GetComponent<Character>();
		}
		public override void OnStart()
	        {
			animator = GetComponent<Animator>();
			characterController = GetComponent<CharacterController>();
			agent = GetComponent<NavMeshAgent>();
	        }
		public override TaskStatus OnUpdate ()
		{
	
			transform.forward = direction.Value;
			Vector3 move =  _owner.Speed * direction.Value * Time.deltaTime;
			//transform.position += _owner.Speed * direction.Value * Time.deltaTime;
			move.y = -10;
			//characterController.Move(move);
			agent.SetDestination(target.Value);

			animator.SetBool("bAttack", false);
			animator.SetBool("bMoving", true);

			return TaskStatus.Success;
		}

	}
}
