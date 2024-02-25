# LunarLanderRL

<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a name="readme-top"></a>
<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![LinkedIn][linkedin-shield]][linkedin-url]




<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/PaulKokhanov1/LunarLanderRL">
    <img src="https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/0dcf9a8d-a634-41c3-ad21-5ba8304bc403" alt="Logo" width="280" height="200">
  </a>

<h3 align="center">Lunar Lander Reinforcement Learning within Unity's ML Agents</h3>

  <p align="center">
    In Light of finishing Andrew Ng’s Machine Learning Specialization, prior to advancing onto the Deep Learning Specialization, I wanted to explore a practical use case of reinforcement learning within simulations. Specifically, during the lectures Andrew mentions an ML environment developed by OpenAI that creates a Lunar Lander with the goal of landing the rover within a specified landing pad. 
    <br />
    <a href="https://github.com/Unity-Technologies/ml-agents"><strong>Explore the docs »</strong></a>
    <br />
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li><a href="#getting-started">Getting Started</a></li>
    <li><a href="#about-my-implementation">About My Implementation</a></li>
    <li><a href="#obstacles-and-observations">Obstacles and Observations</a></li>
    <li><a href="#results">Results</a></li>
    <li><a href="#conclusion">Conclusion</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

## Important Note
If you plan to clone and try out the project, you will have to make a virtual environment within the project folder and download the necessary packages, I did not include the venv in my commit as it stored too large of files for 
github to handle, if you would like to know all needed packages, either refer to MLAgents docs or check out this video: https://www.youtube.com/watch?v=RANRz9oyzko&ab_channel=TheAshBot

<!-- ABOUT THE PROJECT -->
## About The Project
<div align="center">
  <a href="https://github.com/PaulKokhanov1/LunarLanderRL">
    <img src="https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/6ed735d2-09d7-42f3-bfe3-01473ff121c9" alt="Logo" width="280" height="200">
    <img src="https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/efdba43a-cc5b-4a62-b447-c9cd2fac6ba2" alt="Logo" width="280" height="200">
  </a>
</div>

