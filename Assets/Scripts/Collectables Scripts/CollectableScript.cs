using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {

    //to activate the game object associated with the script
	void OnEnable()
    {
        Invoke("DestroyCollectable", 6f);
    }
    //to deactivate the game object associated with the script
    void OnDisable()
    {
        //No code here
    }

    void DestroyCollectable()
    {
        gameObject.SetActive(false);
    }
}
