using UnityEngine;
using System.Collections;

/* **************************************************************************
 * CLASS: FPS COUNTER
 * *************************************************************************/
public class FPSCounter : MonoBehaviour
{
    /* Public Variables */
    public float frequency = 0.5f;

    /* **********************************************************************
     * PROPERTIES
     * *********************************************************************/
    public static int FramesPerSec { get; protected set; }

    /* **********************************************************************
     * EVENT HANDLERS
     * *********************************************************************/
    /*
     * EVENT: Start
     */
    private void Start()
    {
        StartCoroutine(FPS());
    }

    /*
     * EVENT: FPS
     */
    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it
            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
        }
    }
}