My goal was to recreate the Lunar Lander environment using Unity’s MLAgents library. (Mine left, OpenAI’s Right)`

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

* [![CSharp][CSharp.com]][CSharp-url]
* [![Unity][Unity.com]][Unity-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->
## Getting Started

### Q-Learning

So, lets break down the method that we could use to train this “Agent” (i.e our Lunar Lander). First, we need to get one thing straight, simply put, Reinforcement Learning is a branch of machine learning used to train a model to behave a certain way to reach a goal through obtaining desired User-defined rewards and avoiding User-defined “Negative” Rewards. Let’s introduce the “State-Action function” or “Q-function”, essentially, this is the function we use to find a “return” (i.e cumulative reward) for a set of actions. Without going too in-depth, to calculate the Q-function, we use the Bellman Equation,

![image](https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/e02e3934-a147-40fd-99ed-72a6beeed919)

As you can see its defined as the sum of the Rewards you get right away (at your current state) and the rewards you get from behaving in the most optimal fashion for any further states and actions. How do we find the most optimal behavior starting from the next states? Well, think of it as a recursive function. Essentially, we are going down a decision tree, iterating through the different possibilities of states and actions until we reach a base case where we propagate back up the tree taking the maximum of the different decisions we could have made (I’d suggest reading on recursion as it would take quite some time for me to explain it here, https://www.freecodecamp.org/news/how-recursion-works-explained-with-flowcharts-and-a-video-de61f40cb7f9/ ), hence the “optimal” behavior. Now the gamma variable is our discount factor that we introduce to the model to essentially punish the model for taking a long time to find the optimal set of actions and reach the goal. For example, if Gamma was = 0.5 and say we needed to take 10 state and action pairs to get to our final state, then, each time we’d multiple our reward by the gamma value powered to the corresponding state, i.e for the next state it’d be 0.5*Reward, then 0.5^2*Reward, then 0.5^3*Reward, so on and so forth. 

![image](https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/0a34ed8e-3c4b-4e4b-b4b8-4a9d9195b63b)

By the time we reach our goal and get a huge reward, it’s going to be discounted by 0.5^10 * (Final Reward). As you can see the more state and actions we need to use, the lower our return is for the following state and action pairs, thus it enforces to use as little state and action pairs as possible to reach our goal. 

Unfortunately, in practice, it’s not always this simple, as with everything in life, we have randomness and so we must take that into account. There’s no need to really spend too much time explaining, but essentially, if we had a robot and say, given our Q-function we want its next state and action to be to "move left", what if it slips and falls? Well, it might go back to a previous state or even a state we did not expect. Thus, within our Q-function we typically take the average of the “optimal” state and action pairs after our current state. Specifically,

![image](https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/13a21bc4-fc16-497c-a400-43536c898f56)

Now we get into the nitty gritty (I will try my best to explain but feel free to criticize me and send me a detailed message explaining how bad I am at explaining 😊). Here I’m going to talk about specifically training the Lunar Lander. So, in our case, we have a continuous state space (which is why we wouldn't typically use this method for reinforcement learning, more on that in the PPO section), meaning that we don’t have discrete states, like state 1, state 2, etc… Rather our states can be a vector of a bunch of values. In my case, I defined them as: x-position, y-position, x-velocity, y-velocity, angle of lander with respect to horizontal plane, angular velocity and two Boolean values stating whether the left or right leg is touching our landing pad/area. Furthermore, I also had 4 possible actions: Do nothing, thrust right, thrust left, thrust up. 

Our approach to train the model is to use a neural network, which, simply put, is a method to find outputs given a certain input. Considering our case, we use “supervised learning”, meaning we train the neural network through first giving it inputs and known outputs. The best way to explain is through an example, say you have a bunch of images of cats (or dogs, whichever you prefer) and for each picture we have a label saying if it’s a cat or not. We use these inputs and known outputs to train our model so that when we give it a picture of something (cat or not), the model, using all the training we gave it, can classify whether this new input is a picture or a cat or not. Essentially, that is what we do in the Lunar Lander task, first, we must make a training set (our pictures of cats and their labels) using our Bellman Equation. So we start by making the Lunar Lander randomly move around and gather needed results, such as; its current state, its current action it preforms, the current reward it will get for preforming this action and the state it will move to by preforming this action. Now remember our Bellman equation?

![image](https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/36b7f5d0-1b23-45b0-8a9f-e4ae07c8f108)

Well, after gathering all this info, usually around 10,000 sets worth of info, we create our input and known output. Look at the left side of the above equation. This is our input, our current state and current action, let this be “x”. The right side of the equation is our output “y” obtained by using our current reward it will get for performing this action and the state it will move to by performing this action. What about “ a’ “? Well, this is simply considering all possible next actions we can take, we established that when creating our project, i.e in my case: Do nothing, thrust right, thrust left, thrust up. But also, you probably don’t understand how we can compute the Q-function even if we have the needed components. GUESS. Literally we just guess (when we first start training the neural network). By guessing the Q-function we can create our inputs and known outputs, and then by using these inputs and outputs, we train the Neural Network to make a better estimation of the Q-function. 

Crazy enough what we are doing is repeatedly improving our Q-function by going through the above process, and updating our Q-function with new inputs and known outputs from our old Q-function.

Now if you’re still following (probably not due to my poor explanation) , we use this refined Q-function to dictate how our lunar lander should behave. Basically, given a current state, we iterate through the different possible actions and choose whichever one will maximize the reward. Essentially, we want to be greedy and take the largest possible reward at each step. This, in most cases, will cause our lunar lander to take a pretty good path to our landing area, but of course, there is always a chance that it will screw up, and sometimes it might even learn to take a worse path then the optimal. Smarter people than me have obviously considered this and have refined the algorithm in ways to minimize that chance, but we’ll save those explanations for another time. Overall, that is our basics for one way we can implement reinforcement learning.

TLDR: We move around like crazy when first starting training to gather random info points. We use those info points to train an algorithm to make an equation that lets us approximate the reward we get when choosing certain actions based on our current state. Then we iterate through all possible actions and choose the action that yields the greatest return. Continue this until Lunar Lander lands or inevitably sometimes crashes and burns.

### PPO

If the above explanation was not long enough, I unfortunately have to let you know that the above method of maximizing our Q-function is just once way we could train our Agent. It’s often used for discrete sample spaces. However, in my case, where I used MLAgents, their library uses PPO (they have other algorithms as well, but I used PPO), so what is PPO and how is it different?

Lets start by taking about what it is. First we have to explain a policy. In machine learning it is basically an equation that the agent uses to make decisions given their current state. So if we recall our above algorithm, we can consider the policy as choosing the largest Q-value from the possible actions. Now, PPO (Proximal Policy Optimization) also works with policies except it defines a policy and continually updates this policy. 

The policy is updated conservatively by finding a ratio between the current and previous policy, then clipping the ratio between the values of [1 – epsilon, 1 + epsilon] to avoid too large of a change. Typically, epsilon is 0.2.

But what is this policy that we are updating? Well with PPO it varies depending on if we talk about a discrete time space or a continuous one. In my case we are dealing with a continuous time space thus one possible policy we use is the Gaussian Policy,

![image](https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/c3436983-565f-443c-a708-df59d45ba4a7)

I’d suggest reading on the underlining math of the Gaussian policy but essentially it creates a PDF (probability desnity function) for the possible actions we can take, and we see taking a particular action is higher when that action is closer to the mean of the data.

So, using this possible policy, PPO now constraints updating it as I mentioned before between [1 – epsilon, 1 + epsilon]. We come with the following scary equation:

![image](https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/4ab7e14f-59ea-4df3-b7a7-b943850f52c4)

But really its not too overwhelming. All we are doing is taking the average over multiple time steps of the minimum between two values. The 1st value is our ratio between the current and old policy multiplied by this “ A_t “ value. This value is called the “Advantage function” which just measures how good the chosen action is in terms of the future expected rewards. The advantage function calculation actually involves the Q-function that we talked about earlier! The 2nd value is essentially the same as the 1st but this is the part where we clip the ratio*Advantage function so it doesn’t cause the policy to update too drastically.

Now knowing all this we can talk about the process of PPO. 

![image](https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/cb0b0c70-654c-4ac5-ab11-0d94c43a3d1d)

Here is the basic algorithm of PPO with our Clipped Objective. If we go through each of these line by line here is what they mean. To begin, we have to create a Neural Network with some amount of layers and units defined by the user and then create an initial guess for the different parameters for this network. Then, within our for-loop (essentially just iterating the loop k-number of times) we first allow the Agent to interact with its environment using the current policy and collects these trajectories (a sequence of states, actions and rewards obtained by taking certain actions), here is also where we allow the agent to take random actions which allows for exploration. After a set amount of trajectories is collected we compute the Advantage function, which tells us how a specific action compares to our expected return. Finally, we update our policy by updating the parameters of the neural network using some type of optimization network, i.e “Adam” which is like gradient descent except it automatically changes the learning rate as necessary (I’ll save the explanation of gradient descent for another day, https://builtin.com/data-science/gradient-descent ). And then basically repeat this process until we converge or reach a maximum number of iterations.

I realize this is A LOT to digest, and believe me, I still find it hard to remember all these processes aswell, but I think its important to just have a brief idea of what is going on under the hood to better explain certain results I see and to really appreciate the whole process of reinforcement learning.


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ABOUT MY IMPLEMENTATION -->
## About My Implementation

Learning MLAgents was a very fun task since it hides a lot of the tedious work in creating the neural networks and algorithms needed. Thus, all I had to do was: Define states, define actions, define rewards, setup movement and colliders, and train the model. A lot of the considerations were taken from the original OpenAi gym implementation. 

As mentioned above I gave the model 4 possible actions: Do nothing, thrust right, thrust left, thrust up. This kept the spectrum of actions limited and helped to easier train and debug the model.

Next, I used 8 states: x-position, y-position, x-velocity, y-velocity, angle of lander with respect to horizontal plane, angular velocity and two Boolean values stating whether the left or right leg is touching our landing pad/area. 

My Rewards were setup as so: 
-	Increase/decrease the closer/further the lander is to the center of the landing pad
-	increase/decrease the slower/faster the lander is moving.
-	decrease the more the lander is tilted with respect to the ground.
-	increase by 10 points for each leg that is in contact with the ground when the episode ends.
-	decrease by 0.03 points each step a side engine is firing.
-	decrease by 0.03 points each step the main engine is firing.

Although ease of movement is not too important since a real player is not required to control the lander, I still spent some time in creating a minimal yet tough movement pattern to see how the model would adjust. I had your typical thrust main done by simply adding force to the RigidBody2D in the upward direction with respect to the rigidbodies angle. However, for thrusting left or right, I played around with adding force sideways, however, the LL (lunar lander) would not rotate. I also tried the “AddForceAtPosition” function and still saw unintended behavior. So, I ended up with adding force sideways to the rigidbody but also slightly adjusted the Torque depending on whether the LL was moving left or right. This proved to create tough movement patterns to control as a player, but I ended up seeing regardless of how much the model trained, it had no problems. 

Furthermore, as for the setup of the scene, I used a static implementation. Meaning there was no variance of the terrain for different episodes. It would not be too difficult to implement such variety, however, since the landing pad was meant to remain centered at an x-positon of 0, it gave me little room to play with the variance of the terrain and so I didn’t find it necessary to implement that at the moment, since that was not the goal of the project. 

Once a leg encounters the landing pad, we start a coroutine to commence a timer for 3 seconds to allow the LL to attempt to finalize the landing before ending the episode. I could not immediately restart the episode once one leg touched the landing pad since not in every case would the lander land in a perfect horizontal position, and often it would need to re-adjust slightly to get both legs on the platform. However, this may be an optimization point to see whether immediately ending the episode once any one or both of the LL’s legs touch the landing pad may increase the model’s desire to land in a perfectly horizontal position.



<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- OBSTACLES AND OBSERVATIONS -->
## Obstacles and Observations

There were many obstacles that I faced during my implementation. Some came from the MLAgents software being slightly outdated, but the main ones came from my oversight. To begin, straying away from the Reinforcement learning part of this project, I had a lot of trouble discovering methods to identify child colliders from parent OnTriggerEnter2D methods. The collider would often trigger, however, I could never explicitly tell which child was in contact with the 2D Collider. I needed to know this, as a “crash” occurs when the body of our LL collides with the ground or landing pad. A solution I found, without adding scripts to child objects was to simply create a “SerializableField” for my colliders and then in the Parents OnTriggerEnter2D method, use the “isTouching” method to verify the body collider was in contact with the ground collider. Similar methods were done to check the legs being in contact with the landing pad. 

Within the realm of giving appropriate rewards to the agent, I discovered a slight bug in calculating rewards depending on the speed of the LL. As one of my rewards was indicated by how fast or slow the LL was moving as each step, I intended to use the Pythagorean theorem to calculate the overall magnitude of the speed vector given the x and y velocity. Since we wanted a greater reward for going slower, I took the reciprocal of the result. Hence at each step, if the rover was essentially grounded (whether on the landing pad or not) their speed was nearly, if not zero. Thus, this would either result in an error due to dividing by zero, and if it were extremely close to zero, we’d get a nearly infinite reward value. Clearly, this disturbs the training, thus my solution was to simply create a condition to ensure that the overall speed magnitude was larger than 0.5 to only provide rewards when the LL was realistically moving.

Finally, one of the most interesting obstacles because of how unexpected it was and how a lesson can be learned from it (the lesson being that Reinforcement Learning can introduce new patterns of success that were not often considered), can be seen in the below video,



https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/1d0eaee2-a47e-414b-ad3d-a0a32b595691

Nearly all the LL’s had the tendency to move towards the edge of the landing pad. After some investigation, I noticed this was due to me giving a reward for whether the right or left leg of the LL was in contact with the landing pad. Hence, funny enough the model abused this reward by getting to the edge of the landing pad and continuously entering and exiting the landing pad collider to maximize their reward. Thus, as a solution, I could’ve minimized the reward for having each leg in contact with the landing area, OR, I just move the reward to check collision right before the episode ends.

Another Challenging task was finding the correct method to balance the rewards given to the Agent. This was most notable in rewards that had a spectrum depending on certain parameters of the LL. For example, calculating the speed magnitude of the LL had a spectrum of possible rewards, whereas checking whether the left or right leg was in contact with the landing pad had 2 possible outcomes. This was a challenge since it caused me to play around with varying the strength of the rewards that had a spectrum of possible outcomes. Sparse vs dense reward functions was the decision I had to make. I had played with the two possibilities. Giving the Agent one singular reward if it contacted the landing pad, which was our Sparse Reward OR giving the Agent multiple rewards throughout the episode, which was our dense reward.

I will briefly describe the pros and cons of the two methods I used. To begin, the dense reward function led to much easier learning as the agent was inevitably greedy as it saw which action resulted in the greatest reward, by changing its speed and angular position it could maximize its reward each step. However, this also led to undesired behavior because of the agent possibly learning a sub-optimal path



https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/bfdb2373-3eb8-4dbc-9b03-540ff1f9ade0

As for Sparse Rewards, I simply defined mine as a reward given to the agent if they reach the landing pad, regardless of how close to the center of the pad they were. This led to significantly harder learning as the agent was unsure at the beginning on how to move. Hence, I played around with using some different techniques to get the agent on the right track. I first trained it using Curriculum learning, meaning I gave it a simpler task, basically I reduced the height of its starting position and reduced the variety of the x-position where the agent might spawn. Next, I used imitation learning, essentially, manually guiding the agent using recorded steps to show them how to reach their goal. MLAgents offers this ability through the use of Behavior Cloning and Generative Adversarial Imitation Learning (GAIL) https://github.com/Unity-Technologies/ml-agents/blob/main/docs/ML-Agents-Overview.md#imitation-learning 


### Imitation Learning in Unity

In Unity, we can record our results using a script called “Demonstration Recorder” and then reference the recorded demonstrations within our .yaml file that we call once beginning training. Now the two methods Unity uses to create imitation training is Behavior Cloning and GAIL. Behavior Cloning essentially trains the Agent to mimic our demonstrations exactly, thus it is important to have sufficient demonstration data. Next GAIL, uses a 2nd neural network called the “discriminator”, all this 2nd NN does is that it gives the Agent a reward based on how close or far the agent was from the pre-recorded demonstrations. However, as mentioned in the documentation, GAIL falls victim to “survivor bias” which is caused because the agent wants to keep obtaining rewards for mimicking the pre-recorded demonstrations and so, this causes the agent to try and stay “alive”/not-end-the-episode for as long as possible to rack up the rewards. Hence, GAIL is not often used on its own, but rather in conjunction with Behavior Cloning or regular RL .

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- RESULTS -->
## Results

To begin I will talk about the results of the Dense Reward function training, as seen in the video above, after training the dense reward we had overall good behavior from the agent, but the odd thing was that it preformed much better when spawned on the left side of the platform versus the right. Thus, I continued the training and had it only spawn on the right side of the platform to observe if this would improve its overall performance. We see the final performance below,

https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/8e6ad184-a4c6-4ea4-9070-86dce91d868a

We see this extra training DID NOT in fact have a positive impact on the LL pathfinding. Instead, the issue persisted. My hypothesis is that the agent is unable to relearn a more optimal approach after the millions of steps they have already taken and may require re-training from scratch.

Now, lets look at the logistics of our results,

![image](https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/ccd08168-034e-4f78-9bf6-214b7c3ffc3f)

We can see that we had already obtained nearly our maximum reward after 2M steps. However, even after 2M steps, the LL would still behave improperly and hit the side of the landing pad instead of properly landing on top of the pad. Hence the extra 3M steps to refine this behavior was necessary (even though the issue persisted when the LL spawned to the right of the landing pad).

Now moving on to our Spare Reward results. Firstly, I began by running our imitation learning .yaml file with a curriculum learning method for about 2M steps. The curriculum learning method I used was to spawn the LL closer to the landing area. Here were the results,

<img width="413" alt="BC_GAIL_Training_Ver_2_enlarged" src="https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/eb204f5d-526d-474a-95c0-289881425399">


As we can see, although we were increasing our cumulative reward with more steps, we seemed to get worse after reaching our maximum. This was expected as we only give a reward to the Agent for landing on the pad, so there is a large chance for it to begin “screwing up” and especially since we were only using behavior cloning and GAIL, the only learning was coming from my pre-recorded demonstrations, and as my demonstrations were not perfect, a swing of cumulative rewards is highly possible. Then I let it train simply using GAIL (with less influence/strength) and our extrinsic reward system. The results seemed to follow a similar pattern as seen in the Dense Reward function learning.

Then after reintroducing the reward functions within the training to complete the overall training we received the following results,

<img width="417" alt="Final_training_results_Ver_2_enlarged" src="https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/68ae13e0-691c-4e7a-b168-c03851f2c21a">


Clearly we see the Agent began to converge to consistently obtaining nearly the maximum reward quite quickly. Personally, I don’t believe this would have occurred as quickly without using ciricuulum learning and imitation learning as the agent would have had to explore for significantly longer. Furthermore, without the recorded demonstrations and the strict behavioral rewards that we use in Dense Reward functions we would have seen much more odd behavior from the LL. For example, taking in the video of our agent after the completed training,



https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/d6069b0c-e74f-4218-895b-6d3ed1198a05

We can right away see that it does not value quickly getting to the landing pad nor maintaining a horizontal level with the ground as it approaches the landing pad. Moreover, it is also unconcerned with how close we are to the center of the landing pad. 

Finally, as a fun experiment, after adjusting a couple lines in the code and letting the Agent train for 24 hours time using dense reward functions, I obtained the following final training result,



https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/66743d28-94d3-41ac-83e8-8d5d8dc309b0

With these metrics,

<img width="667" alt="24_hour_training" src="https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/095b6fb5-bd8b-4985-b8ed-ed158528098f">


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONCLUSION -->
## Conclusion

After many iterations of training the Agents by manipulating certain hyperparameters, the overall yaml file, exploring Sparse vs Dense Reward functions, I’d conclude that a dense reward function with some level of imitation learning is the best for optimal results. There are also certain hyperparameters to be played with, such as how many hidden layers and hidden units we include, but I don’t think they have as significant of an impact as how we decide to balance our reward system for the Agent. 

Now in terms of this project. of course it will never be completely finished as there are sooooo many elements to explore, but from what started out as an idea to reimplement a simple environment and turned into a full multi-week research endeavor I enjoyed the entire process. Not only will I continue to explore different elements of training the model but maybe even expand into a 3D space to see how my results are altered by including a third dimension. 


<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->
## Contact

Paul Kokhanov - [Website](https://paulkokhanov.com/) - paul.kokhanov@gmail.com

Project Link: [https://github.com/PaulKokhanov1/LunarLanderRL](https://github.com/PaulKokhanov1/LunarLanderRL)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [Unity MLAgents](https://github.com/Unity-Technologies/ml-agents)
* [Gymnasium](https://gymnasium.farama.org/)
* [ML Specialization taught by Andrew Ng](https://www.coursera.org/specializations/machine-learning-introduction)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo_name/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo_name/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo_name/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo_name/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/paulkokhanov
[product-screenshot]: https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/6ed735d2-09d7-42f3-bfe3-01473ff121c9
[product-screenshot-original]:https://github.com/PaulKokhanov1/LunarLanderRL/assets/69466838/efdba43a-cc5b-4a62-b447-c9cd2fac6ba2

[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[CSharp.com]: https://img.shields.io/badge/C%23-%23512BD4?style=for-the-badge&logo=csharp&logoColor=white
[CSharp-url]: https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit
[Unity.com]: https://img.shields.io/badge/unity-0769AD?style=for-the-badge&logo=unity&logoColor=white
[Unity-url]: https://unity.com/
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com 
