using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace AI {
	public class CanAttack : Conditional 
	{
		public SharedFloat distance;

		private Character _owner;
	
		public override void OnAwake ()
		{
			_owner = gameObject.GetComponent<Character>();
		}


		public override TaskStatus OnUpdate ()
		{			
			if (distance.Value < _owner.DisAttack) {
				// 判断聚集
				return TaskStatus.Success;
           		} else {
				return TaskStatus.Failure;
           		}
		}
	}
}
