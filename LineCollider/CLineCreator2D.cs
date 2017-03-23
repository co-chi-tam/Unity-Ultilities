using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LineCollider2D {
	public class CLineCreator2D : MonoBehaviour {

		[SerializeField]	private CLineCollider2D m_LinePrefab;
		[SerializeField]	private List<CLineCollider2D> m_Lines;

		private CLineCollider2D m_ActiveLine;

		private void Start() {
			m_Lines = new List<CLineCollider2D> ();
		}

		private void Update() {
			if (Input.GetMouseButtonDown (0)) {
				var lineGo = Instantiate (m_LinePrefab);
				m_ActiveLine = lineGo;
			}
			if (Input.GetMouseButtonUp (0)) {
				m_ActiveLine = null;
			}
			if (m_ActiveLine != null) {
				var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				m_ActiveLine.UpdateLine (mousePos);
			}

		}
	
	}
}
