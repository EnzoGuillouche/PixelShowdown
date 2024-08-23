using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexBehavior : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.parent.GetChild(transform.parent.childCount - 1).localPosition.x, transform.parent.GetChild(transform.parent.childCount - 1).localPosition.y+3);
    }
}
