using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillation : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField]Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period = 2f;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon){ return; } //to protect from naN error aka not a number //epsilon is a tiny number when sometimes 0 is not absolute and error might occur cuz of small difference we use this to calculate smallest of numbers
        float cycles = Time.time / period; //continually growing over time

        const float tau = Mathf.PI * 2; //const value of 6.283
        float rawSinWave = Mathf.Sin(cycles * tau); //going from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f; //recalculating for it to go from 0 to 1

        Vector3 offset = movementVector*movementFactor; //variable offset of type vector3
        transform.position = startingPosition + offset;
    }
}
