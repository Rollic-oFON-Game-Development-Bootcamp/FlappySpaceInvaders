using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] private float sideMovementSensivity = 0.1f;
    [SerializeField] private float rotateMovemetSensivity = 1f;
    [SerializeField] private Transform TopLeftLimit;
    [SerializeField] private Transform BottomRightLimit;

    private float RotationLimit = 45;

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

        rotateRoll.y = Mathf.Clamp(rotateRoll.y, -RotationLimit, RotationLimit);

        Quaternion rotation = Quaternion.Euler(0, rotateRoll.y, 0);

        transform.rotation = rotation;
        transform.position = localPos;

        SideTransporting();
    }

    private float transportingElapsedTime = 0;
    private float transportingTimeLimit = 0.25f;
    private void SideTransporting()
    {
        if (transform.position.x == TopLeftLimit.localPosition.x || transform.position.x == BottomRightLimit.localPosition.x)
        {
            transportingElapsedTime += Time.deltaTime;

            if (transportingElapsedTime >= transportingTimeLimit)
            {
                SideTransportAnimate();
                transportingElapsedTime = 0;
            }

        }
        else
        {
            transportingElapsedTime = 0;
        }
    }

    private void SideTransportAnimate()
    {
        transform.DOShakeScale(0.25f, vibrato: 5, strength: 0.5f)
            .OnComplete(SideTransport);
    }

    private void SideTransport()
    {
        float x;
        if (transform.position.x == TopLeftLimit.localPosition.x)
        {
            x = BottomRightLimit.localPosition.x - 0.5f;
        }
        else
        {
            x = TopLeftLimit.localPosition.x + 0.5f;
        }

        transform.position = new Vector3(x, transform.position.y, transform.position.z);

    }

}
