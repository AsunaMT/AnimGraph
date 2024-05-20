using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimGraph;

public class AnimGraphTest : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimGraphAsset animGraph;
    void Start()
    {
        if(animGraph != null)
        {
            var animator = GetComponent<Animator>();
            if(animator != null)
            {
                animGraph.Init(animator);
            }
            animGraph.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        animGraph?.Dispose();
    }
}
