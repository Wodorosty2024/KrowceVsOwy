using UnityEngine;

public class Parallex : MonoBehaviour
{
    private float length, startpos;
    public float parallexEffect;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        startpos = transform.position.x;
        length = sr.bounds.size.x;
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = new Vector2(length * 3f, sr.size.y);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = -PlayerController.instance.currentHorizontalSpeed * parallexEffect;

        float newpos = transform.position.x + dist;

        if (newpos > startpos + length)
        {
            newpos -= length;
        }
        else if (newpos < startpos - length)
        {
            newpos += length;
        }
        transform.position = new Vector3(newpos, transform.position.y, transform.position.z);
    }
}