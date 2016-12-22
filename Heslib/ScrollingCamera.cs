using UnityEngine;
using System.Collections;

public class ScrollingCamera : MonoBehaviour {
	public float edgeProportions = .1f;//how close to the edge the mouse should be to start scrolling.

	public float baseSpeed = .5f;
	public float speedMultiplier = 5f;

	public Vector3 min;
	public Vector3 max;
	public float scrollSpeed = 1;

	Transform t;


	float xmin;
	float ymin;
	float xmax;
	float ymax;

	// Use this for initialization
	void Start () {
		t = transform;

		xmin = edgeProportions * Screen.width;

		ymin = edgeProportions * Screen.height;
		xmax = Screen.width- (edgeProportions * Screen.width);
		ymax = Screen.height - (edgeProportions * Screen.height);
	}
	
	// Update is called once per frame
	void Update () {




		if(Input.mousePosition.x < xmin && t.position.x > min.x)
		{
			t.Translate((Mathf.Abs(Input.mousePosition.x - xmin) * speedMultiplier * baseSpeed + baseSpeed) * -Time.deltaTime,0,0);
		}
		if (Input.mousePosition.x > xmax && t.position.x < max.x)
		{
			t.Translate((Mathf.Abs(Input.mousePosition.x - xmax) * speedMultiplier * baseSpeed + baseSpeed) * Time.deltaTime, 0, 0);
		}
		if (Input.mousePosition.y < ymin && t.position.y > min.y)
		{
			t.Translate(0, (Mathf.Abs(Input.mousePosition.y - ymin) * speedMultiplier * baseSpeed + baseSpeed) * -Time.deltaTime, 0);
		}
		if (Input.mousePosition.y > ymax && t.position.y < max.y)
		{
			t.Translate(0, (Mathf.Abs(Input.mousePosition.y - ymax) * speedMultiplier * baseSpeed + baseSpeed) * Time.deltaTime, 0);
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;

		if (!((scroll + t.position.z > max.z) ||  ( t.position.z + scroll < min.z)))
		{
			Camera.main.orthographicSize += scroll;
		}
		
	}
    
}
