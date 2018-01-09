using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CDataPointComponent : CComponent {

	#region Fields

	[Header("Data")]
	[SerializeField]	protected string m_DataName = "EmptyPoint";
	public string dataName {
		get { return this.m_DataName; }
		set { this.m_DataName = value; }
	}
	[SerializeField]	protected bool m_IsUpdate = true;
	public bool isUpdate {
		get { return this.m_IsUpdate; }
		set { this.m_IsUpdate = value; }
	}
	[SerializeField]	protected int m_ValuePoint = 100;
	public int curValuePoint {
		get { return this.m_ValuePoint; }
		set { this.m_ValuePoint = value < 0 
				? 0 
				: value > this.m_MaxValuePoint 
					? this.m_MaxValuePoint 
					: value; 
		}
	}
	[SerializeField]	protected int m_MaxValuePoint = 100;
	public int maxValuePoint {
		get { return this.m_MaxValuePoint; }
		set { this.m_MaxValuePoint = value; }
	}
	[Range(0.01f, 10f)]
	[SerializeField]	protected float m_ConsumeSpeed = 1f;
	[SerializeField]	protected int m_ConsumePerSecond = 1;
	public int consumePerSecond {
		get { return this.m_ConsumePerSecond; }
		set { this.m_ConsumePerSecond = value; }
	}
	protected float m_ValueCounter = 1f;
	protected float m_Valuenterval = 1f;

	[Header("Events")]
	public UnityEvent OnLowValue;
	public UnityEvent OnMaxValue;

	#endregion

	#region Implementation Component

	protected override void Update ()
	{
		base.Update ();
		if (this.m_IsUpdate) {
			this.UpdateValuePoint (Time.deltaTime * this.m_ConsumeSpeed);
		}
	}

	#endregion

	#region Main methods

	protected virtual void UpdateValuePoint(float dt) {
		if (this.m_ValueCounter > 0f) {
			this.m_ValueCounter -= dt;
		} else {
			this.curValuePoint = this.m_ValuePoint - this.m_ConsumePerSecond;
			this.m_ValueCounter = this.m_Valuenterval;
		}
		if (this.m_ValuePoint <= 0) {
			if (this.OnLowValue != null) {
				this.OnLowValue.Invoke ();
			}
		}
		if (this.m_ValuePoint >= this.m_MaxValuePoint) {
			if (this.OnMaxValue != null) {
				this.OnMaxValue.Invoke ();
			}
		}
	}

	public override void Reset ()
	{
		base.Reset ();
		this.curValuePoint = this.m_MaxValuePoint;
	}

	#endregion

}
