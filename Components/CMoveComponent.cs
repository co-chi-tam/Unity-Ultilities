using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CMoveComponent : CComponent {

	#region Fields

	[Header ("Value")]
	[SerializeField]	protected float m_MoveSpeed = 5f;
	public float moveSpeed {
		get { return this.m_MoveSpeed; }
		set { this.m_MoveSpeed = value; }
	}
	[SerializeField]	protected float m_RotationSpeed = 5f;
	public float rotationSpeed {
		get { return this.m_RotationSpeed; }
		set { this.m_RotationSpeed = value; }
	}
	protected float m_PreviousMoveSpeed;
	[SerializeField]	protected float m_MinDistance = 0.1f;
	public Vector3 currentPosition {
		get { 
			return this.transform.position; 
		}
		set { 
			this.transform.position = value;
		}
	}
	[SerializeField]	protected Vector3 m_TargetPosition;
	public Vector3 targetPosition {
		get { return this.m_TargetPosition; }
		set { this.m_TargetPosition = value; }
	}
	[SerializeField]	protected bool m_AutoMove = true;
	public bool autoMove {
		get { return this.m_AutoMove; }
		set { this.m_AutoMove = value; }
	}

	[Header("Events")]
	public UnityEvent OnNearestTarget;
	public UnityEvent OnMove;

	protected Transform m_Transform;

	#endregion

	#region Implementation Component

	protected override void Awake ()
	{
		base.Awake ();
		this.m_Transform = this.transform;
		this.m_PreviousMoveSpeed = this.m_MoveSpeed;
	}

	protected override void Update ()
	{
		base.Update ();
		if (this.m_AutoMove) {
			this.Move (Time.deltaTime);
		}
	}

	#endregion

	#region Main methods

	public virtual void Move(float dt) {
		var direction = this.m_TargetPosition - this.m_Transform.position;

		if (direction.sqrMagnitude > this.m_MinDistance * this.m_MinDistance * this.m_MoveSpeed) {
			var movePoint = this.m_Transform.position + direction.normalized * this.m_MoveSpeed * dt;
			this.m_Transform.position = movePoint;

			var angle = Mathf.Atan2 (direction.x, direction.z) * Mathf.Rad2Deg;
			this.m_Transform.rotation = Quaternion.Lerp (
				this.m_Transform.rotation, 
				Quaternion.AngleAxis (angle, Vector3.up),
				this.m_RotationSpeed * dt);

			if (this.OnMove != null) {
				this.OnMove.Invoke ();
			}
		} else {
			if (this.OnNearestTarget != null) {
				this.OnNearestTarget.Invoke ();
			}
		}
	}

	public virtual bool IsNearestTarget() {
		var direction = this.m_TargetPosition - this.m_Transform.position;
		return direction.sqrMagnitude < this.m_MinDistance;
	}

	public override void Reset ()
	{
		base.Reset ();
		this.m_TargetPosition = this.m_Transform.position;
		this.m_MoveSpeed = this.m_PreviousMoveSpeed;
	}

	#endregion

}
