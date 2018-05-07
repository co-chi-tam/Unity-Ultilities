using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(LineRenderer))]
public class CUnitRopeCurve2D : MonoBehaviour {

	[Header("Configs")]
	[SerializeField]	protected Vector2 m_ScreenScale = new Vector2(2436f, 1125f);
	[SerializeField]	protected int m_LineLength 		= 100;
	[SerializeField]	protected float m_MinValue		= 0.8f;
	[SerializeField]	protected float m_MaxValue		= 1f;
	[SerializeField]	protected Vector2 m_MaxScale	= new Vector2 (15f, 10f);
	[SerializeField]	protected int m_Segments 		= 2;
	[Range(0, 1f)]
	[SerializeField]	protected float m_Offset		= 0f;
	[SerializeField]	protected AnimationCurve m_LineCurve;
	[SerializeField]	protected AnimationCurve m_LineCurve2;

	protected RectTransform m_RectTransform;
	protected LineRenderer m_LineRenderer;
	protected Vector3 m_UIPosition = new Vector3 (0f, 0f, 0f);
	protected Vector2 m_UIScale = new Vector2 (1f, 1f);


	protected virtual void Awake() {
		this.m_RectTransform 	= this.GetComponent <RectTransform> ();
		this.m_LineRenderer 	= this.GetComponent <LineRenderer> ();
		this.m_UIPosition 		= this.m_RectTransform.position;
		this.m_UIScale 			= this.m_RectTransform.sizeDelta;
	}

	protected virtual void Start() {
		this.InitLine ();
		this.DrawLine ();
	}

	protected virtual void LateUpdate() {
		this.DrawLine ();
	}

	public virtual void InitLine () {
		this.m_LineRenderer.useWorldSpace = false;
		var maxPoint = this.m_Segments * 2 + 1;
		var keyFrames = new Keyframe[maxPoint];
		for (int i = 0; i < maxPoint; i++) {
			var time = (float) i / (maxPoint - 1);
			var	value = i % 2 == 0 ? this.m_MaxValue : this.m_MinValue;
			var keyFrame = new Keyframe (time, value);
			keyFrames[i] = keyFrame;
		}
		this.m_LineCurve = new AnimationCurve (keyFrames);
		this.m_LineCurve2 = new AnimationCurve (new Keyframe (0f, 1f), new Keyframe (1f, 1f));
	}

	public virtual void DrawLine() {
		this.m_LineRenderer.positionCount = this.m_LineLength;
		var step 			= 1f / this.m_LineLength;
		this.m_UIPosition	= this.m_RectTransform.position;
		this.m_UIScale		= this.m_RectTransform.sizeDelta;
		for (int i = 0; i < this.m_LineLength; i++) {
			// X
			var xValue = i * step;
			var x = this.m_UIPosition.x
				+ xValue 
				* Mathf.Lerp (this.m_UIScale.x, this.m_MaxScale.x, this.m_Offset);
			// Y
			var yValue = Mathf.Lerp (this.m_LineCurve.Evaluate (xValue), this.m_LineCurve2.Evaluate (xValue), this.m_Offset);
			var y = this.m_UIPosition.y
				+ yValue 
				* Mathf.Lerp (this.m_UIScale.y, this.m_MaxScale.y, this.m_Offset);
			// DRAW
			var position = new Vector3 (x, y, this.m_UIPosition.z);
			this.m_LineRenderer.SetPosition (i, position);
		}
	}

}
