using UnityEngine;
using System.Collections;

namespace UtilRenderer { 
	[RequireComponent (typeof(Renderer))]
	public class UtilRenderer : MonoBehaviour {

		[SerializeField]	private GameObject m_Root;
		[SerializeField]	private Camera m_MainCamera;
		[SerializeField]	private string m_DefaultLayer;
		[SerializeField]	private string m_OnInvisibleLayer;

		private IUtilRenderer m_IRenderer;
		private int m_DefaultCameraLayer;

		private void Awake() {
			if (m_Root != null) {
				m_IRenderer = m_Root.GetComponent<IUtilRenderer> ();
			}
			if (m_MainCamera != null) {
				m_DefaultCameraLayer = m_MainCamera.cullingMask;
			}
		}

		private void Start() {
			
		}

		private void OnBecameInvisible() {
			if (m_Root != null) {
				m_IRenderer.OnVisible(false);
				m_Root.layer = LayerMask.NameToLayer (m_OnInvisibleLayer);
			}
		}

		private void OnBecameVisible() {
			if (m_Root != null) {
				m_IRenderer.OnVisible(true);
				m_Root.layer  = LayerMask.NameToLayer (m_DefaultLayer);
			}
		}
	
	}
}
