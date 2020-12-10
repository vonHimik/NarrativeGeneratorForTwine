# NarrativeGeneratorForTwine

What is there:

1) Wrapper

- Launches FastDownward, passes it launch parameters, need to specify the path to the PDDL files with the domain and the problem.

2) A class for interacting with FastDownward.

- Uses a wrapper, can read the plan at the output (from a text file) and add actions to the plan.

3) Plan

- A class that implements actions in an array, managing their addition and retrieval.

4) Action

- A class that implements such a thing as an action. It is subdivided into several types (to determine whether it can be used by different types of agents), requires CSP of variables / parameters (implemented as method parameters), execution of preconditions (specially specified Boolean conditions) and, as a result, produces an effect (implemented as a change in the state of the World)... All types are reference types to interact and modify directly, rather than working with values ​​within a class.

5) World

- A class that implements the state of the world / system. In various cases, it can be used as a display of a real situation, or as a representation of the goal state, or as a representation of agents' beliefs about the world around them. Contains lists of locations, agents, goal states, and all actions available in the game.

6) Location

- Representation of a certain area of ​​space in which agents are located. Contains a list of agents located in it, pointers to related locations, allows searching for agents within itself.

7) Agent

- A class that implements and manages agents. Has a plan, a list of available actions, name, role, status (alive / dead), goals, beliefs and initiative. Based on the plan from his list of available actions, he chooses an action to perform (the plan is made in advance, each move, using the planner), and sends it to the Storyworld Convergence.

8) Storyworld Convergence

- A class that implements a managing agent, which requests actions from the others in accordance with their initiative, checks for compliance with the constraints, and applies or interrupts with its action.

9) Constraint

- A class that implements constraints on the actions of agents. Implements different types of constraints, checks with boolean checks that certain conditions are met when an action is applied.

10) Storytelling algorithm

- Determines the order of work of other classes, their interaction, construction of a graph of data and determines the initial state of the world. More precisely, constructs it.

11) Node of the graph (StoryNode).

- A class that implements the story-graph node. Stores the state of the world, and stores information about whose turn it is at the moment.

The rest of the things are still in a somewhat rudimentary state.