using UnityEngine;

// Rotating beam, seen from a 2D view.
// Beam "rotates" around the lamp: pointing right/left it is at full length, and as it swings
// toward/away from the camera it shrinks toward the lamp, then grows out the other side.
// Done by squashing the pivot's X scale with cos(angle): +1 = full right, 0 = toward camera, -1 = full left.
//
// 'Beam' must be the PIVOT object (at the lamp, rotation 0,0,0, scale 1,1,1).
// The triangle sprite is a child of the pivot and keeps its own offset/rotation/scale.
public class Beacon : MonoBehaviour
{
    public Transform beam;               // the pivot (parent of the triangle)
    public SpriteRenderer beamSprite;    // the triangle
    public float degPerSec = 60f;
    [Range(0f, 1f)] public float maxAlpha = 0.45f;

    [Header("Fake 3D depth")]
    public bool depthSorting = true;     // beam passes in front of / behind the tower
    public int frontOrder = 6;           // drawn over the tower while swinging toward the camera
    public int backOrder = 0;            // drawn behind the tower while swinging away
    const float MinScale = 0.02f;        // avoids a zero scale at the exact turning point

    float angle;                         // degrees, 0 = pointing right

    void Update()
    {
        angle = Mathf.Repeat(angle + degPerSec * Time.deltaTime, 360f);

        float rad = angle * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad);
        if (Mathf.Abs(c) < MinScale) c = c < 0f ? -MinScale : MinScale;
        beam.localScale = new Vector3(c, 1f, 1f);   // negative X mirrors the beam to the other side

        if (beamSprite)
        {
            if (depthSorting)
                beamSprite.sortingOrder = Mathf.Sin(rad) >= 0f ? frontOrder : backOrder;

            var col = beamSprite.color;
            col.a = maxAlpha;
            beamSprite.color = col;
        }
    }
}
