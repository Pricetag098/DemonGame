using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

public class WeaponSway : MonoBehaviour
{
    [Header("BobSettings")]
    [SerializeField] AnimationCurve horizontalFrequencyCurve;
    [SerializeField] float horizontalFrequencyVarience;
    [SerializeField] AnimationCurve horizontalOffsetCurve;
    [SerializeField] float horizontalOffsetVarience;

    [Header("Vertical Bob Settings")]
    [SerializeField] AnimationCurve verticalFrequencyCurve;
    [SerializeField] float verticalFrequencyVarience;
    [SerializeField] AnimationCurve verticalOffsetCurve;
    [SerializeField] float verticalOffsetVarience;

    float horizontalOffset, verticalOffset;
    float horizontalFrequency, verticalFrequency;

    float moveTimerHorizontal, moveTimerVertical;

    PlayerInputt playerInput;

    Vector3 originPos;
    Vector3 newPosHorizontal, newPosVertical;
    Vector3 lastPosHorizontal, lastPosVertical;

    bool movingLeft, movingRight, movingUp, movingDown;


    void Awake()
    {
        playerInput = FindAnyObjectByType<PlayerInputt>();
    }

    private void Start()
    {
        originPos = transform.localPosition;
        movingLeft = true;
        movingUp = true;

        GetNewHorizontalSway();
        GetNewVerticalSway();
    }

    void Update()
    {
        Debug.Log("Velocity Magnitude: " + playerInput.rb.velocity.magnitude); // Debug velocity magnitude

        // Horizontal Sway
        moveTimerHorizontal += Time.deltaTime;
        float newX = Mathf.Lerp(lastPosHorizontal.x, newPosHorizontal.x, moveTimerHorizontal / horizontalFrequency);
        Debug.Log("Horizontal Lerp: " + newX);  // Debug Lerp calculation for horizontal movement

        if (Vector3.Distance(transform.localPosition, newPosHorizontal) < 0.01f)
        {
            Debug.Log("Switching Horizontal Direction");
            movingLeft = !movingLeft;
            movingRight = !movingRight;
            GetNewHorizontalSway();
        }

        // Vertical Sway
        moveTimerVertical += Time.deltaTime;
        float newY = Mathf.Lerp(lastPosVertical.y, newPosVertical.y, moveTimerVertical / verticalFrequency);
        Debug.Log("Vertical Lerp: " + newY);  // Debug Lerp calculation for vertical movement

        if (Vector3.Distance(transform.localPosition, newPosVertical) < 0.01f)
        {
            Debug.Log("Switching Vertical Direction");
            movingUp = !movingUp;
            movingDown = !movingDown;
            GetNewVerticalSway();
        }

        // Apply both horizontal and vertical movement together
        transform.localPosition = new Vector3(newX, newY, originPos.z);
        Debug.Log("New Position: " + transform.localPosition);  // Debug the new position of the object
    }

    // Get new horizontal sway
    public void GetNewHorizontalSway()
    {
        float f = horizontalFrequencyCurve.Evaluate(playerInput.rb.velocity.magnitude);
        float minF = f - (horizontalFrequencyVarience / 2);
        float maxF = f + (horizontalFrequencyVarience / 2);
        horizontalFrequency = Random.Range(minF, maxF);
        Debug.Log("Horizontal Frequency: " + horizontalFrequency);  // Debug the horizontal frequency

        float o = horizontalOffsetCurve.Evaluate(playerInput.rb.velocity.magnitude);
        float minO = o - (horizontalOffsetVarience / 2);
        float maxO = o + (horizontalOffsetVarience / 2);
        horizontalOffset = Random.Range(minO, maxO);
        Debug.Log("Horizontal Offset: " + horizontalOffset);  // Debug the horizontal offset

        lastPosHorizontal = transform.localPosition;

        if (movingLeft)
        {
            newPosHorizontal = new Vector3(originPos.x - horizontalOffset, originPos.y, originPos.z);
        }
        else
        {
            newPosHorizontal = new Vector3(originPos.x + horizontalOffset, originPos.y, originPos.z);
        }

        moveTimerHorizontal = 0f;
    }

    // Get new vertical sway
    public void GetNewVerticalSway()
    {
        float f = verticalFrequencyCurve.Evaluate(playerInput.rb.velocity.magnitude);
        float minF = f - (verticalFrequencyVarience / 2);
        float maxF = f + (verticalFrequencyVarience / 2);
        verticalFrequency = Random.Range(minF, maxF);
        Debug.Log("Vertical Frequency: " + verticalFrequency);  // Debug the vertical frequency

        float o = verticalOffsetCurve.Evaluate(playerInput.rb.velocity.magnitude);
        float minO = o - (verticalOffsetVarience / 2);
        float maxO = o + (verticalOffsetVarience / 2);
        verticalOffset = Random.Range(minO, maxO);
        Debug.Log("Vertical Offset: " + verticalOffset);  // Debug the vertical offset

        lastPosVertical = transform.localPosition;

        if (movingUp)
        {
            newPosVertical = new Vector3(originPos.x, originPos.y + verticalOffset, originPos.z);
        }
        else
        {
            newPosVertical = new Vector3(originPos.x, originPos.y - verticalOffset, originPos.z);
        }

        moveTimerVertical = 0f;
    }
}
