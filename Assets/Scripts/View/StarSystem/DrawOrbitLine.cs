﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawOrbitLine : MonoBehaviour
{
    float theta_scale = 0.02f;        //Set lower to add more points
    int size; //Total number of points in circle

    LineRenderer lineRenderer;

    [SerializeField]
    Material mat;

    public float radius;

    void Start()
    {
        float sizeValue = (2.0f * Mathf.PI) / theta_scale;
        size = (int)sizeValue;
        size++;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = mat;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.positionCount = size;
        lineRenderer.useWorldSpace = false;
    }

    private void Update()
    {
        Vector3 pos;
        float theta = 0f;
        for (int i = 0; i < size; i++)
        {
            theta += (2.0f * Mathf.PI * theta_scale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            pos = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, pos);
        }        
    }
}