using UnityEngine;
using System;
using System.Collections;

public class WaitHelper {

	/// <summary>
	/// WaitHelper wait for fixed update.
	/// </summary>
	private static WaitForFixedUpdate m_WaitForFixedUpdate = new WaitForFixedUpdate ();
	public static WaitForFixedUpdate WaitFixedUpdate {
		get { 
			return m_WaitForFixedUpdate;
		}
	}

	/// <summary>
	/// WaitHelper wait for end of frame.
	/// </summary>
	private static WaitForEndOfFrame m_WaitForEndOfFrame = new WaitForEndOfFrame ();
	public static WaitForEndOfFrame WaitEndOfFrame {
		get { 
			return m_WaitForEndOfFrame;
		}
	}

	/// <summary>
	/// WaitHelper wait for 1 seconds.
	/// </summary>
	private static WaitForSeconds m_WaitForSortSeconds = new WaitForSeconds (1f);
	public static WaitForSeconds WaitForShortSeconds {
		get { 
			return m_WaitForSortSeconds;
		}
	}

	/// <summary>
	/// WaitHelper wait for 3 seconds.
	/// </summary>
	private static WaitForSeconds m_WaitForLongSeconds = new WaitForSeconds (3f);
	public static WaitForSeconds WaitForLongSeconds {
		get { 
			return m_WaitForLongSeconds;
		}
	}

}
