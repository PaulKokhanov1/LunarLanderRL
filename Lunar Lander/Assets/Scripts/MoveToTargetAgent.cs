using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Google.Protobuf.WellKnownTypes;

public class MoveToTargetAgent : Agent
{

    public Rigidbody2D rb;
    public Collider2D bodyCollider;
    public Collider2D rightLegCollider;
    public Collider2D leftLegCollider;
    public Collider2D target;

    public float movementSpeed = 5f;
    public float turnSpeed = 1f;

    private Coroutine timer;
    private float speedRewardTotal = 0;
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
        rb.transform.localPosition = new Vector3(Random.Range(-8f, 8f), 3.5f,0); //changed starting position to only right side so agent trains better
        speedRewardTotal = 0;
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

        //decrease reward depending on angular rotation of ship at each time step
        //take absolute value to always have +ve value, then multiply by -ve to make sure we are decreasing reward as
        //we stray further from horizontal
        //Debug.Log(rb.rotation);
        float angularPositionReward = -Mathf.Abs(rb.rotation);
        AddReward(angularPositionReward*0.01f);

        //Debug.Log("Velocity is: x: " + rb.velocity.x + " , y: " + rb.velocity.y);

        //need to ensure we don't divide by zero
        float speed = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2));
        if (speed > 0.5f)
        {
            //increased/decreased the reward the slower/faster the lander is moving
            //so slower it moves the greater the reward
            //not as important so not varying the rewards much, 
            float velocityReward = 10 / speed;
            
            speedRewardTotal += velocityReward * 0.01f;
            AddReward(velocityReward * 0.01f);
        }


        //to handle case where timer commenced once episode is nearly done
 /*       if (this.StepCount > 950 )
        {
            StopCoroutine(timer);
        }*/

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
            //added rewards here to encourage not over-using hyppothetical "Fuel"
            case 1: //thrust main/up
                dirToGo = transform.up * 1f;
                AddReward(-0.03f);
                turn = 0;
                break;
            case 2: //thrust right
                dirToGo = transform.right * 1f;
                //rb.AddForceAtPosition(transform.up * movementSpeed, rb.transform.position + new Vector3(-0.5f, -0.5f, 0));
                AddReward(-0.03f);
                turn = -1f;
                break;
            case 3: //thrust left
                dirToGo = transform.right * -1f;
                //rb.AddForceAtPosition(transform.up * movementSpeed, rb.transform.position + new Vector3(0.5f, -0.5f, 0));
                AddReward(-0.03f);
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
            //also technically a crash
            AddReward(-100f);
            EndEpisode();

        } else if (bodyCollider.IsTouching(collision))
        {
            //once collision occurs between body and ground, unless we touch another surface or exit the trigger and 
            //trigger it once again, this section wont be called twice so dont have to worry about giving reward too many times for
            //inconsistent results
            //Debug.Log("Body Collider is in Contact");
            if(collision.TryGetComponent(out Ground ground))
            {
                //crash -100f
                //Debug.Log("Crahsed");
                AddReward(-100f);
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

/*    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Visual")
        {
            //punish agent for leaving the target
            AddReward(-10f);

        }

    }*/

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);

        //Debug.Log("timer finished");
        if (leftLegCollider.IsTouching(target) && rightLegCollider.IsTouching(target) && Mathf.Approximately(angle, 0))
        {
            float disFromCenterReward;
            if (transform.position.x > 0)
            {
                disFromCenterReward = (200 * Mathf.Pow(0.01f, transform.position.x)) + 100;

            }
            else
            {
                disFromCenterReward = (200 * Mathf.Pow(0.01f, -transform.position.x)) + 100;
            }
            AddReward(disFromCenterReward);
            //Debug.Log("Distance from Center Reward is: " +  disFromCenterReward);
            AddReward(100f);
        }
        //will need to eventually fix this so that doesnt continusoly add reward once ship lifts up and down again
        //can fix with a bool but will implement after I figure out how to stop movement once the rigidbody touches the target
        //or maybe just allow this behavior and see how the ML agent learns
        if (rightLegCollider.IsTouching(target))
        {
            //Debug.Log("Added 10f for RIGHT");
            AddReward(10f);
        }
        if (leftLegCollider.IsTouching(target))
        {
            //Debug.Log("Added 10f for LEFT");
            AddReward(10f);
        }

        //Debug.Log("Speed Reward is: " + speedRewardTotal);
        //Debug.Log(this.GetCumulativeReward());
        EndEpisode();
        
    }

}
