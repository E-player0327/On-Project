using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAnimation : MonoBehaviour
{
    public bool isHeadIdle;
    public float time { get; set; }

    private void FixedUpdate()
    {
        if (isHeadIdle)
        {
            time += Time.fixedDeltaTime;
            if (time < 2.25f)
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.0053f * (2.25f - time), transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.0053f * (4.5f - time), transform.position.z);


            if (time > 4.5f)
                time = 0;
        }
    }
}
