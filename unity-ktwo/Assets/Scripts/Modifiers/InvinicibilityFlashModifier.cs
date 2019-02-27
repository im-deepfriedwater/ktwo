using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To use this: activate this component by another script when you,
// want the flashing effect on deactivate it when you want it off.
public class InvinicibilityFlashModifier : MonoBehaviour
{
    [Tooltip("The object that has the mesh renderer should be named 'Graphics'. Gets filled in at runtime.")]
    public GameObject mesh; // The graphics object to activate / deactivate in and out rapidly.
    [Range(0, 1)]
    public float rateOfFlash = 0.1f; // Time in seconds it oscillates between off and on.

    float currentTimeInState;
    bool graphicsOn = false;

    // Start is called before the first frame update
    void Start()
    {
        mesh = transform.Find("Graphics").gameObject;
    }

    void OnDisable()
    {
        mesh.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        mesh.SetActive(graphicsOn);

        if (currentTimeInState > rateOfFlash)
        {
            graphicsOn = !graphicsOn;
            currentTimeInState = 0;
        }

        currentTimeInState += Time.deltaTime;
    }
}
