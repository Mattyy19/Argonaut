using UnityEngine;

public class WorldCheckpoint : MonoBehaviour
{
    public Health playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint")
        {
            activateCheckpoint(other.transform.GetChild(0));
        }
    }

    public void activateCheckpoint(Transform flag)
    {
        flag.gameObject.SetActive(true);
        playerHealth.setCheckpointPos(flag.transform.position + new Vector3(0, 2));
    }
}
