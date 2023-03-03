using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TransactionManager : MonoBehaviour
{
    public int maxQueueSize = 10;
    public float lineGap = 2f;
    private List<Vector3> linePositions = new List<Vector3>();
    public List<Customer> customers = new List<Customer>(); //a list of customers waiting to make their purchase, directly correlates with linePositions so customer[0] is at linePosition[0]

    public Transform cashRegister;
    //queue of customers waiting to purchase
    // Start is called before the first frame update
    void Start()
    {

        //set the transform positions of linePositions
        Vector3 cashRegPos = cashRegister.transform.position;
        Vector3 cashRegDir = cashRegister.transform.forward;
        Quaternion cashregRot = cashRegister.transform.rotation;
        for (int i = 0; i < maxQueueSize; i++)
        {
            
            Vector3 newPosition = cashRegPos + cashRegDir * lineGap * (i+1); //dont know if this is right
            linePositions.Add(newPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //for every loop, set the destination to their position in line
        for (int i = 0; i < customers.Count; i++)
        {
            customers[i].gameObject.GetComponent<NavMeshAgent>().SetDestination(linePositions[i]);
            customers[i].gameObject.GetComponent<NavMeshAgent>().stoppingDistance = 0;

        }
    }

   
    public void GetInLine(Customer customer)
    {
        customers.Add(customer);
    }

    public void MakePurchaseAndLeave() {
        if (customers.Count == 0) {
            Debug.Log("No one is in line");
            return;
        }
        Collider[] hits = Physics.OverlapSphere(linePositions[0], 1.75f);
        if (hits.Length == 0) {
            Debug.Log("Let the customer reach the front");
            return;
        }

        Customer customer = customers[0];

        customer.MakePurchase();

        customer.GetComponent<CustomerMovement>().InLine = false; //customer is no longer in line

        customers.Remove(customer);

        //customer leaves store
        customer.gameObject.GetComponent<CustomerMovement>().LeaveStore = true;


    }

    public int GetCustomerIndexInLine(Customer customer) {
        for (int i = 0; i < customers.Count; i++) {
            if (customers[i] == customer) {
                return i;
            }
        }

        return -1;
    }
}
