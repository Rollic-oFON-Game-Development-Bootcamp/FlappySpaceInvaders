using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] private float sideMovementSensivity = 0.1f;
    [SerializeField] private float rotateMovemetSensivity = 1f;
    [SerializeField] private Transform TopLeftLimit;
    [SerializeField] private Transform BottomRightLimit;

    private Vector3 inputDrag;
    private Vector3 inputpreviousMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleMovement();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputpreviousMousePosition = Input.mousePosition;

        }
        if (Input.GetMouseButton(0))
        {
            Vector3 deltaMouse = Input.mousePosition - inputpreviousMousePosition;
            inputDrag = deltaMouse;
            inputpreviousMousePosition = Input.mousePosition;
        }
        else
        {
            inputDrag = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        Vector3 localPos = transform.position;
       
        localPos += inputDrag * sideMovementSensivity;
        localPos.x = Mathf.Clamp(localPos.x, TopLeftLimit.localPosition.x, BottomRightLimit.localPosition.x);
        localPos.y = Mathf.Clamp(localPos.y, BottomRightLimit.localPosition.y, TopLeftLimit.localPosition.y);

        Vector3 rotateRoll = Vector3.up * inputDrag.x * rotateMovemetSensivity;
        rotateRoll.y = Mathf.LerpAngle(0, rotateRoll.y + transform.rotation.eulerAngles.y, 0.95f);

        rotateRoll.y = Mathf.Clamp(rotateRoll.y, -45, 45);
        Debug.Log(" inputDrag = " + inputDrag.ToString() + " rotateRoll = " + rotateRoll.ToString());

        Quaternion rotation = Quaternion.Euler(0, rotateRoll.y, 0);

        transform.rotation = rotation;
        transform.position = localPos;
    }
}
