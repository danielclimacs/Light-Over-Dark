using UnityEngine;

// Rotating beam seen from a 2D view.
// Beam "rotates" around the lamp: pointing right/left it is at full length, and as it swings
// toward/away from the camera it shrinks toward the lamp, then grows out the other side.

public class Beacon : MonoBehaviour
{
    public Transform beam;               // pivot (parent of the triangle)
    public SpriteRenderer beamSprite;    // triangle object
    public float degPerSec = 60f;

    [Range(0f, 1f)]
    public float transparency = 0.45f;

    [Header("Fake 3D depth")]
    public int frontOrder = 6;
    public int backOrder = 0;
    const float MinScale = 0.02f;        // avoids no beam object when pointing directly at or away from screen

    float angle;                         // degrees, 0 = pointing right

    void Update()
    {
        angle = Mathf.Repeat(angle + degPerSec * Time.deltaTime, 360f);

        float rad = angle * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad);
        if (Mathf.Abs(c) < MinScale)
        {
            if (c < 0f)
            {
                c = -MinScale;
            }
            else
            {
                c = MinScale;
            }
        }

        beam.localScale = new Vector3(c, 1f, 1f);

        if (beamSprite)
        {
            beamSprite.sortingOrder = Mathf.Sin(rad) >= 0f ? frontOrder : backOrder;
            var col = beamSprite.color;
            col.a = transparency;
            beamSprite.color = col;
        }
    }
}
