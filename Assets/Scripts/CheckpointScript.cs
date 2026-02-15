using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MovmentScript>(out MovmentScript player))
        {
            Vector3 spawnPosition = this.transform.position - this.transform.forward * 2;
            player.checkpointPosition = new Vector3(spawnPosition.x, player.gameObject.transform.position.y, spawnPosition.z);
            player.checkpointRotation = this.transform.rotation;
        }
    }
}
