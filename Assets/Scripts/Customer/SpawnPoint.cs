using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    private GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer") && other.gameObject.GetComponent<CustomerMovement>().LeaveStore == true) {
            controller.currCustomers -= 1;
            controller.uiController.customerAudioSource.clip = controller.customerLeaveStoreSound;
            controller.uiController.customerAudioSource.Play();

            other.gameObject.GetComponent<ReactionUI>().DestroyReactionUI();
            Destroy(other.gameObject);
        }
    }
}
