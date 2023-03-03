using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class CustomerMovement : MonoBehaviour
{
    NavMeshAgent nm;
    Rigidbody rb;
    Animator anim;

    public Transform spawnPoint;

    public Transform target;
    public List<Transform> wayPoints = new List<Transform>();
    public int currWayPoint;

    public float speed, stopDistance;
    public float offset;

    public float pauseTimer;

    private float currTimer;

    public float rotationSpeed;

    private bool gotItem = false;
    private bool isInspecting = false;
    private bool leaveStore = false;
    private bool inLine = false;

    public TransactionManager transactionManager;
    public ReputationSystem repSystem;
    public GameController gameController;

    public bool LeaveStore {
        get { return leaveStore; }
        set { leaveStore = value; }
    }

    public bool InLine
    {
        get { return inLine; }
        set { inLine = value; }
    }

    public bool IsInspecting
    {
        get { return isInspecting; }
    }



    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        target = wayPoints[currWayPoint];
        currTimer = pauseTimer;
    }

    // Update is called once per frame
    void Update()
    {
        nm.acceleration = speed;

        if (gotItem == false)
        {
            nm.stoppingDistance = stopDistance;
            float distance = Vector3.Distance(transform.position, target.position); //calculate distance between this and target
            //move to waypoint
            if (distance > stopDistance + offset && wayPoints.Count > 0) //moving
            {
                //Animator: set bool for moving = true
                //animator: set bool for idle = false
                //find waypoint
                target = wayPoints[currWayPoint];

            }
            else if (distance <= stopDistance + offset && wayPoints.Count > 0) //arrived to destination
            {
                DisplayTable table = wayPoints[currWayPoint].gameObject.GetComponent<DisplayTable>();

                //make customer turn to face the table
                transform.LookAt(table.transform);
                


                //if the table is unlocked and has an item for sale
                if (!table.IsLocked && table.IsItemForSale)
                {
                    table.IsLocked = true; //lock the table from other customers
                    isInspecting = true;
                }

                if (currTimer > 0)
                {
                    currTimer -= Time.deltaTime;
                    //Animator : Set Bool for moving = False
                    //Animator : Set Bool for Idle = True;
                }
                if (currTimer <= 0)
                {
                    if (isInspecting == true)
                    {
                        isInspecting = false;

                        KeyValuePair<PotionItem, int> info = table.GetPotionInfo();
                        int pricePoint = GetComponent<Customer>().ReactToItemPrice(info.Key, info.Value);

                        if (pricePoint < 3)
                        {
                            //purchase item
                            GetComponent<Customer>().TakePotionFromTable(table);

                            gotItem = true;

                            if (pricePoint == 0)
                            {
                                repSystem.reputation += 1f;
                                if (repSystem.reputation > 100f)
                                {
                                    repSystem.reputation = 100f;
                                }
                                repSystem.UpdateSlider(repSystem.reputation / 100f);
                                gameController.UpdateSpawnRate(-0.1f);
                            }
                            else if (pricePoint == 1)
                            {
                                repSystem.reputation += 0.5f;
                                if (repSystem.reputation > 100f)
                                {
                                    repSystem.reputation = 100f;
                                }
                                repSystem.UpdateSlider(repSystem.reputation / 100f);
                                gameController.UpdateSpawnRate(-0.05f);
                            }
                            else if (pricePoint == 2)
                            {
                                repSystem.reputation -= 1f;
                                if (repSystem.reputation < 0f)
                                {
                                    repSystem.reputation = 0f;
                                }
                                repSystem.UpdateSlider(repSystem.reputation / 100f);
                                gameController.UpdateSpawnRate(0.1f);
                            }
                            else if (pricePoint == 3) {
                                repSystem.reputation -= 2f;
                                if (repSystem.reputation < 0f)
                                {
                                    repSystem.reputation = 0f;
                                }
                                repSystem.UpdateSlider(repSystem.reputation / 100f);
                                gameController.UpdateSpawnRate(0.2f);
                            }
                        }
                        else
                        {
                            //make customer go to spawn point and destroy 
                            leaveStore = true;
                            gotItem = true; //oopsies
                            table.IsLocked = false;
                            return;
                        }


                        table.IsLocked = false;

                    }
                    else
                    {
                        currWayPoint++;
                        if (currWayPoint >= wayPoints.Count)
                        {
                            currWayPoint = 0;
                        }
                        target = wayPoints[currWayPoint];
                        currTimer = pauseTimer;

                    }

                }
            }
            nm.SetDestination(target.position);
        }
        else if (gotItem == true && inLine == false && leaveStore == false)
        { //customer retrieved the potion and needs to check out
            //transaction manager is now handling customer movement, add the customer to the list 
            nm.stoppingDistance = 0;
            transactionManager.GetInLine(GetComponent<Customer>());
            inLine = true;
        }
        else if (gotItem == true && inLine == true && leaveStore == false) { 
            
        }
        else if (leaveStore == true)
        { //customer is leaving 
            nm.SetDestination(spawnPoint.position);
            Vector3 offsetVec = new Vector3(offset, offset, offset);
            nm.stoppingDistance = 0;
            //once the customer enters the spawnPoint trigger, it will be destroyed
        }
        

        
    }

}
