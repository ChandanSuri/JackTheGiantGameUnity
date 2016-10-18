using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour,IPointerUpHandler, IPointerDownHandler {

    private PlayerMoveJoystick playerMove;

    void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMoveJoystick>();
    }

	public void OnPointerDown(PointerEventData data)
    {
        if (gameObject.name == "Left")
        {
            playerMove.SetMoveLeft(true);
            //Debug.Log("Touched the left button");
        }
        else
        {
            playerMove.SetMoveLeft(false);
            //Debug.Log("Touched the Right Button");
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        playerMove.StopMoving();
        /*
        if (gameObject.name == "Left")
        {
            //Debug.Log("Released the left button");
        }
        else
        {
            //Debug.Log("Released the Right Button");
        }
        */
    }
}
