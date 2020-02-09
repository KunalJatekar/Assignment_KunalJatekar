using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerController PlayerController;

    int horizontal = 0;
    int vertical = 0;

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    public event System.Action OnCollision;

    void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = 0;
        vertical = 0;

        GetKeyboardInput();
        SetMovement();
    }

    void GetKeyboardInput()
    {
        horizontal = GetAxisRaw(Axis.Horizontal);
        vertical = GetAxisRaw(Axis.Vertical);

        if (horizontal != 0)
            vertical = 0;
    }

    void SetMovement()
    {
        if (vertical != 0)
        {
            PlayerController.SetUpInputDirection((vertical == 1) ? PlayerDirection.UP : PlayerDirection.DOWN);
        }
        else if (horizontal != 0)
        {
            PlayerController.SetUpInputDirection((horizontal == 1) ? PlayerDirection.RIGHT : PlayerDirection.LEFT);
        }
    }

    int GetAxisRaw(Axis axis)
    {
        if (axis == Axis.Horizontal)
        {
            bool left = Input.GetKeyDown(KeyCode.LeftArrow);
            bool right = Input.GetKeyDown(KeyCode.RightArrow);

            if (left)
                return -1;
            if (right)
                return 1;

            return 0;
        }
        else if (axis == Axis.Vertical)
        {
            bool up = Input.GetKeyDown(KeyCode.UpArrow);
            bool down = Input.GetKeyDown(KeyCode.DownArrow);

            if (up)
                return 1;
            if (down)
                return -1;

            return 0;
        }
        return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other.tag : " + other.tag);
        if(other.tag == "Walls" || other.tag == "Tail")
        {
            if (OnCollision != null)
                OnCollision();
        }
    }
}
