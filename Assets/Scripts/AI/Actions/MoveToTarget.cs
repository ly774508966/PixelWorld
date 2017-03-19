using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace AISystem
{
	public class MoveToTarget : Action 
	{
		public SharedVector3 direction;

		private Character _owner;
       		private Animator animator;

		public override void OnAwake ()
		{
			_owner = gameObject.GetComponent<Character>();
		}
		public override void OnStart()
	        {
	                animator = GetComponent<Animator>();
	        }
		public override TaskStatus OnUpdate ()
		{
	
			transform.forward = direction.Value;
			transform.position += _owner.Speed * direction.Value * Time.deltaTime;
            		//targetTransform.Translate(translation.Value, relativeTo);

			animator.SetBool("bAttack", false);
			animator.SetBool("bMoving", true);

			return TaskStatus.Success;
		}

	}
}
