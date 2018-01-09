using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPhysicDetectComponent : CComponent {

	#region Fields

	[Header("Data")]
	[SerializeField]	protected Transform m_DetectTransform;
	[SerializeField]	protected float m_DetectRadius = 10f;
	public float detectRadius {
		get { return this.m_DetectRadius; }
		set { this.m_DetectRadius = value; }
	}

	[Header("Detect")]
	[SerializeField]	protected bool m_AutoDetect = true;
	public bool autoDetect {
		get { return this.m_AutoDetect; }
		set { this.m_AutoDetect = value; }
	}
	[SerializeField]	protected LayerMask m_DetectLayerMask = -1;
	[SerializeField]	protected int m_ColliderCount;
	public int colliderCount {
		get { return this.m_ColliderCount; }
		protected set { this.m_ColliderCount = value; }
	}
	protected int m_PreviousCount = 0;
	[SerializeField]	protected int m_MaximumDetect = 20;
	public int maximumDetect {
		get { return this.m_MaximumDetect; }
		set { 
			this.m_MaximumDetect = value; 
			this.m_SampleColliders = new Collider[value];
		}
	}
	[SerializeField]	protected Collider[] m_SampleColliders;
	public Collider[] sampleColliders {
		get { return this.m_SampleColliders; }
		protected set { 
			this.m_SampleColliders = value; 
			this.m_MaximumDetect = value.Length; 
		}
	}

	[Header("Event")]
	public UnityEvent OnFree;
	public UnityEvent OnDetected;

	#endregion

	#region Implementation Component

	protected override void Awake ()
	{
		base.Awake ();
		this.m_SampleColliders = new Collider[this.m_MaximumDetect];
		this.m_PreviousCount = 0;
	}

	protected override void LateUpdate ()
	{
		base.LateUpdate ();
		if (this.m_AutoDetect) {
			this.DetectObjects ();
		}
	}

	protected virtual void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		if (this.m_DetectTransform != null) {
			Gizmos.DrawWireSphere (this.m_DetectTransform.position, this.m_DetectRadius);
		} else {
			Gizmos.DrawWireSphere (this.transform.position, this.m_DetectRadius);
		}
	}

	#endregion

	#region Main methods

	public virtual void DetectObjects() {
		this.m_ColliderCount = Physics.OverlapSphereNonAlloc (
			this.m_DetectTransform.position, 
			this.m_DetectRadius, 
			this.m_SampleColliders,
			this.m_DetectLayerMask);  
		if (this.m_PreviousCount != this.m_ColliderCount 
			&& this.m_ColliderCount != 0) {
			if (this.OnDetected != null) {
				this.OnDetected.Invoke ();				
			}
			this.m_PreviousCount = this.m_ColliderCount;
		} else {
			if (this.OnFree != null) {
				this.OnFree.Invoke ();
			}
		}
	}

	public override void Reset ()
	{
		base.Reset ();
		this.m_SampleColliders = new Collider[this.m_MaximumDetect];
		this.m_PreviousCount = -1;
	}

	#endregion

}
