using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MovmentScript>(out MovmentScript player))
        {
            player.gameObject.transform.position = player.checkpointPosition;
            player.gameObject.transform.rotation = player.checkpointRotation;
            player.velocity = 0;
        }
    }
}
