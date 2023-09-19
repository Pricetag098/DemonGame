using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum TweenDirection { Up, DiagonalUpRight, Right, DiagonalDownRight, Down, DiagonalDownLeft, Left, DiagonalUpLeft}
public class DirectionTweens
{
    public Vector2 startPosition;

    private float value = 1000f;

    public void ChooseTweenDirection(TweenDirection direction)
    {
        switch (direction)
        {
            case TweenDirection.Up:
                {
                    startPosition = new Vector2(0, -value);
                    break;
                }
            case TweenDirection.DiagonalUpRight:
                {
                    startPosition = new Vector2(value, value);
                    break;
                }
            case TweenDirection.Right:
                {
                    startPosition = new Vector2(value, 0f);
                    break;
                }
            case TweenDirection.DiagonalDownRight:
                {
                    startPosition = new Vector2(value, -value);
                    break;
                }
            case TweenDirection.Down:
                {
                    startPosition = new Vector2(0f, value);
                    break;
                }
            case TweenDirection.DiagonalDownLeft:
                {
                    startPosition = new Vector2(-value, -value);
                    break;
                }
            case TweenDirection.Left:
                {
                    startPosition = new Vector2(-value, 0f);
                    break;
                }
            case TweenDirection.DiagonalUpLeft:
                {
                    startPosition = new Vector2(-value, value);
                    break;
                }
        }
    }
}
