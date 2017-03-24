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
				var camera = Camera.main;
				if (camera.orthographic) {
					var mousePos = camera.ScreenToWorldPoint (Input.mousePosition);
					m_ActiveLine.UpdateLine (mousePos);
				} else {
					var absZ = Mathf.Abs (camera.transform.position.z);
					var mousePos = camera.ScreenPointToRay (Input.mousePosition);
					m_ActiveLine.UpdateLine (mousePos.direction * absZ);
				}
			}

		}
	
	}
}
