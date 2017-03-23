using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LineCollider2D {
	[RequireComponent(typeof (LineRenderer))]
	[RequireComponent(typeof(EdgeCollider2D))]
	public class CLineCollider2D : MonoBehaviour {

		private LineRenderer m_LineRenderer;
		private EdgeCollider2D m_EdgeCollider;
		private List<Vector2> m_Points;

		private void Awake() {
			m_LineRenderer = this.GetComponent<LineRenderer> ();
			m_EdgeCollider = this.GetComponent<EdgeCollider2D> ();
			m_Points = new List<Vector2> ();
		}

		public void UpdateLine(Vector2 point) {
			if (m_Points.Count == 0) {
				SetPoint (point);
				return;
			}
			if (Vector2.Distance (m_Points [m_Points.Count - 1], point) > 0.1f) {
				SetPoint (point);
			}
		}

		private void SetPoint(Vector2 point) {
			m_Points.Add (point);
			m_LineRenderer.SetVertexCount (m_Points.Count);
			var edgePoint = new Vector2 [m_Points.Count];
			for (int i = m_Points.Count - 1; i >= 0; i--) {
				m_LineRenderer.SetPosition (i, m_Points[m_Points.Count - 1 - i]);
				edgePoint [i] = m_Points [i];
			}
			if (m_Points.Count > 1) {
				m_EdgeCollider.points = edgePoint;
			}
		}

	}
}
