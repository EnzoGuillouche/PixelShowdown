using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special2Cooldown : MonoBehaviour
{
    private string specialName;
    // Start is called before the first frame update
    void Start()
    {
        specialName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(specialName == "1Spe2" ? GutsActions.spe2Cooldown1 : GutsActions.spe2Cooldown2, transform.localScale.y);
    }
}
