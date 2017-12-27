using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapManager : MonoBehaviour {

	#region Fields

	[Header("Places")]
	[SerializeField]	protected float m_PlaceDistance = 50f;
	[SerializeField]	protected string m_PlaceNamePattern = "x{0}+y{1}";
	protected Vector2[] m_PlacePatterns = new Vector2[] {
		new Vector2 (-1f, 1f), new Vector2 (0f, 1f), new Vector2 (1f, 1f), 
		new Vector2 (-1f, 0f), new Vector2 (0f, 0f), new Vector2 (1f, 0f), 
		new Vector2 (-1f, -1f), new Vector2 (0f, -1f), new Vector2 (1f, -1f), 
	};
	[SerializeField]	protected List<Transform> m_UsedPlaces;
	[SerializeField]	protected List<Transform> m_ReusePlaces;
	protected Dictionary<string, Transform> m_MapInstance;

	[Header("Target")]
	[SerializeField]	protected Transform m_Target;
	[SerializeField]	protected Vector2 m_CurrentPosition;
	protected Vector2 m_PreviousPosition = new Vector2(-999f, -999f);
	protected bool m_NeedUpdate = false;

	#endregion

	#region Implementation MonoBehaviour

	protected void Awake() {
		this.m_ReusePlaces = new List<Transform> ();
		this.m_MapInstance = new Dictionary<string, Transform> ();
	}

	protected void Start() {
		this.InitMap();
	}

	protected void LateUpdate() {
		this.CheckPlacePatterns ();
		this.CheckCurrentPosition ();
		if (this.m_NeedUpdate) {
			this.UpdateReusePlaces ();
			this.UpdatePlaces ();
		} 
	}

	#endregion

	#region Main methods

	protected void InitMap() {
		var childCount = this.transform.childCount;
		for (int i = 0; i < childCount; i++) {
			var child = this.transform.GetChild (i);
			var updatePos = this.m_PlacePatterns [i % 9];
			child.gameObject.SetActive (false);
			this.UpdatePlanetPosition (child, updatePos); 
			this.m_ReusePlaces.Add (child);
		}
		this.m_NeedUpdate = true;
	}

	protected void CheckCurrentPosition() {
		if (this.m_Target == null)
			return;
		var position = this.m_Target.position;
		this.m_CurrentPosition.x = Mathf.RoundToInt (position.x / this.m_PlaceDistance);
		this.m_CurrentPosition.y = Mathf.RoundToInt (position.y / this.m_PlaceDistance);
		if (this.m_CurrentPosition.x != this.m_PreviousPosition.x
		    || this.m_CurrentPosition.y != this.m_PreviousPosition.y) {
			this.m_NeedUpdate = true;
			this.m_PreviousPosition.x = this.m_CurrentPosition.x;
			this.m_PreviousPosition.y = this.m_CurrentPosition.y;
		}
	}

	protected void CheckPlacePatterns() {
		var currentPos 	= this.m_CurrentPosition;
		// topLeft
		this.m_PlacePatterns[0].x = currentPos.x - 1f;
		this.m_PlacePatterns[0].y = currentPos.y + 1f;
		// topUp
		this.m_PlacePatterns[1].x = currentPos.x;
		this.m_PlacePatterns[1].y = currentPos.y + 1f; 
		// topRight
		this.m_PlacePatterns[2].x = currentPos.x + 1f;
		this.m_PlacePatterns[2].y = currentPos.y + 1f;
		// left
		this.m_PlacePatterns[3].x = currentPos.x - 1f;
		this.m_PlacePatterns[3].y = currentPos.y;
		// center
		this.m_PlacePatterns[4].x = currentPos.x;
		this.m_PlacePatterns[4].y = currentPos.y; 
		// right
		this.m_PlacePatterns[5].x = currentPos.x + 1f; 
		this.m_PlacePatterns[5].y = currentPos.y;
		// bottomLeft
		this.m_PlacePatterns[6].x = currentPos.x - 1f;
		this.m_PlacePatterns[6].y = currentPos.y - 1f; 
		// bottomDown
		this.m_PlacePatterns[7].x = currentPos.x;
		this.m_PlacePatterns[7].y = currentPos.y - 1f; 
		// bottomRight
		this.m_PlacePatterns[8].x = currentPos.x + 1f;
		this.m_PlacePatterns[8].y = currentPos.y - 1f;
	}

	protected void UpdateReusePlaces() {
		for (int x = 0; x < this.m_UsedPlaces.Count; x++) {
			var checkName = this.m_UsedPlaces [x].name;
			var isGoodPlace = false;
			for (int i = 0; i < this.m_PlacePatterns.Length; i++) {
				var planetPos = this.m_PlacePatterns[i];
				var planetName = string.Format (this.m_PlaceNamePattern, planetPos.x, planetPos.y);
				if (planetName == checkName) {
					isGoodPlace = true;
					break;
				}
			}
			if (isGoodPlace == false) {
				var usedObject = this.m_UsedPlaces [x];
				if (this.m_ReusePlaces.Contains (usedObject) == false) {
					this.AddReuseObject (usedObject);
				}
			}
		}
	}

	protected void UpdatePlaces() {
		for (int i = 0; i < this.m_PlacePatterns.Length; i++) {
			var planetPos = this.m_PlacePatterns[i];
			var placeName = string.Format (this.m_PlaceNamePattern, planetPos.x, planetPos.y);
			var isGoodPlace = false;
			for (int x = 0; x < this.m_UsedPlaces.Count; x++) {
				var checkName = this.m_UsedPlaces [x].name;
				isGoodPlace |= placeName == checkName;
			}
			if (isGoodPlace == false) {
				var randomIndex = 0;
				var reuseObject = this.m_ReusePlaces [randomIndex];
				if (this.m_MapInstance.ContainsKey (placeName)) {
					reuseObject = this.m_MapInstance [placeName];
				} else {
					randomIndex = Random.Range (0, this.m_ReusePlaces.Count);
					reuseObject = this.m_ReusePlaces [randomIndex];
				} 
				this.AddUsedObject (reuseObject);
				this.UpdatePlanetPosition (reuseObject, planetPos);
			}
		}
		this.m_NeedUpdate = true;
	}

	protected void AddReuseObject(Transform value) {
		this.m_ReusePlaces.Add (value);
		this.m_UsedPlaces.Remove (value);
		this.m_UsedPlaces.TrimExcess ();
		value.gameObject.SetActive (false);
	}

	protected void AddUsedObject(Transform value) {
		this.m_ReusePlaces.Remove (value);
		this.m_UsedPlaces.Add (value);
		this.m_ReusePlaces.TrimExcess ();
		value.gameObject.SetActive (true);
	}

	protected void UpdatePlanetPosition (Transform planet, Vector2 pos) {
		planet.name = string.Format (this.m_PlaceNamePattern, pos.x, pos.y);
		planet.position = new Vector3 (pos.x * this.m_PlaceDistance, pos.y * this.m_PlaceDistance, 0f);
	}

	#endregion

}
