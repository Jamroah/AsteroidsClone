// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System.Collections;

// Allows us to control what axis the objects spawn from.
public enum AXIS_BIAS
{
    VERTICAL = 0,
    HORIZONTAL = 1,
    BOTH = HORIZONTAL | VERTICAL
}

public struct MathV2D
{
    public static Vector2 OnEdgeOfCircle(float deg, float radius)
    {
        float rad = deg * Mathf.Deg2Rad;
        return new Vector2(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad));
    }

    /// In the editor it gets the vector points of the editor camera rather than the game camera.
    /// Works absolutely as expected when built.
    public static Vector2 GetRandomVectorOutsideCamera(AXIS_BIAS bias, Camera camera)
    {
        float chanceX = Random.Range(0, 100);
        float chanceY = Random.Range(0, 100);

        // We only randomise the vector between 25 and 75 to ensure they result in off-screen coords
        // i.e.25 +- 1 = offscreen. 0 +- 1 = potentially on-screen.
        Vector2 coords = new Vector2(Random.Range(25, 75f) / 100f, Random.Range(25, 75f) / 100f);

        //Debug.Log("Old Coords: " + coords);

        // A different way would be to detect which edge the random coord is closest to and move the difference.
        // This'll do for for now.
        if (chanceX < 50 && bias == AXIS_BIAS.HORIZONTAL)
            coords.x -= 1;
        else if (chanceY < 50 && bias == AXIS_BIAS.VERTICAL)
            coords.y -= 1;
        else if (chanceX < 100 && bias == AXIS_BIAS.HORIZONTAL)
            coords.x += 1;
        else if (chanceY < 100 && bias == AXIS_BIAS.VERTICAL)
            coords.y += 1;

        // Clamp the coordinates
        coords.x = Mathf.Clamp(coords.x, -0.1f, 1.1f);
        coords.y = Mathf.Clamp(coords.y, -0.1f, 1.1f);

        //Debug.Log("New Coords: " + coords);
        return camera.ViewportToWorldPoint(coords);
    }

    /// <summary>
    /// Gets a random vector outside of the main camera. Option to choose axis bias.
    /// </summary>
    /// <param name="bias"></param>
    /// <returns></returns>
    public static Vector2 GetRandomVectorOutsideCamera(AXIS_BIAS bias)
    {
        return GetRandomVectorOutsideCamera(bias, Camera.main);
    }

    /// <summary>
    /// Gets a random vector outside of the main camera.
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetRandomVectorOutsideCamera()
    {
        return GetRandomVectorOutsideCamera(AXIS_BIAS.BOTH);
    }

    //public static Quaternion LookAt2D(Transform from)
    //{

    //}
}
