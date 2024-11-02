﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum JoystickType 
{ 
    Fixed, 
    Floating, 
    Dynamic 
}

public class VariableJoystick : Joystick
{
	[SerializeField] private float moveThreshold = 1;
	[SerializeField] private JoystickType joystickType = JoystickType.Fixed;

    private Vector2 fixedPosition = new Vector2(256, 256);

	private void Awake()
	{
        SetMode(joystickType);
	}

	private void SetMode(JoystickType joystickType)
    {
        if (joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
        {
			background.gameObject.SetActive(false);
		}
	}

    protected override void Start()
    {
        base.Start();

        fixedPosition = background.anchoredPosition;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(joystickType != JoystickType.Fixed)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
        }

        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
			background.gameObject.SetActive(false);
		}

        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }

        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

