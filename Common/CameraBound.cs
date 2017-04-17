using UnityEngine;
using System.Collections;

public class CameraBound : MonoBehaviour {

    public Transform target;
    public GameObject spriteMap;
    private Vector3 scaleOffSet;
    private float cameraMove = 1.5f;

    private float rightBound;
    private float leftBound;
    private float topBound;
    private float bottomBound;

	void Start () {
        BoundMap();
	}
	
	void Update () {
        //Mathf clamp is limit to pos1 from pos2...
        var pos = new Vector3(target.position.x, target.position.y, transform.position.z);
        pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
        pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);

        //set Camera position follow bound and game object target
        float x = Mathf.Lerp(transform.position.x, pos.x, Time.deltaTime * cameraMove);
        float y = Mathf.Lerp(transform.position.y, pos.y, Time.deltaTime * cameraMove);
        gameObject.transform.position = new Vector3(x, y, transform.position.z);
	}

    void LateUpdate()
    {
        BoundMap();
    }

    public void BoundMap()
    {
        //find game object
        SpriteRenderer map = spriteMap.GetComponent<SpriteRenderer>();
        //get local scale object
        scaleOffSet = map.transform.localScale;

        //get width and height of Camera main !!! Impostant
        float vertExtend = Camera.main.orthographicSize;
        float horzExtend = vertExtend * Screen.width / Screen.height;

        //get left, right, top, down bound ...
        //with x,y,z = 0 is center Sprite, Accounding we have Camera width subtract map width 
        // this bound left.
        leftBound = (float)(horzExtend - (map.sprite.bounds.size.x * scaleOffSet.x) / 2.0f) + map.transform.position.x;
        rightBound = (float)(((map.sprite.bounds.size.x * scaleOffSet.x) / 2.0f) - horzExtend) + map.transform.position.x;
        bottomBound = (float)(vertExtend - (map.sprite.bounds.size.y * scaleOffSet.y) / 2.0f) + map.transform.position.y;
        topBound = (float)((map.sprite.bounds.size.y * scaleOffSet.y) / 2.0f - vertExtend) + map.transform.position.y;
    }
}
