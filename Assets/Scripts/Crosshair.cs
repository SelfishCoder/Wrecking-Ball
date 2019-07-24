using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CrosshairState crosshairState;
    [SerializeField] private Vector3 deflection;
    [SerializeField] private float speed;
    private float timeElapsed;
    private Vector3 newPosition;

    private void Start()
    {
        float deflectionX = ((Screen.width / 2) - (this.GetComponent<RectTransform>().sizeDelta.x / 2));
        float deflectionY = ((Screen.height / 2) - (this.GetComponent<RectTransform>().sizeDelta.y / 2));
        deflection = new Vector3(deflectionX, deflectionY, 0);
        timeElapsed = 0;
        newPosition = Vector3.zero;
        crosshairState = CrosshairState.OnMoveHorizontally;
    }

    private void MoveCrosshairHorizontally()
    {
        newPosition.x = Mathf.Sin(timeElapsed * speed) * deflection.x;
        transform.localPosition = newPosition;
    }

    private void MoveCrosshairVertically()
    {
        newPosition.y = Mathf.Sin(timeElapsed * speed) * deflection.y;
        transform.localPosition = newPosition;
    }

    private void Update()
    {
        switch (crosshairState)
        {
            case CrosshairState.OnMoveHorizontally:
                timeElapsed += Time.deltaTime;
                MoveCrosshairHorizontally();
                if (Input.GetMouseButtonDown(0))
                {
                    crosshairState = CrosshairState.OnMoveVertically;
                    timeElapsed = 0;
                }

                break;

            case CrosshairState.OnMoveVertically:
                timeElapsed += Time.deltaTime;
                MoveCrosshairVertically();
                if (Input.GetMouseButtonDown(0))
                {
                    crosshairState = CrosshairState.OnPowerSelection;
                }

                break;

            case CrosshairState.None:
                break;
            case CrosshairState.OnPowerSelection:
                break;
            default:
                break;
        }
    }
}