using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Narrative_Generator
{
    class StoryAlgorithm
    {
        //////////////////////////////////
        /* INTERNAL SYSTEMS COMPONENTS */
        /////////////////////////////////
        /// <summary>
        /// An object of the class that manages the generation of the planning domain and the planning problem.
        /// </summary>
        public PDDL_Module pddlModule = new PDDL_Module();
        /// <summary>
        /// An object of the class that manages the assignment and control of the fulfillment of the goals of agents.
        /// </summary>
        public GoalManager goalManager = new GoalManager();
        /// <summary>
        /// An object of the class that manages the creation and control of constraints.
        /// </summary>
        public ConstraintManager constraintManager = new ConstraintManager();
        /// <summary>
        /// An object of the class that manages the agent action process.
        /// </summary>
        public StoryworldConvergence storyworldConvergence = new StoryworldConvergence();
        /// <summary>
        /// An object of the class that manages the description of the story graph in the dot format.
        /// </summary>
        public GraphСonstructor graphСonstructor = new GraphСonstructor();
        /// <summary>
        /// An object of the class that manages the description of the story graph in a format accepted by the Twine platform.
        /// </summary>
        public TwineGraphConstructor twineGraphConstructor = new TwineGraphConstructor();

        /// <summary>
        /// Story graph, which is a collection of nodes connected by oriented edges.
        /// </summary>
        public StoryGraph newStoryGraph = new StoryGraph();

        ///////////////
        /* SETTINGS */
        //////////////
        /// <summary>
        /// The type of setting in which the story is created.
        /// </summary>
        public Setting setting;
        /// <summary>
        /// Active settings marker: whether the connection between locations will be used explicitly, 
        ///    or from any one it will be possible to move to any location.
        /// </summary>
        public bool RandomConnectionOfLocations { get; set; }
        /// <summary>
        /// Active settings marker: whether random events occur during the story generation.
        /// </summary>
        public bool RandomEncounters { get; set; }
        /// <summary>
        /// Active settings marker: whether the result of aggressive actions of agents (leading to a change status of agents) will be determined randomly.
        /// </summary>
        public bool RandomBattlesResults { get; set; }
        /// <summary>
        /// Active settings marker: whether the order of agent activity will be chosen randomly.
        /// </summary>
        public bool RandomDistributionOfInitiative { get; set; }
        /// <summary>
        /// Active settings marker: whether to hide "empty" actions.
        /// </summary>
        public bool HideNothingToDoActions { get; set; }
        /// <summary>
        /// Settings: seed for random generation.
        /// </summary>
        public int Seed { get; set; }
        /// <summary>
        /// Settings: limit on the maximum number of nodes in a graph.
        /// </summary>
        public int MaxNodes { get; set; }
        /// <summary>
        /// Settings: duration of protection of the protagonist from status change.
        /// </summary>
        public int ProtagonistProtectionTime { get; set; }
        /// <summary>
        /// Settings: duration of protection of the antagonist from status change.
        /// </summary>
        public int AntagonistProtectionTime { get; set; }
        /// <summary>
        /// Settings: the chance of success in the search for evidence action.
        /// </summary>
        public int EvidenceFindChance { get; set; }
        /// <summary>
        /// Active characters behaviours settings marker: whether agents can find evidences in a detective setting
        /// </summary>
        public bool CanFindEvidence { get; set; }
        /// <summary>
        /// Active characters behaviours settings marker: whether the protagonist will be protected by the system from actions that change the agent's status.
        /// </summary>
        public bool ProtagonistWillSurvive { get; set; }
        /// <summary>
        /// Active characters behaviours settings marker: whether the antagonist will be protected by the system from actions that change the agent's status.
        /// </summary>
        public bool AntagonistWillSurvive { get; set; }
        /// <summary>
        /// Active characters behaviours settings marker: each antagonist's "kill" action has a unique description depending on the conditions in which it was performed.
        /// </summary>
        public bool UniqWaysToKill { get; set; }
        /// <summary>
        /// Active characters behaviours settings marker: the antagonist will complete his goals in a strictly defined order.
        /// </summary>
        public bool StrOrdVicSec { get; set; }
        /// <summary>
        /// Active characters behaviours settings marker: each agent has unique goals, chosen randomly.
        /// </summary>
        public bool EachAgentsHasUG { get; set; }

        /////////////////////////////////////
        /* CHARACTERS BEHAVIOURS SETTINGS */
        ////////////////////////////////////
        /// <summary>
        /// Marker to activation of the agent's behavior setting: the antagonist is able to initiate conversations with other agents.
        /// </summary>
        public bool talkativeAntagonist;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: enemies are able to initiate conversations with other agents.
        /// </summary>
        public bool talkativeEnemies;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: the antagonist know how to lure victims.
        /// </summary>
        public bool cunningAntagonist;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: enemies know how to lure victims.
        /// </summary>
        public bool cunningEnemies;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: the antagonist does not use aggressive actions (which can change the agent's status.
        /// </summary>
        public bool peacefulAntagonist;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: enemies do not use aggressive actions (which can change the agent's status).
        /// </summary>
        public bool peacefulEnemies;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: the protagonist cannot initiate communication with other agents.
        /// </summary>
        public bool silentProtagonist;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: ordinary agents cannot initiate communication with other agents.
        /// </summary>
        public bool silentCharacters;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: the protagonist can use aggressive actions (leading to a change in the agent's status).
        /// </summary>
        public bool aggresiveProtagonist;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: ordinary agents can use aggressive actions (leading to a change in the agent's status).
        /// </summary>
        public bool aggresiveCharacters;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: the protagonist becomes cowardly and can use the "run" action.
        /// </summary>
        public bool cowardlyProtagonist;
        /// <summary>
        /// Marker to activation of the agent's behavior setting: ordinary agents become cowardly and can use the "run" action.
        /// </summary>
        public bool cowardlyCharacters;

        ///////////////////
        /* SYSTEM STATE */
        //////////////////
        /// <summary>
        /// The current state of the story world.
        /// </summary>
        public WorldDynamic currentStoryState = new WorldDynamic();
        /// <summary>
        /// An indicator of whether at least one goal condition has been reached.
        /// </summary>
        public bool reachedGoalState = false;
        /// <summary>
        /// The number of agents active in this story instance.
        /// </summary>
        public int AgentsCounter { get; set; }
        /// <summary>
        /// The number of locations available in this story instance.
        /// </summary>
        public int LocationsCounter { get; set; }

        /// <summary>
        /// A method that reads user-selected settings and transfers them to individual system modules.
        /// </summary>
        /// <param name="note">Text to display on the main screen.</param>
        public void ReadUserSettingsInput (ref TextBox note)
        {
            note.Text = "SETTINGS READING";

            // We read the setting and save it in the starting state of the world.
            currentStoryState.GetStaticWorldPart().SetSetting(setting);

            // If random connections of locations are defined, or a generic fantasy setting is chosen, 
            //   then we write in the start state information that the locations are interconnected.
            if (RandomConnectionOfLocations || setting.Equals(Setting.GenericFantasy) || setting.Equals(Setting.Detective) 
                || (setting.Equals(Setting.DragonAge) && RandomEncounters))
            { currentStoryState.GetStaticWorldPart().ConnectionOn(); }

            // If a random result of battles is determined, then we write information about this in the starting state.
            if (RandomBattlesResults) { currentStoryState.GetStaticWorldPart().RandomBattlesResultsOn(); }

            // If it is determined that it is required to hide actions of the "do nothing" type, then we inform the graph construct about this.
            if (HideNothingToDoActions) { graphСonstructor.HideNTDActions(); }

            if (UniqWaysToKill) { currentStoryState.GetStaticWorldPart().UniqWaysToKillOn(); }
            if (StrOrdVicSec) { currentStoryState.GetStaticWorldPart().StrictOrderOfVictimSelectionOn(); }
            if (EachAgentsHasUG) { currentStoryState.GetStaticWorldPart().AgentsHasUniqGoalsOn(); }
            if (CanFindEvidence) { currentStoryState.GetStaticWorldPart().CanFindEvidenceOn(); }

            // We initialize the module for constructing pddl files with the necessary information.
            pddlModule.Initialization(setting, currentStoryState.GetStaticWorldPart().GetConnectionStatus(), AgentsCounter, CanFindEvidence);
            pddlModule.charactersBehaviourInitialization(talkativeAntagonist, talkativeEnemies, cunningAntagonist, cunningEnemies, peacefulAntagonist, peacefulEnemies,
                                                         silentProtagonist, silentCharacters, aggresiveProtagonist, aggresiveCharacters, cowardlyProtagonist,
                                                         cowardlyCharacters);

            // Initialize the Goal Manager.
            goalManager.Initialization();
        }

        /// <summary>
        /// A method that creates an initial state of the world based on user preferences.
        /// </summary>
        /// <param name="note">Text to display on the main screen.</param>
        public void CreateInitialState (ref TextBox note)
        {
            // === LOCATIONS CREATING ===
            note.Text = "LOCATIONS CREATING";

            // We create locations, determine their names and the presence of evidence in them.
            List<string> locationNames = CreateLocationsNamesList(LocationsCounter);
            List<bool> locationsEvidences = CreateLocationsEvidencesList(LocationsCounter);
            Dictionary<LocationStatic, LocationDynamic> locations = CreateLocationSet(locationNames, locationsEvidences);
            
            // The first step in creating the initial state is setting up the environment, that is - locations.
            CreateEnviroment(locations);

            // === AGENTS CREATING ===
            note.Text = "AGENTS CREATING";

            // We create sets of attributes for agents: names, states, roles, goals, and beliefs.
            List<string> names = CreateAgentsNamesList(AgentsCounter);
            List<bool> statuses = CreateStatusesList(AgentsCounter);
            List<AgentRole> roles = CreateAgentsRolesList(AgentsCounter);
            List<Goal> goals = goalManager.CreateGoalSet(roles);
            List<WorldContext> beliefs = CreateBeliefsSet(AgentsCounter);

            // The second step in creating the initial state is the creation of agents, initially with empty goals and beliefs, 
            //    since they are highly dependent on the agents themselves existing in the "world". We'll finish setting this up in the next step.
            CreateAgents(names, statuses, roles, goals, beliefs, GetRandomLocationName(locationNames), AgentsCounter, locationNames);

            // We randomly assign an initiative value to the agents to determine the order of their turn, and sort the agents on the initiative.
            DistributionOfInitiative();
            currentStoryState.OrderAgentsByInitiative();

            // The third step in creating an initial state is assigning to agents their goals and beliefs.

            // === Goals === //
            goalManager.AssignGoalsToAgents(ref currentStoryState);

            // === Beliefs === //

            // We go through all the agents in the world.
            foreach (var agent in currentStoryState.GetAgents())
            {
                // For each separately.
                foreach (var anotherAgent in currentStoryState.GetAgents())
                {
                    // If the agent meets himself.
                    if (agent.Equals(anotherAgent))
                    {
                        continue;
                    }
                    else
                    {
                        switch (setting)
                        {
                            case Setting.DragonAge:
                                agent.Value.GetBeliefs().AddAgentInBeliefs(anotherAgent, anotherAgent.Key.GetRole());
                                break;
                            case Setting.GenericFantasy:
                                agent.Value.GetBeliefs().AddAgentInBeliefs(anotherAgent, anotherAgent.Key.GetRole());
                                break;
                            case Setting.Detective:
                                // Otherwise, copies the name of the selected agent and by default does not consider him a killer.
                                agent.Value.GetBeliefs().AddAgentInBeliefs(anotherAgent, AgentRole.USUAL);
                                break;
                            case Setting.DefaultDemo:
                                // Otherwise, copies the name of the selected agent and by default does not consider him a killer.
                                agent.Value.GetBeliefs().AddAgentInBeliefs(anotherAgent, AgentRole.USUAL);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// A method that creates a list of names for locations.
        /// </summary>
        /// <param name="locationsCounter">The number of locations that should be present in the world.</param>
        /// <returns>List of names for locations of the specified size.</returns>
        public List<string> CreateLocationsNamesList (int locationsCounter)
        {
            // We create an empty list.
            List<string> namesList = new List<string>();

            // We prepare a list with specified names.
            List<string> specifiedNamesList = new List<string>();

            switch (setting)
            {
                case Setting.DragonAge:
                    if (RandomEncounters)
                    {
                        specifiedNamesList = new List<string>
                        {
                            "Lothering", "MagesTower", "Orzammar", "BrecilianForest", "Denerim", "DeepRoads", "Road", "Crossroad", "Riverside"
                        };
                    }
                    else
                    {
                        specifiedNamesList = new List<string>
                        {
                            "Lothering", "MagesTower", "Orzammar", "BrecilianForest", "Denerim", "DeepRoads"
                        };
                    }
                    break;
                case Setting.GenericFantasy:
                    specifiedNamesList = new List<string>
                    {
                       "Village", "Town", "AncientRuins", "MysteriousForest", "Dungeon", "MountainPass"
                    };
                    break;
                case Setting.Detective:
                    specifiedNamesList = new List<string>
                    {
                        /*"LivingRoom", "Rock", "Beach", "PlayerRoom", "WargraveRoom", "MacArthurRoom", "BrentRoom", "RogersRoom",
                        "ArmstrongRoom", "BloreRoom", "LombardRoom", "ClaytonRoom", "MarstonRoom"*/
                        "LivingRooms", "Hall", "Rock", "Beach"
                    };
                    break;
                case Setting.DefaultDemo:
                    specifiedNamesList = new List<string>
                    {
                        "kitchen", "dining-room", "hall", "garden", "bedroom", "guest-bedroom", "bathroom", "attic"
                    };
                    break;
            }

            // We go through the empty list, assigning an entry from the list with prepared names to the corresponding cell.
            // Until we reach the specified cell (there may be more names than necessary).
            for (int i = 0; i < locationsCounter; i++)
            {
                namesList.Add(specifiedNamesList[i]);
            }

            // We return a list with the names of locations.
            return namesList;
        }

        /// <summary>
        /// A method that determines in which of the locations the evidence will be located.
        /// </summary>
        /// <param name="evidenceCount">The number of evidences to be found.</param>
        /// <returns>List of booleans variables corresponding to locations with corresponding indexes.</returns>
        public List<bool> CreateLocationsEvidencesList (int evidenceCount)
        {
            // We create a list that stores information about the presence of evidence in locations.
            List<bool> locationsEvidences = new List<bool>();

            for (int i = 0; i < evidenceCount; i++) { locationsEvidences.Add(false); }

            if (!CanFindEvidence) { return locationsEvidences; }

            // We go through the list, adding information about the presence of evidence. At the moment - there is no evidence !!!
            // Until we reach the specified cell (there may be more names than necessary).
            for (int i = 0; i < evidenceCount; i++)
            {
                locationsEvidences.Add(false);
            }

            // We return a list of evidence in locations.
            return locationsEvidences;
        }

        /// <summary>
        /// A method that creates a list defining the states of agents.
        /// </summary>
        /// <param name="agentsCounter">The number of agents that will be represented in the world.</param>
        /// <returns>List of booleans variables corresponding to agents with corresponding indexes.</returns>
        public List<bool> CreateStatusesList (int agentsCounter)
        {
            // We create an empty list.
            List<bool> statuses = new List<bool>();

            // We go through the list, assigning information about the state of agents. At the moment - everyone is alive !!!
            // Until we reach the specified cell (there may be more names than necessary).
            for (int i = 0; i < agentsCounter; i++)
            {
                statuses.Add(true);
            }

            // Returning a list with states.
            return statuses;
        }

        /// <summary>
        ///  A method that creates a list of names for agents.
        /// </summary>
        /// <param name="agentsCounter">The number of agents that will be represented in the world.</param>
        /// <returns>List of names for agents of the specified size.</returns>
        public List<string> CreateAgentsNamesList (int agentsCounter)
        {
            // We create an empty list.
            List<string> namesList = new List<string>();

            // Create a list with specified names.
            List<string> specifiedNamesList = new List<string>();

            switch (setting)
            {
                case Setting.DragonAge:
                    if (RandomEncounters)
                    {
                        specifiedNamesList = new List<string>()
                        {
                            "GreyWarden", "Archdemon", "Darkspawns", "Ghost"
                        };
                    }
                    else
                    {
                        specifiedNamesList = new List<string>()
                        {
                            "GreyWarden", "Archdemon"
                        };
                    }
                    break;
                case Setting.GenericFantasy:
                    specifiedNamesList = new List<string>()
                    {
                         "Hero", "Undead", "Rogue", "Goblins", "Barbarians", "InsidiousVillain"
                    };
                    break;
                case Setting.Detective:
                    specifiedNamesList = new List<string>()
                    {
                        "Player", "LawrenceJohnWargrave", "JohnGordonMacArthur", "ThomasRogers", "EmilyCarolineBrent",
                        "EthelRogers", "EdwardGeorgeArmstrong", "WilliamHenryBlore", "PhilipLombard", "VeraElizabethClaythorne",
                        "AnthonyJamesMarston"
                    };
                    break;
                case Setting.DefaultDemo:
                    specifiedNamesList = new List<string>()
                    {
                        "Player", "Judge", "Journalist", "Mafia-boss", "Politician", "Rich", "Clerk"
                    };
                    break;
            }

            // We go through the empty list, adding names from the list with names to the corresponding cells.
            // Until we reach the specified cell (there may be more names than necessary).
            for (int i = 0; i < agentsCounter; i++)
            {
                namesList.Add(specifiedNamesList[i]);
            }

            // We return a list with the names of the agents.
            return namesList;
        }

        /// <summary>
        /// A method that returns a randomly selected location name from a list of locations.
        /// </summary>
        /// <param name="namesList">List of names of locations present in the world.</param>
        /// <returns>Location name.</returns>
        public string GetRandomLocationName (List<string> namesList)
        {
            // We get a random number, from 0 to the number of elements in the list.
            Random random = new Random(Seed);
            int rand = random.Next(namesList.Count);

            // The resulting random number is used as the index of the element in the list of location names, which we will return.
            return namesList[rand];
        }

        /// <summary>
        /// A method that creates a list of roles for agents.
        /// </summary>
        /// <param name="agentsCounter">The number of agents that will be represented in the world.</param>
        /// <returns>List of agent's roles corresponding to agents with corresponding indexes.</returns>
        public List<AgentRole> CreateAgentsRolesList (int agentsCounter)
        {
            // We create an empty list.
            List<AgentRole> rolesList = new List<AgentRole>();

            // Create a list with roles.
            List<AgentRole> specifiedRolesList = new List<AgentRole>();

            switch (setting)
            {
                case Setting.DragonAge:
                    if (RandomEncounters)
                    {
                        specifiedRolesList = new List<AgentRole>()
                        {
                            AgentRole.PLAYER, AgentRole.ANTAGONIST, AgentRole.ENEMY, AgentRole.ENEMY
                        };
                    }
                    else
                    {
                        specifiedRolesList = new List<AgentRole>()
                        {
                            AgentRole.PLAYER, AgentRole.ANTAGONIST
                        };
                    }
                    break;
                case Setting.GenericFantasy:
                    specifiedRolesList = new List<AgentRole>()
                    {
                        AgentRole.PLAYER, AgentRole.ENEMY, AgentRole.ENEMY, AgentRole.ENEMY, AgentRole.ENEMY, AgentRole.ANTAGONIST
                    };
                    break;
                case Setting.Detective:
                    specifiedRolesList = new List<AgentRole>()
                    {
                        AgentRole.PLAYER, AgentRole.ANTAGONIST, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL,
                        AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL
                    };
                    break;
                case Setting.DefaultDemo:
                    specifiedRolesList = new List<AgentRole>()
                    {
                        AgentRole.PLAYER, AgentRole.ANTAGONIST, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL
                    };
                    break;
            }

            // We go through the empty list, assigning values from the list with roles to the corresponding cells.
            // Until we reach the specified cell (there may be more roles than necessary).
            for (int i = 0; i < agentsCounter; i++)
            {
                rolesList.Add(specifiedRolesList[i]);
            }

            // Returning a list with agent roles.
            return rolesList;
        }

        /// <summary>
        /// We get info about agents from user input. From it we find out how many agents there are, what roles they have, their beliefs, 
        ///    and we will have to design them and add them to the game world.
        /// </summary>
        /// <param name="names">List of agent names.</param>
        /// <param name="statuses">List of initial states of agents.</param>
        /// <param name="roles">List of agent roles.</param>
        /// <param name="goals">List of agents goals.</param>
        /// <param name="beliefs">List of agents beliefs.</param>
        /// <param name="spawnLocationName">List of locations where agents spawn.</param>
        /// <param name="numbers">The number of agents that will be represented in the world.</param>
        /// <param name="namesList">List of location names.</param>
        public void CreateAgents (List<string> names, List<bool> statuses, List<AgentRole> roles, List<Goal> goals, 
                                  List<WorldContext> beliefs, string spawnLocationName, int numbers, List<string> namesList)
        {
            for (int i = 0; i < numbers; i++)
            {
                switch (setting)
                {
                    case Setting.DragonAge:
                        if (RandomEncounters)
                        {
                            switch (names[i])
                            {
                                case "Archdemon":
                                    CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "DeepRoads");
                                    break;
                                case "Darkspawns":
                                    CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "Road");
                                    break;
                                case "Ghost":
                                    CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "Riverside");
                                    break;
                                case "GreyWarden":
                                    CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "Lothering");
                                    break;
                            }
                        }
                        else
                        {
                            switch (roles[i])
                            {
                                case AgentRole.ANTAGONIST:
                                    CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "DeepRoads");
                                    break;
                                case AgentRole.PLAYER:
                                    CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "Lothering");
                                    break;
                            }
                        }
                        break;
                    case Setting.GenericFantasy:
                        switch (names[i])
                        {
                            case "Hero":
                                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "Village");
                                break;
                            case "Undead":
                                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "AncientRuins");
                                break;
                            case "Rogue":
                                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "Dungeon");
                                break;
                            case "Goblins":
                                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "MysteriousForest");
                                break;
                            case "Barbarians":
                                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "MountainPass");
                                break;
                            case "InsidiousVillain":
                                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], "Town");
                                break;
                        }
                        break;
                    case Setting.Detective:
                        if (i < namesList.Count)
                        {
                            CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], /*"LivingRoom"*/ namesList[i]);
                        }
                        else
                        {
                            CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], /*"LivingRoom"*/ namesList[namesList.Count - 1]);
                        }
                        //CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], /*"LivingRoom"*/ namesList[i]);
                        break;
                    case Setting.DefaultDemo:
                        CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], /*namesList[i]*/ spawnLocationName);
                        break;
                }
            }
        }

        /// <summary>
        /// This method creates a separate agent using the information passed to it. 
        /// Then it places the agent on the environment and passes information about it to it.
        /// </summary>
        /// <param name="name">Agent name.</param>
        /// <param name="status">The initial state of the agent.</param>
        /// <param name="role">Agent role.</param>
        /// <param name="goals">Agent goals.</param>
        /// <param name="beliefs">Agent beliefs.</param>
        /// <param name="spawnLocationName">Agent's spawn location.</param>
        public void CreateAgent (string name, bool status, AgentRole role, Goal goals, WorldContext beliefs, string spawnLocationName)
        {
            // We clone locations from the world.
            Dictionary<LocationStatic, LocationDynamic> locations = currentStoryState.CloneLocations();

            // We construct a new agent, from static and dynamic parts.
            AgentStateStatic newAgentStateStatic = new AgentStateStatic(name, role);
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic(status, goals, beliefs, newAgentStateStatic);
            KeyValuePair<AgentStateStatic, AgentStateDynamic> newAgent = 
               new KeyValuePair<AgentStateStatic, AgentStateDynamic>(newAgentStateStatic, newAgentStateDynamic);

            // Add the agent to the list of agents.
            currentStoryState.AddAgent(newAgent, currentStoryState.GetLocationByName(spawnLocationName));

            // We transfer information about the locations in the world to the agent.
            newAgent.Value.GetBeliefs().SetLocationsInWorld(locations);

            // We inform the agent in which location it was created.
            newAgent.Value.GetBeliefs().SetMyLocation(newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName));
            newAgent.Value.GetBeliefs().AddAgentInBeliefs(newAgent, newAgent.Key.GetRole());
            newAgent.Value.GetBeliefs().GetAgentByName(newAgent.Key.GetName()).
                SetLocation(newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName));
        }

        /// <summary>
        /// A method that adds locations to the current state of the world.
        /// </summary>
        /// <param name="locations">List of locations in the world.</param>
        public void CreateEnviroment (Dictionary<LocationStatic, LocationDynamic> locations)
        {
            // Add locations to the dynamic component of the current state of the world.
            currentStoryState.AddLocations(locations);

            // Create an empty list of static components of locations.
            List<LocationStatic> locationsList = new List<LocationStatic>();

            // We go through the list of locations, adding their static components to an empty list.
            foreach (var location in locations)
            {
                locationsList.Add(location.Key);
            }

            // Assign the list of static components of locations to the static component to the current state of the world.
            currentStoryState.GetStaticWorldPart().AddLocations(locationsList);
        }

        /// <summary>
        /// A method that determines of agents' initiative. The higher the score, the lower the agent will be in the list.
        /// </summary>
        public void DistributionOfInitiative()
        {
            if (RandomDistributionOfInitiative)
            {
                // Create a list of random values (in the range from 0 to 99), equal to the number of agents.
                List<int> valuesOfInitiative = NumberGenerator(currentStoryState.GetNumberOfAgents());

                // We go through the list of agents in the current state.
                foreach (var agent in currentStoryState.GetAgents())
                {
                    // Assign a random number to the agent from the list.
                    agent.Value.SetInitiative(RandomSelectionFromTheList(ref valuesOfInitiative));

                    // If the agent has the role of a player, then it is guaranteed to have an initiative of 100.
                    if (agent.Key.GetRole() == AgentRole.PLAYER) { agent.Value.SetInitiative(100); }
                }
            }
            else
            {
                for (int i = 0; i < currentStoryState.GetNumberOfAgents(); i++)
                {
                    foreach (var agent in currentStoryState.GetAgents())
                    {
                        agent.Value.SetInitiative(i);

                        // If the agent has the role of a player, then it is guaranteed to have an initiative of 100.
                        if (agent.Key.GetRole() == AgentRole.PLAYER) { agent.Value.SetInitiative(100); }
                    }
                }
            }
        }

        /// <summary>
        /// A method that implements a random number generator, in the range 0 through 99, 
        ///     that returns a set of numbers no larger than the specified size.
        /// </summary>
        /// <param name="maximum">Limit on the size of the generated set.</param>
        /// <returns>A set of random numbers.</returns>
        public List<int> NumberGenerator (int maximum)
        {
            // We will instantiate the random number generator.
            Random random = new Random(Seed);

            // Create an empty list of numbers.
            List<int> result = new List<int>();

            // We start a loop that will continue until it returns the specified number of values to us.
            while (result.Count != maximum)
            {
                // We get a random number in the range from 0 to 99.
                int temp = random.Next(0, 99);

                // If it is already in the list, then continue without doing anything.
                if (result.Contains(temp))
                {
                    continue;
                }
                // Otherwise, add it to the list.
                else
                {
                    result.Add(temp);
                }
            }

            // We return a list with a set of random numbers.
            return result;
        }

        /// <summary>
        /// A method that returns a randomly selected number from a specified list of numbers and removes it from the list.
        /// </summary>
        /// <param name="list">A set of numbers.</param>
        /// <returns>A number randomly selected from a set.</returns>
        public int RandomSelectionFromTheList (ref List<int> list)
        {
            // We will instantiate the random number generator.
            Random random = new Random(Seed);

            // We get the index using a random number generator, in the range from 0 to the number of elements in the list.
            int index = random.Next(0, list.Count());

            // We read and store from the list the value of the cell with the previously obtained index.
            int result = list[index];

            // We remove this cell from the list.
            list.RemoveAt(index);

            return result;
        }

        /// <summary>
        /// A method that defines the constraints imposed on the story.
        /// </summary>
        /// <param name="note">Text to display on the main screen.</param>
        public void CreateConstraints(ref TextBox note)
        {
            note.Text = "CONSTRAINTS CREATING";

            switch (setting)
            {
                case Setting.DragonAge:
                    HashSet<AgentStateStatic> restrictedAgents = new HashSet<AgentStateStatic>()
                    {
                        currentStoryState.GetAgentByName("GreyWarden").Key, currentStoryState.GetAgentByName("Archdemon").Key
                    };

                    HashSet<AgentStateStatic> playerAgent = new HashSet<AgentStateStatic>() { currentStoryState.GetAgentByName("GreyWarden").Key };

                    HashSet<LocationStatic> restrictedLocations = new HashSet<LocationStatic>()
                    {
                        currentStoryState.GetLocationByName("Denerim").Key, currentStoryState.GetLocationByName("DeepRoads").Key
                    };

                    HashSet<LocationStatic> bannedLocations = new HashSet<LocationStatic>()
                    {
                        currentStoryState.GetLocationByName("DeepRoads").Key
                    };

                    HashSet<LocationStatic> questLocations = new HashSet<LocationStatic>()
                    {
                        currentStoryState.GetLocationByName("MagesTower").Key, currentStoryState.GetLocationByName("Orzammar").Key,
                        currentStoryState.GetLocationByName("BrecilianForest").Key
                    };

                    HashSet<LocationStatic> revisitBanLocationsForPlayer = new HashSet<LocationStatic>()
                    {
                        currentStoryState.GetLocationByName("Lothering").Key, currentStoryState.GetLocationByName("MagesTower").Key,
                        currentStoryState.GetLocationByName("Orzammar").Key, currentStoryState.GetLocationByName("BrecilianForest").Key
                    };

                    constraintManager.CreateRestrictingLocationConstraint(false, false, true, false, false, false, false, false, false, false, false,
                                                                             restrictedLocations, restrictedAgents, 0); // archdemonWaitsUntilPlayerComesToDenerim

                    constraintManager.CreateRestrictingLocationConstraint(false, false, false, true, false, false, false, false, false, false, false,
                                                                             null, playerAgent, 0); // revisitBanForPlayer

                    constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, true, false, false, false, false, false, false,
                                                                             questLocations, playerAgent, 0); // playerMustCompleteQuestBeforeMoving

                    constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, false, true, false, false, false, false, false,
                                                                             bannedLocations, restrictedAgents, 0); // playerCantMoveToDeepRoads

                    constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, false, false, true, false, false, false, false,
                                                                             restrictedLocations, playerAgent, 3); // questsLimitFromPlayerBeforeMoveToDenerim


                    constraintManager.CreateActionRestrictingConstraint(true, true, false, new HelpMages(),
                                                                           new List<PlanAction> { new HelpTemplars() }, 0); // helpMagesVsHelpTemplar

                    constraintManager.CreateActionRestrictingConstraint(true, true, false, new HelpTemplars(),
                                                                           new List<PlanAction> { new HelpMages() }, 0); // helpTemplarsVsHelpMages

                    constraintManager.CreateActionRestrictingConstraint(true, true, false, new HelpElfs(),
                                                                           new List<PlanAction> { new HelpWerewolves() }, 0); // helpElfsVsHelpWerewolves

                    constraintManager.CreateActionRestrictingConstraint(true, true, false, new HelpWerewolves(),
                                                                           new List<PlanAction> { new HelpElfs() }, 0); // helpWerewolvesVsHelpElfs

                    constraintManager.CreateActionRestrictingConstraint(true, true, false, new HelpPrinceBelen(),
                                                                           new List<PlanAction> { new HelpLordHarrowmont() }, 0); // helpBelenVsHelpHarrowmont

                    constraintManager.CreateActionRestrictingConstraint(true, true, false, new HelpLordHarrowmont(),
                                                                           new List<PlanAction> { new HelpPrinceBelen() }, 0); // helpHarrowmontVsHelpBelen

                    if (RandomEncounters)
                    {
                        HashSet<AgentStateStatic> enemyes = new HashSet<AgentStateStatic>()
                        {
                            currentStoryState.GetAgentByName("Darkspawns").Key, currentStoryState.GetAgentByName("Ghost").Key
                        };

                        HashSet<LocationStatic> locationsForEncauters = new HashSet<LocationStatic>()
                        {
                            currentStoryState.GetLocationByName("Crossroad").Key, currentStoryState.GetLocationByName("Road").Key,
                            currentStoryState.GetLocationByName("Riverside").Key
                        };

                        constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, false, false, false, true, false, false, false,
                                                                                 locationsForEncauters, enemyes, 0); // locationRestrictingForEnemyes

                        constraintManager.CreateRestrictingLocationConstraint(false, false, false, true, false, false, false, false, false, false, false,
                                                                                 null, enemyes, 0); // revisitBanForEnemyes

                        constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, false, false, false, false, true, false, false,
                                                                                 null, enemyes, 0); // doNotMoveEnemyes

                        /*constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, false, false, false, false, false, true, false,
                                                                                 locationsForEncauters, playerAgent, 0);*/ // playerMustVisitAtLeastOneOfThisLocation

                        constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, false, false, false, false, false, false, true,
                                                                                 locationsForEncauters, playerAgent, 0); // playerMustVisitOnlyOneOfThisLocations
                    }
                    break;
                case Setting.GenericFantasy:
                    HashSet<AgentStateStatic> heroAgent = new HashSet<AgentStateStatic>() { currentStoryState.GetAgentByName("Hero").Key };

                    HashSet<LocationStatic> questLocationsGeneric = new HashSet<LocationStatic>()
                    {
                        currentStoryState.GetLocationByName("AncientRuins").Key, currentStoryState.GetLocationByName("Dungeon").Key,
                        currentStoryState.GetLocationByName("MysteriousForest").Key
                    };

                    HashSet<AgentStateStatic> enemyesGeneric = new HashSet<AgentStateStatic>()
                    {
                        currentStoryState.GetAgentByName("Undead").Key, currentStoryState.GetAgentByName("Rogue").Key,
                        currentStoryState.GetAgentByName("Goblins").Key, currentStoryState.GetAgentByName("Barbarians").Key,
                        currentStoryState.GetAgentByName("InsidiousVillain").Key
                    };

                    constraintManager.CreateRestrictingLocationConstraint(false, false, false, true, false, false, false, false, false, false, false,
                                                                             null, heroAgent, 0); // revisitBanForHero

                    constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, true, false, false, false, false, false, false,
                                                                             questLocationsGeneric, heroAgent, 0); // heroMustCompleteQuestBeforeMoving

                    constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, false, false, false, false, true, false, false,
                                                                             null, enemyesGeneric, 0); // doNotMoveEnemyesGeneric

                    constraintManager.CreateActionRestrictingConstraint(false, false, true, new CompleteQuest(), null, 0); // oneQuestInOneLocation

                    constraintManager.CreateAliveConstraint(false, false, true, currentStoryState.GetAgentByRole(AgentRole.PLAYER).Key, 0); // endIfPlayerDied

                    constraintManager.CreateAliveConstraint(false, false, true, currentStoryState.GetAgentByName("InsidiousVillain").Key, 0); // endIfBossDied

                    break;
                case Setting.Detective:
                    HashSet<LocationStatic> detectiveLocations = new HashSet<LocationStatic>();
                    foreach (var location in currentStoryState.GetLocations())
                    {
                        detectiveLocations.Add(location.Key);
                    }

                    HashSet<AgentStateStatic> killerAgent = new HashSet<AgentStateStatic>();
                    killerAgent.Add(currentStoryState.GetAgentByRole(AgentRole.ANTAGONIST).Key);
                
                    constraintManager.CreateRestrictingLocationConstraint(false, false, false, false, true, false, false, false, false, false, false,
                                                         detectiveLocations, killerAgent, 0); // killerCantMoveTwice
                    break;
                case Setting.DefaultDemo: break;
            }

            if (ProtagonistWillSurvive)
            {
                if (ProtagonistProtectionTime == 0)
                {
                    constraintManager.CreateAliveConstraint(false, true, true, currentStoryState.GetAgentByRole(AgentRole.PLAYER).Key, 
                        ProtagonistProtectionTime);
                }
                else if (ProtagonistProtectionTime > 0)
                {
                    constraintManager.CreateAliveConstraint(true, false, true, currentStoryState.GetAgentByRole(AgentRole.PLAYER).Key, 
                        ProtagonistProtectionTime);
                }
            }

            if (AntagonistWillSurvive)
            {
                if (AntagonistProtectionTime == 0)
                {
                    constraintManager.CreateAliveConstraint(false, true, true, currentStoryState.GetAgentByRole(AgentRole.ANTAGONIST).Key,
                        AntagonistProtectionTime);
                }
                else if (AntagonistProtectionTime > 0)
                {
                    constraintManager.CreateAliveConstraint(true, false, true, currentStoryState.GetAgentByRole(AgentRole.ANTAGONIST).Key,
                        AntagonistProtectionTime);
                }
            }
        }

        /// <summary>
        ///  A method that creates a set of ready-made locations.
        /// </summary>
        /// <param name="locationNames">List of location names.</param>
        /// <param name="locationsEvidences">List of evidences in locations.</param>
        /// <returns>List of generated locations.</returns>
        public Dictionary<LocationStatic, LocationDynamic> CreateLocationSet (List<string> locationNames, List<bool> locationsEvidences)
        {
            // Create an empty set of locations.
            Dictionary<LocationStatic, LocationDynamic> locations = new Dictionary<LocationStatic, LocationDynamic>();

            // We create a location the required number of times, using the constructors of its static and dynamic parts, and then add it to the set.
            for (int i = 0; i < locationNames.Count; i++)
            {
                LocationStatic newLocationStatic = new LocationStatic(locationNames[i]);
                LocationDynamic newLocationDynamic = new LocationDynamic(locationsEvidences[i], newLocationStatic);
                locations.Add(newLocationStatic, newLocationDynamic);
            }

            // Shuffle the locations in the set in random order.
            OrderLocationsRandom(ref locations);

            // We establish connections between locations.
            locations = LocationsConnection(locations);

            // We return the ready-made set of locations.
            return locations;
        }

        /// <summary>
        /// A method that creates a set of beliefs to convey to agents.
        /// </summary>
        /// <param name="count">The size of the generated set.</param>
        /// <returns>List of agents beliefs.</returns>
        public List<WorldContext> CreateBeliefsSet (int count)
        {
            // Create an empty list of beliefs.
            List<WorldContext> newWorldBeliefsList = new List<WorldContext>();

            // We create the required number of instances of the agent representing the belief class and add them to the list.
            // We will fill them in at a later stage.
            for (int i = 0; i < count; i++)
            {
                WorldContext newWorldBeliefs = new WorldContext();
                newWorldBeliefsList.Add(newWorldBeliefs);
            }

            // Returning a list of beliefs.
            return newWorldBeliefsList;
        }

        /// <summary>
        /// A method that shuffles locations in a transferred set of locations.
        /// </summary>
        /// <param name="locations">List of locations in the world.</param>
        public void OrderLocationsRandom (ref Dictionary<LocationStatic, LocationDynamic> locations)
        {
            // Sorting locations based on their hashcode values.
            locations = locations.OrderBy(x => x.Value.GetHashCode()).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// A method that verifies that all locations in the transferred set are connected (there is a way that can bypass all locations).
        /// </summary>
        /// <param name="locations">List of locations in the world.</param>
        /// <returns>True if the path through all locations is found, false otherwise.</returns>
        public bool pathExistenceControlling (Dictionary<LocationStatic, LocationDynamic> locations)
        {
            bool result = false;

            // We create a queue and a set of visited locations.
            Queue<LocationStatic> queue = new Queue<LocationStatic>();
            HashSet<LocationStatic> visitedLocations = new HashSet<LocationStatic>();

            // We will use the first location from the list of locations as the root.
            LocationStatic root = locations.First().Key;

            // Add the root to the queue and the list of visited locations.
            queue.Enqueue(root);
            visitedLocations.Add(root);

            // As long as there is a location in the queue.
            while (queue.Count > 0)
            {
                // We take the location from the queue.
                LocationStatic currentLocation = queue.Dequeue();

                // We go through all the locations associated with the checked location.
                foreach (LocationStatic nextLocation in currentLocation.GetConnectedLocations())
                {
                    // If we have already visited one of these locations, then we just continue.
                    if (visitedLocations.Contains(nextLocation)) continue;

                    // Otherwise, add a new "unknown" location to the queue and the list of visited locations.
                    queue.Enqueue(nextLocation);
                    visitedLocations.Add(nextLocation);
                }
            }

            // If the number of locations visited during the passage is equal to the total number of locations, then the check was successful.
            if (visitedLocations.Count == locations.Count) { result = true; }

            return result;
        }


        /// <summary>
        /// A method that randomly assigns connections between locations.
        /// </summary>
        /// <param name="locations">List of locations in the world.</param>
        /// <returns>List of locations connected to each other.</returns>
        public Dictionary<LocationStatic, LocationDynamic> LocationsConnection (Dictionary<LocationStatic, LocationDynamic> locations)
        {
            bool pathExists = false;

            if (RandomConnectionOfLocations)
            {
                // Until the path is created.
                while (!pathExists)
                {
                    // We go through all the locations in the list and clear their connections with other locations.
                    foreach (var loc in locations)
                    {
                        loc.Key.ClearAllConnections();
                    }

                    // We go through all the locations in the location list.
                    foreach (var location in locations)
                    {
                        //Random random = new Random(Seed);
                        Random random = new Random();

                        int connectionLimit = random.Next(1, 2); // 3

                        // While the location has no connections with other locations.
                        while (location.Key.GetConnectedLocations().Count == 0)
                        {
                            for (int i = 0; i < connectionLimit; i++)
                            {
                                // We get a random number.
                                int rand = random.Next(locations.Count);

                                if (!location.Equals(locations.ElementAt(rand)) && !location.Key.ConnectionChecking(locations.ElementAt(rand).Key))
                                {
                                    // Then we connect these locations with each other.
                                    location.Key.AddConnection(locations.ElementAt(rand).Key);
                                    locations.ElementAt(rand).Key.AddConnection(location.Key);
                                }
                            }
                        }
                    }

                    // We check if there is a path that allows you to bypass all locations (i.e. are they all connected).
                    pathExists = pathExistenceControlling(locations);
                }
            }
            else if (setting.Equals(Setting.GenericFantasy))
            {
                foreach (var location1 in locations)
                {
                    foreach (var location2 in locations)
                    {
                        if (location1.Key.GetName().Equals("Village") && location2.Key.GetName().Equals("MountainPass"))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                        else if (location1.Key.GetName().Equals("MountainPass") &&
                            (location2.Key.GetName().Equals("AncientRuins") || location2.Key.GetName().Equals("MysteriousForest")))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                        else if (location1.Key.GetName().Equals("AncientRuins") && location2.Key.GetName().Equals("Dungeon"))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                        else if (location1.Key.GetName().Equals("Town") &&
                            (location2.Key.GetName().Equals("Dungeon") || location2.Key.GetName().Equals("MysteriousForest")))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                    }
                }
            }
            else if (setting.Equals(Setting.DragonAge) && RandomEncounters)
            {
                foreach (var location1 in locations)
                {
                    foreach (var location2 in locations)
                    {
                        if (location1.Key.GetName().Equals("Lothering") && 
                            (location2.Key.GetName().Equals("MagesTower") || 
                             location2.Key.GetName().Equals("BrecilianForest") || 
                             location2.Key.GetName().Equals("Orzammar")))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                        else if (location1.Key.GetName().Equals("Road") && 
                            (location2.Key.GetName().Equals("MagesTower") || 
                             location2.Key.GetName().Equals("BrecilianForest") || 
                             location2.Key.GetName().Equals("Orzammar")))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                        else if (location1.Key.GetName().Equals("Riverside") && 
                            (location2.Key.GetName().Equals("MagesTower") ||
                             location2.Key.GetName().Equals("BrecilianForest") ||
                             location2.Key.GetName().Equals("Orzammar")))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                        else if (location1.Key.GetName().Equals("Crossroad") && 
                            (location2.Key.GetName().Equals("MagesTower") ||
                             location2.Key.GetName().Equals("BrecilianForest") ||
                             location2.Key.GetName().Equals("Orzammar")))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                        else if (location1.Key.GetName().Equals("Denerim") &&
                            (location2.Key.GetName().Equals("MagesTower") ||
                            location2.Key.GetName().Equals("BrecilianForest") ||
                            location2.Key.GetName().Equals("Orzammar") ||
                            location2.Key.GetName().Equals("Crossroad") ||
                            location2.Key.GetName().Equals("Riverside") ||
                            location2.Key.GetName().Equals("Road")))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                        else if (location1.Key.GetName().Equals("DeepRoads") && location2.Key.GetName().Equals("Denerim"))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                    }
                }
            }
            else if (setting.Equals(Setting.Detective))
            {
                foreach (var location1 in locations)
                {
                    foreach (var location2 in locations)
                    {
                        if ((location1.Key.GetName().Equals("Rock") && location2.Key.GetName().Equals("Hall"))
                            || (location1.Key.GetName().Equals("Beach") && location2.Key.GetName().Equals("Rock"))
                            || (location1.Key.GetName().Equals("LivingRooms") && location2.Key.GetName().Equals("Hall")))
                        {
                            location1.Key.AddConnection(location2.Key);
                            location2.Key.AddConnection(location1.Key);
                        }
                    }
                }
            }

            // Returns locations with established connections between them.
            return locations;
        }

        /// <summary>
        /// A method that checks the correct connection of nodes in a graph and fixes bugs.
        /// </summary>
        /// <param name="newStoryGraph">Generated story graph.</param>
        public void FixingOfIncorrectlyConnectedNodes (ref StoryGraph newStoryGraph)
        {
            for (int i = 1; i < newStoryGraph.GetNodes().Count; i++)
            {
                if (newStoryGraph.GetNodes().ElementAt(i).GetEdges().Count == 1 && !newStoryGraph.GetNodes().ElementAt(i).goalState)
                {
                    int troubleNodeNumber = newStoryGraph.GetNodes().ElementAt(i).GetNumberInSequence();
                    int radius = 10;

                    for (int j = troubleNodeNumber; j < troubleNodeNumber + radius; j++)
                    {
                        int dif = newStoryGraph.GetNodes().ElementAt(j).GetEdges().Count - newStoryGraph.GetNodes().ElementAt(j).GetLinks().Count;
                        if (dif > 1)
                        {
                            int stopNumber = (newStoryGraph.GetNodes().ElementAt(j).GetEdges().Count - 1) / 2;
                            int startNumber = (newStoryGraph.GetNodes().ElementAt(j).GetEdges().Count) - 1;

                            for (int z = startNumber; z > stopNumber; z--)
                            {
                                newStoryGraph.GetNodes().ElementAt(j).GetEdges().ElementAt(z).SetUpperNode(newStoryGraph.GetNodes().ElementAt(i));
                                newStoryGraph.GetNodes().ElementAt(i).AddEdge(newStoryGraph.GetNodes().ElementAt(j).GetEdges().ElementAt(z));
                                newStoryGraph.GetNodes().ElementAt(j).RemoveEdge(newStoryGraph.GetNodes().ElementAt(j).GetEdges().ElementAt(z));
                            }

                            break;
                        }
                    }

                    for (int j = troubleNodeNumber; j > troubleNodeNumber - radius; j--)
                    {
                        int dif = newStoryGraph.GetNodes().ElementAt(j).GetEdges().Count - newStoryGraph.GetNodes().ElementAt(j).GetLinks().Count;
                        if (dif > 1)
                        {
                            int stopNumber = (newStoryGraph.GetNodes().ElementAt(j).GetEdges().Count - 1) / 2;
                            int edgesCount = newStoryGraph.GetNodes().ElementAt(j).GetEdges().Count;
                            int startNumber = edgesCount - 1;

                            for (int z = startNumber; z > stopNumber; z--)
                            {
                                newStoryGraph.GetNodes().ElementAt(j).GetEdges().ElementAt(z).SetUpperNode(newStoryGraph.GetNodes().ElementAt(i));
                                newStoryGraph.GetNodes().ElementAt(i).AddEdge(newStoryGraph.GetNodes().ElementAt(j).GetEdges().ElementAt(z));
                                newStoryGraph.GetNodes().ElementAt(j).RemoveEdge(newStoryGraph.GetNodes().ElementAt(j).GetEdges().ElementAt(z));
                            }

                            break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Method is an entry point that controls the operation of the algorithm (the sequence of launching other methods).
        /// </summary>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="txtOutputPath">The path to write the resulting files.</param>
        public void Start (ref TextBox note, string txtOutputPath)
        {
            // We read the settings and create the initial state of the world.
            ReadUserSettingsInput(ref note);
            CreateInitialState(ref note);
            CreateConstraints(ref note);
            storyworldConvergence.SetConstraints(constraintManager.GetConstraints());
            pddlModule.GeneratePDDLDomains(ref note);

            // We create a start node (root) based on the start state of the world.
            note.Text = "ROOT NODE CREATING";
            StoryNode root = new StoryNode();
            root.SetWorldState(currentStoryState);
            root.SetActivePlayer(false);
            root.SetActiveAgent(currentStoryState.GetFirstAgent());
            newStoryGraph.SetRoot(root);

            // The algorithm calculates a SPECIFIC story.
            note.Text = "STORY GRAPH CREATING - START";
            newStoryGraph = CreateStoryGraph(newStoryGraph.GetRoot(), ref note, txtOutputPath);

            FixingOfIncorrectlyConnectedNodes(ref newStoryGraph);

            // Create a visual graph.
            note.Text = "VISUAL GRAPH CREATING";
            DialogResult result = MessageBox.Show(
                "Would you like to continue or move on to the next step?",
                "Graph visualization",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.No) {}
            else if (result == DialogResult.Yes)
            {
                graphСonstructor.CreateGraph(newStoryGraph, txtOutputPath + @"\newStoryGraph.dt", ref note);
            }

            // Create an HTML file including Twine engine and generated history.
            note.Text = "TWINE FILE CREATING";
            DialogResult result2 = MessageBox.Show(
                "Would you like to continue or move on to the next step?",
                "Creating a file for TWINE based on a graph",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

            if (result2 == DialogResult.No) { }
            else if (result2 == DialogResult.Yes)
            {
                twineGraphConstructor.CreateTwineGraph(newStoryGraph, txtOutputPath);
            }

            DialogResult result3 = MessageBox.Show(
                "The program has been successfully completed.",
                "Exit",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

            if (result3 == DialogResult.OK) { return; }
        }

        /// <summary>
        /// A method in which we sequentially create a story graph, node by node, starting at the root, using the concept of Breadth First Search.
        /// </summary>
        /// <param name="rootNode">The starting node of the graph (root).</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="root">A marker that is considering the root node.</param>
        public void BFSTraversing (StoryNode rootNode, ref TextBox note, bool root = true)
        {
            note.Text = "STORY GRAPH CREATING - CONSTRUCTION";

            // We create a queue and a list of visited nodes.
            Queue<StoryNode> queue = new Queue<StoryNode>();
            HashSet<StoryNode> visitedNodes = new HashSet<StoryNode>();

            // We add the node to the queue and the list of visited nodes.
            queue.Enqueue(rootNode);
            visitedNodes.Add(rootNode);

            // We initialize numeric variables storing the number of the current agent and the number of the last node in the sequence.
            int actualAgentNumber = 0;
            int globalNodeNumber = -1;

            // We will perform the action in a loop until the queue becomes empty.
            while (queue.Count > 0 && newStoryGraph.GetNodes().Count <= MaxNodes)
            {
                // We take the node in question from the queue.
                StoryNode currentNode = queue.Dequeue();

                // If we come across a node with a target state, then we do not expand it.
                if (goalManager.ControlToAchieveGoalState(ref currentNode))
                {
                    continue;
                }
                else
                {
                    // If the node in question is a root.
                    if (currentNode.Equals(rootNode) && root)
                    {
                        // We call the method for creating a new node.
                        Step(newStoryGraph.GetRoot(), actualAgentNumber, root, ref globalNodeNumber, ref queue, ref note, EvidenceFindChance);

                        // Add the considered node back to the queue (in the case of the root, we only changed it and will consider it again),
                        //    and also to the list of visited nodes. We remove the flag indicating that we are considering the root node.
                        queue.Enqueue(currentNode);
                        visitedNodes.Add(currentNode);
                        root = false;
                    }
                    // If the node in question IS NOT a root.
                    else
                    {
                        // We determine the index of the agent, which will have to act when creating a new node.
                        actualAgentNumber = GetActualAgentNumber(currentNode.GetWorldState().GetIndexOfAgent(currentNode.GetActiveAgent()), ref currentNode);

                        // We call the method to create a new node.
                        Step(newStoryGraph.GetNode(currentNode), actualAgentNumber, root, ref globalNodeNumber, ref queue, ref note, EvidenceFindChance);
                    }
                }

                // We go through the nodes associated with the considered one.
                foreach (StoryNode nextNode in currentNode.GetLinks())
                {
                    // If one of them has already been visited earlier, then we continue, moving on to the next.
                    if (visitedNodes.Contains(nextNode))
                    {
                        continue;
                    }

                    bool equal = false;
                    foreach (var visitedNode in visitedNodes)
                    {
                        if (visitedNode.Equals(nextNode))
                        {
                            equal = true;
                        }
                    }

                    if (equal) { continue; }

                    // Otherwise, we add them to the queue and the list of visited nodes.
                    queue.Enqueue(nextNode);
                    visitedNodes.Add(nextNode);
                }

                // We clear the link pointing to the node in question.
                currentNode = null;
            }
        }

        /// <summary>
        /// A method that traverses the graph according to the concept of breadth-first search 
        ///    and determines the presence of at least one target state in it.
        /// </summary>
        /// <param name="rootNode">The starting node of the graph (root).</param>
        /// <returns>True if the graph contains at least one node where the goal state is reached, otherwise false.</returns>
        public bool BFSGoalAchieveControl (StoryNode rootNode)
        {
            // We create a queue and a list of visited nodes.
            Queue<StoryNode> queue = new Queue<StoryNode>();
            HashSet<StoryNode> visitedNodes = new HashSet<StoryNode>();

            // Add the root node to the queue and the list of visited nodes.
            queue.Enqueue(rootNode);
            visitedNodes.Add(rootNode);

            // We are in a loop until the queue is empty.
            while (queue.Count > 0)
            {
                // We take the current node under consideration from the queue.
                StoryNode currentNode = queue.Dequeue();

                // We check if the target state has been reached in the node under consideration.
                reachedGoalState = goalManager.ControlToAchieveGoalState(ref currentNode);

                // If so, terminate the method and return true.
                if (reachedGoalState) { return true; }

                // Otherwise, we go through the nodes associated with the node in question.
                foreach (StoryNode nextNode in currentNode.GetLinks())
                {
                    // If they have already been visited earlier, then we continue, moving on to the next.
                    if (visitedNodes.Contains(nextNode)) continue;

                    // Otherwise, we add to the queue and the list of visited nodes.
                    queue.Enqueue(nextNode);
                    visitedNodes.Add(nextNode);
                }
            }

            // If no target state was found, then terminate the method and return false.
            return false;
        }

        /// <summary>
        /// The method that controls the creation of the story graph.
        /// </summary>
        /// <param name="rootNode">The starting node of the graph (root).</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="txtOutputPath">The path to write the resulting files.</param>
        /// <returns>Generated story graph.</returns>
        public StoryGraph CreateStoryGraph (StoryNode rootNode, ref TextBox note, string txtOutputPath)
        {
            // We clone the root into a separate variable.
            StoryNode originRoot = (StoryNode)rootNode.Clone();

            // We continue to work until at least one target state is reached.
            while (!reachedGoalState)
            {
                note.Text = "STORY GRAPH CREATING - MAIN CYCLE";

                // We create a new graph by starting to expand the root.
                BFSTraversing(newStoryGraph.GetRoot(), ref note);

                note.Text = "GOAL ACHIEVE CONTROL";

                // We go through the created graph, looking for target states in it.
                BFSGoalAchieveControl(newStoryGraph.GetRoot());

                // If it was not possible to find even one target state in the constructed graph.
                if (!reachedGoalState || newStoryGraph.GetNodes().Count > MaxNodes)
                {
                    note.Text = "GRAPH RECREATE";

                    MaxNodes = MaxNodes * 2;

                    DialogResult result = MessageBox.Show(
                        "Would you like to save your result before trying again?",
                        "Failed to construct graph",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);

                    if (result == DialogResult.Yes)
                    {
                        if (HideNothingToDoActions) graphСonstructor.HideNTDActions();
                        graphСonstructor.CreateGraph(newStoryGraph, txtOutputPath + @"\newStoryGraph.dt", ref note);
                    }
                    else if (result == DialogResult.No) {}

                    // Then we clear the graph, and all the links added to the root.
                    newStoryGraph = new StoryGraph();
                    originRoot.GetEdges().Clear();
                    originRoot.GetLinks().Clear();

                    // After that, we reassign to the previously cleared column an indication of the root.
                    newStoryGraph.SetRoot((StoryNode)originRoot.Clone());

                    reachedGoalState = false;
                }
            }

            // We return a graph that is guaranteed to have at least one target state.
            return newStoryGraph;
        }

        /// <summary>
        /// Convergence in turn asks agents for actions, checks them, applies them, counteracts them, or does not.
        /// </summary>
        /// <param name="currentNode">Considered graph node.</param>
        /// <param name="agentIndex">The index of the active agent (performing the action).</param>
        /// <param name="root">A marker that is considering the root node.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="queue">The queue of nodes to be considered.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="findEvidenceShance">The agent's chance to find a evidence when performing the action to find it.</param>
        public void Step (StoryNode currentNode, int agentIndex, bool root, ref int globalNodeNumber, ref Queue<StoryNode> queue, ref TextBox note, int findEvidenceShance)
        {
            note.Text = "AGENT ACTION REQUEST";

            // Convergence assigns who is on the turn to the node and then applies the changes to the state of the world.
            currentStoryState = currentNode.GetWorldState();
            currentStoryState.GetStaticWorldPart().IncreaseTurnNumber();

            while (!currentStoryState.GetAgentByIndex(agentIndex).Value.GetStatus())
            {
                agentIndex = GetActualAgentNumber(agentIndex, ref currentNode);
            }

            // We check if the agent from whom we are going to request an action is alive (i.e. capable of doing it).
            if (currentStoryState.GetAgentByIndex(agentIndex).Value.GetStatus())
            {
                // We create a request for action of the specified agent from the specified state.
                storyworldConvergence.ActionRequest(currentStoryState.GetAgentByIndex(agentIndex), ref newStoryGraph, ref currentStoryState, 
                                                     currentNode, root, ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
            }
        }

        /// <summary>
        /// A method that returns the index of the agent that should perform the action.
        /// </summary>
        /// <param name="prevNumber">Index of the agent who performed the action in the previous state.</param>
        /// <param name="currentNode">Link to the current node.</param>
        /// <returns>The index of the agent that is currently to perform the action.</returns>
        public int GetActualAgentNumber (int prevNumber, ref StoryNode currentNode)
        {
            // Flag for checking if the agent being checked is alive.
            bool aliveControl = false;

            // Determine how many agents exist in the current state.
            int maxNumber = currentNode.GetWorldState().GetNumberOfAgents();

            // Default result.
            int result = 0;

            // Until the flag signaling the status of the agent being checked is omitted.
            while (!aliveControl)
            {
                // If the index of the previous agent is equal to the maximum possible index.
                if (prevNumber == maxNumber - 1)
                {
                    // Then we go back to the beginning of the index list and set 0 as the result (the circle is passed).
                    result = 0;
                    prevNumber = 0;
                }
                else
                {
                    // Otherwise, we increase the value of the index of the previous agent by one and write it down as a result.
                    prevNumber++;
                    result = prevNumber;
                }

                // We check if the agent with the received index is alive. If not, we continue the cycle.
                if (!currentNode.GetWorldState().GetAgentByIndex(result).Value.GetStatus()) { continue; }

                // Otherwise, we raise the flag that the control has been passed.
                aliveControl = true;
            }

            // We return the result - the index of the guaranteed living agent acting after the specified in the parameters.
            return result;
        }
    }
}