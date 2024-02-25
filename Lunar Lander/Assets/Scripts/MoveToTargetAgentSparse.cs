using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Google.Protobuf.WellKnownTypes;

//Script for Sparse Rewards
public class MoveToTargetAgentSparse : Agent
{

    public Rigidbody2D rb;
    public Collider2D bodyCollider;
    public Collider2D rightLegCollider;
    public Collider2D leftLegCollider;
    public Collider2D target;

    public float movementSpeed = 5f;
    public float turnSpeed = 1f;

    private Coroutine timer;
    private float angle;

    /* For Rewards
     * is increased/decreased the closer/further the lander is to the landing pad & is touching the landing pad. - done
     * is increased/decreased the slower/faster the lander is moving. - done
     * is decreased the more the lander is tilted (angle not horizontal). - done
     * is increased by 10 points for each leg that is in contact with the ground when episode ends. - done
     * is decreased by 0.03 points each frame a side engine is firing. - done
     * is decreased by 0.3 points each frame the main engine is firing. - done
     */

    public override void OnEpisodeBegin()
    {
        if (timer != null)
        {
            //Debug.Log("timer Stopped");
            StopCoroutine(timer);
            timer = null;
        }
        rb.transform.localPosition = new Vector3(Random.Range(-8f, 8f), 3.5f,0); //changed starting position to allow for smaller area of starting
        rb.velocity = Vector3.zero;

        rb.rotation = 0;
        rb.angularVelocity = 0;
        
    }

    //what the agent can "See"
    public override void CollectObservations(VectorSensor sensor)
    {

        Vector3 lunarLanderPosition = transform.parent.transform.InverseTransformPoint(rb.position);
        Vector3 lunarLanderVelocity = rb.velocity;
        float lunarLanderAngle = rb.rotation;
        float lunarLanderAngularVelocity = rb.angularVelocity;
        


        sensor.AddObservation(lunarLanderPosition.x);
        sensor.AddObservation(lunarLanderPosition.y);


        sensor.AddObservation(lunarLanderVelocity.x);
        sensor.AddObservation(lunarLanderVelocity.y);

        sensor.AddObservation(lunarLanderAngle);
        sensor.AddObservation(lunarLanderAngularVelocity);

    }



    //Agent recieves action and this function moves the agent
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Move the agent using the action.
        MoveAgent(actions.DiscreteActions);

    }

    /// <summary>
    /// Moves the agent according to the selected action.
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        
        var dirToGo = Vector3.zero;
        float turn = 0;

        var action = act[0];

        switch (action)
        {
            case 1: //thrust main/up
                dirToGo = transform.up * 1f;
                turn = 0;
                break;
            case 2: //thrust right
                dirToGo = transform.right * 1f;
                turn = -1f;
                break;
            case 3: //thrust left
                dirToGo = transform.right * -1f;
                turn = 1f;
                break;
        }
        rb.AddForce(dirToGo * movementSpeed, ForceMode2D.Force);
        rb.AddTorque(turnSpeed * turn );
    }

    //allows us to move the AI as needed for testing
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 2; //thrust right
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1; //thrust main
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 3; //thrust left
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Walls wall))
        {
            EndEpisode();

        } else if (bodyCollider.IsTouching(collision))
        { 

            if(collision.TryGetComponent(out Ground ground))
            {
                EndEpisode();                
                
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "Visual")
        {
            //Debug.Log("collided");
            Vector3 normal = collision.contacts[0].normal;
            //Debug.Log(normal);
            angle = Vector3.Angle(normal, Vector3.up);

            //Debug.Log(timer);
            if (timer == null)
            {
                //Debug.Log("Timer Started");
                timer = StartCoroutine(Timer());
            }

        }

    }


    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);

        //Debug.Log("timer finished");
        if (leftLegCollider.IsTouching(target) && rightLegCollider.IsTouching(target) && Mathf.Approximately(angle, 0))
        {
            AddReward(100f);
        }
        Debug.Log(this.GetCumulativeReward());
        EndEpisode();
        
    }

}
