using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGreenLightAnimation()
    {
        GetComponent<Animation>().Play("GreenLightAnimation");
    }

    public void PlayRedLightAnimation()
    {
        GetComponent<Animation>().Play("RedLightAnimation");
    }
}
