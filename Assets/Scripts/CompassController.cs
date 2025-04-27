using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour
{
    public RawImage compassImage;
    public Transform player;

    void Update()
    {
        if (player != null && compassImage != null)
        {
            float playerYaw = player.eulerAngles.y;
            float uvOffset = playerYaw / 360f;

            // Ensure it always stays between 0 and 1 (looping)
            uvOffset %= 1.0f;

            compassImage.uvRect = new Rect(uvOffset, 0f, 1f, 1f);
        }
    }
}
