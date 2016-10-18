using UnityEngine;
using System.Collections;
//To scale the background according to the dimensions of the camera
public class BGScaler : MonoBehaviour {

	void Start () {

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 tempScale = transform.localScale;

        float width = sr.bounds.size.x;
        float worldHeight = Camera.main.orthographicSize * 2.0f;
        float worldWidth = worldHeight / Screen.height * Screen.width;

        tempScale.x = worldWidth / width;
        transform.localScale = tempScale;

	}
	
}//BG Scaler
