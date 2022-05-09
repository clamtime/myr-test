using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // gravity
    public float gravityScale = 9.81f;
    public Vector3 gravityDirection = Vector3.down;

    // transform
    public Vector3 size = Vector3.one;

    // apperance
    public Color color;

    
    // components
    private Rigidbody rb;
    private Renderer ren;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ren = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(gravityDirection * gravityScale, ForceMode.Force);
    }

    /// <summary>
    /// Change the gravity for the character.
    /// </summary>
    /// <param name="scale">The speed of the gravity.</param>
    /// <param name="direction">A unit vector direction of the gravity.</param>
    public void ChangeGravity(float scale, Vector3 direction)
    {
        gravityScale = scale;
        gravityDirection = direction.normalized;
    }

    /// <summary>
    /// Change the scale of an object.
    /// </summary>
    /// <param name="size">What to change the scale to.</param>
    public void ChangeSize(Vector3 newSize)
    {
        size = newSize;
        transform.localScale = size;
    }

    /// <summary>
    /// Change the color of an object.
    /// </summary>
    /// <param name="col">What to change the materials color to.</param>
    public void ChangeColor(Color col)
    {
        color = col;
        ren.material.color = color;
    }
}
