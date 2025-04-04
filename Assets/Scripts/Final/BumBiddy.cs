using UnityEngine;
using UnityEngine.Video;

public class BumBiddy : MonoBehaviour
{
    public VideoPlayer videoplayer;

    bool init = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            init = true;

            videoplayer.Play();
        }
    }
}
