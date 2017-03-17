using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace AI {
	public class IsInSight : Conditional 
	{
		public SharedString tag;
	        [RequiredField]
		public SharedFloat distance;
	        [RequiredField]
		public SharedVector3 direction;


		private Character _owner;

		public override void OnAwake ()
		{
			_owner = gameObject.GetComponent<Character>();
		}

		public override TaskStatus OnUpdate ()
		{

           		GameObject go = GameObject.FindWithTag(tag.Value);
           		if (go == null) {
           			return TaskStatus.Failure;
           		}

           		Vector3 offset = go.transform.position - transform.position;
           		distance.Value = offset.magnitude;

			if (distance.Value <= _owner.DisSight) {
				// calculate dir
	           		offset.y = 0;
				direction.Value = offset.normalized;

				return TaskStatus.Success;
           		} else {
				return TaskStatus.Failure;
           		}
		}
	}
}
