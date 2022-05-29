using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/**
 * limiting the player sight
 */

public class VignetteController : MonoBehaviour
{

    public float intensity = 0.3f;
    public float duration = 3f;
    public Volume volume = null;

    private Vignette vignette = null;

    private bool isDim;

    void Awake()
    {
        isDim = false;

        if (volume.profile.TryGet(out Vignette vignette))
            this.vignette = vignette;
    }

    void Update() {

        if (!isDim)
            StartCoroutine(TurnDark());

    }

    IEnumerator TurnDark() {

        isDim = true;
        yield return new WaitForSeconds(duration);
        SetIntensity();
        isDim = false;
        yield return null;
    
    }

    public void SetIntensity(float value = 0.01f) {

        if (intensity < 0.7f)
            intensity += value;
        else if (intensity >= 0.7f)
            intensity = 0.7f;

        vignette.intensity.Override(intensity);

    }

    public void ResetIntensity() {

        intensity = 0.3f;
        vignette.intensity.Override(intensity);

    }

}
