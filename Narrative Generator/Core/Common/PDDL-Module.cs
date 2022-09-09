﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class PDDL_Module
    {
        Setting setting;
        bool locationsAreConnected;
        int agentsCounter;

        // Characters behaviours settings
        public bool talkativeAntagonist;
        public bool talkativeEnemies;
        public bool cunningAntagonist;
        public bool cunningEnemies;
        public bool peacefulAntagonist;
        public bool peacefulEnemies;
        public bool silentProtagonist;
        public bool silentCharacters;
        public bool aggresiveProtagonist;
        public bool aggresiveCharacters;
        public bool cowardlyProtagonist;
        public bool cowardlyCharacters;

        public PDDL_Module()
        {
            setting = Setting.DefaultDemo;
            locationsAreConnected = true;
            agentsCounter = 6;
        }

        public PDDL_Module (Setting setting, bool locationsAreConnected, int agentsCounter)
        {
            this.setting = setting;
            this.locationsAreConnected = locationsAreConnected;
            this.agentsCounter = agentsCounter;
        }

        public void Initialization (Setting setting, bool locationsAreConnected, int agentsCounter)
        {
            this.setting = setting;
            this.locationsAreConnected = locationsAreConnected;
            this.agentsCounter = agentsCounter;
        }

        public void charactersBehaviourInitialization(bool talkativeAntagonist, bool talkativeEnemies, bool cunningAntagonist, bool cunningEnemies, bool peacefulAntagonist,
                                                      bool peacefulEnemies, bool silentProtagonist, bool silentCharacters, bool aggresiveProtagonist, bool aggresiveCharacters,
                                                      bool cowardlyProtagonist, bool cowardlyCharacters)
        {
            this.talkativeAntagonist = talkativeAntagonist;
            this.talkativeEnemies = talkativeEnemies;
            this.cunningAntagonist = cunningAntagonist;
            this.cunningEnemies = cunningEnemies;
            this.peacefulAntagonist = peacefulAntagonist;
            this.peacefulEnemies = peacefulEnemies;
            this.silentProtagonist = silentProtagonist;
            this.silentCharacters = silentCharacters;
            this.aggresiveProtagonist = aggresiveProtagonist;
            this.aggresiveCharacters = aggresiveCharacters;
            this.cowardlyProtagonist = cowardlyProtagonist;
            this.cowardlyCharacters = cowardlyCharacters;
        }

        public void GeneratePDDLDomains()
        {
            string fileName = "";
            string domainName = "";
            string predicates = "";
            string actions = "";

            int numberOfKillers = 1;
            int numberOfVictims = 1;

            string connected = null;
            if (locationsAreConnected) { connected = "(connected ?room-from ?room-to)"; }
            else { connected = ""; }

            foreach (AgentRole role in Enum.GetValues(typeof(AgentRole)))
            {
                fileName = role.ToString() + "Domain";
                domainName = setting.ToString().ToLower() + "-domain";

                predicates = "(ROOM ?x) (AGENT ?x) (ANTAGONIST ?x) (ENEMY ?x) (PLAYER ?x) (USUAL ?x) (alive ?x) (died ?x) (wait ?x) (in-room ?x ?y) (connected ?x ?y) " +
                    "(complete-quest ?x) (want-go-to ?x ?y) (thinks-is-a-killer ?x ?y) (found-evidence-against ?x ?y) (scared ?x) (angry-at ?x ?y) " +
                    "(explored-room ?x ?y) (contains-evidence ?x) (talking ?x ?y)";

                if (role.Equals(AgentRole.ANTAGONIST) || role.Equals(AgentRole.ENEMY))
                {
                    if (setting.Equals(Setting.DefaultDemo))
                    {
                        // Action - Entrap
                        actions = actions.Insert(actions.Length, Environment.NewLine + "(:action Entrap" + Environment.NewLine
                           + " :parameters(?k ?a ?place)" + Environment.NewLine
                           + " :precondition (and (ROOM ?place) (" + role.ToString() + " ?k) (AGENT ?a) (alive ?k) (alive ?a)" + Environment.NewLine
                           + " (in-room ?k ?place) (not (in-room ?a ?place)))" + Environment.NewLine
                           + " :effect (and (in-room ?a ?place)))" + Environment.NewLine);

                        // Action - Kill
                        actions = actions.Insert(actions.Length, Environment.NewLine + "(:action Kill" + Environment.NewLine + ":parameters (?k ?victim ?r");
                        for (int i = 1; i <= agentsCounter - numberOfKillers - numberOfVictims; i++)
                        {
                            actions = actions.Insert(actions.Length, " ?a" + i);
                        }
                        actions = actions.Insert(actions.Length, ")" + Environment.NewLine);
                        actions = actions.Insert(actions.Length, ":precondition (and (ROOM ?r) (" + role.ToString() + " ?k) (AGENT ?victim)");
                        for (int i = 1; i <= agentsCounter - numberOfKillers - numberOfVictims; i++)
                        {
                            actions = actions.Insert(actions.Length, "(AGENT ?a" + i + ") ");
                        }
                        actions = actions.Insert(actions.Length, Environment.NewLine);
                        for (int i = 1; i <= agentsCounter - numberOfKillers - numberOfVictims; i++)
                        {
                            actions = actions.Insert(actions.Length, "(not ( = ?victim ?a" + i + ")) ");
                        }
                        actions = actions.Insert(actions.Length, Environment.NewLine);
                        for (int i = 1; i <= agentsCounter - numberOfKillers - numberOfVictims; i++)
                        {
                            for (int j = i; j <= agentsCounter - numberOfKillers - numberOfVictims; j++)
                            {
                                if (i != j)
                                {
                                    actions = actions.Insert(actions.Length, "(not ( = ?a" + i + " ?a" + j + ")) ");
                                }
                            }
                        }
                        actions = actions.Insert(actions.Length, Environment.NewLine);
                        actions = actions.Insert(actions.Length, " (alive ?k) (alive ?victim) " + Environment.NewLine);
                        actions = actions.Insert(actions.Length, "(in-room ?k ?r) (in-room ?victim ?r)" + Environment.NewLine);
                        for (int i = 1; i <= agentsCounter - numberOfKillers - numberOfVictims; i++)
                        {
                            actions = actions.Insert(actions.Length, " (or (and (alive ?a" + i + ")" + "(not (in-room ?a" + i + " ?r)))  (died ?a" + i + "))");
                        }
                        actions = actions.Insert(actions.Length, Environment.NewLine + ")" + Environment.NewLine);
                        actions = actions.Insert(actions.Length, ":effect (and (died ?victim) (not (alive ?victim))))" + Environment.NewLine);
                    }
                    else
                    {
                        // Action - Kill
                        actions = actions.Insert(actions.Length, Environment.NewLine + "(:action Kill" + Environment.NewLine + ":parameters (?k ?victim ?r)");
                        actions = actions.Insert(actions.Length, Environment.NewLine + ":precondition (and (ROOM ?r) (" + role.ToString() + " ?k) (AGENT ?victim)");
                        actions = actions.Insert(actions.Length, " (alive ?k) (alive ?victim) " + Environment.NewLine);
                        actions = actions.Insert(actions.Length, "(in-room ?k ?r) (in-room ?victim ?r))" + Environment.NewLine);
                        actions = actions.Insert(actions.Length, ":effect (and (died ?victim) (not (alive ?victim))))" + Environment.NewLine);
                    }

                    if (talkativeAntagonist)
                    {
                        // Action - Tell about a suspicious
                        actions = actions.Insert(actions.Length, Environment.NewLine + "(:action TellAboutASuspicious" + Environment.NewLine
                            + " :parameters (?k ?a ?place ?suspicious-place)" + Environment.NewLine
                            + " :precondition (and (ROOM ?place) (ROOM ?suspicious-place) (" + role.ToString() + " ?k)(AGENT ?a) (alive ?k) (alive ?a) " + Environment.NewLine
                            + "(in-room ?k ?place) (in-room ?a ?place) (not (= ?place ?suspicious-place)))" + Environment.NewLine
                            + " :effect (and (in-room ?a ?suspicious-place)))" + Environment.NewLine);
                    }
                }              

                if (role.Equals(AgentRole.PLAYER) || role.Equals(AgentRole.USUAL))
                {
                    if (role.Equals(AgentRole.PLAYER))
                    {
                        if (aggresiveProtagonist)
                        {
                            // Action - Fight 1
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action fight-enemy" + Environment.NewLine
                                + " :parameters (?p ?e ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (ENEMY ?e) (PLAYER ?p) (alive ?e) (alive ?p) (in-room ?p ?place)(in-room ?e ?place))"
                                + Environment.NewLine + " :effect (and (died ?e) (not (alive ?e))))" + Environment.NewLine);

                            // Action - Fight 2
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action fight-antagonist" + Environment.NewLine
                                + " :parameters (?p ?a ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (ANTAGONIST ?a) (PLAYER ?p) (alive ?a) (alive ?p) (in-room ?p ?place)(in-room ?a ?place))"
                                + Environment.NewLine + " :effect (and (died ?a) (not (alive ?a))))" + Environment.NewLine);
                        }

                        if (setting.Equals(Setting.GenericFantasy))
                        {
                            // Action - Complete Quest
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action complete-quest" + Environment.NewLine
                                + " :parameters (?p ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?p) (alive ?p) (in-room ?p ?place))" + Environment.NewLine
                                + " :effect (and (complete-quest ?p)))" + Environment.NewLine);
                        }

                        if (setting.Equals(Setting.DragonAge))
                        {
                            // Action - Help Mages
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action help-mages" + Environment.NewLine
                                + " :parameters (?p ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?p) (alive ?p) (in-room ?p ?place))" + Environment.NewLine
                                + " :effect (and (complete-quest ?p)))" + Environment.NewLine);

                            // Action - Help Templars
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action help-templars" + Environment.NewLine
                                + " :parameters (?p ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?p) (alive ?p) (in-room ?p ?place))" + Environment.NewLine
                                + " :effect (and (complete-quest ?p)))" + Environment.NewLine);

                            // Action - Help Elfs
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action help-elfs" + Environment.NewLine
                                + " :parameters (?p ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?p) (alive ?p) (in-room ?p ?place))" + Environment.NewLine
                                + " :effect (and (complete-quest ?p)))" + Environment.NewLine);

                            // Action - Help Werewolves
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action help-werewolves" + Environment.NewLine
                                + " :parameters (?p ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?p) (alive ?p) (in-room ?p ?place))" + Environment.NewLine
                                + " :effect (and (complete-quest ?p)))" + Environment.NewLine);

                            // Action - Help Prince Belen
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action help-prince" + Environment.NewLine
                                + " :parameters (?p ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?p) (alive ?p) (in-room ?p ?place))" + Environment.NewLine
                                + " :effect (and (complete-quest ?p)))" + Environment.NewLine);

                            // Action - Help Lord Harrowmont
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action help-lord" + Environment.NewLine
                                + " :parameters (?p ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?p) (alive ?p) (in-room ?p ?place))" + Environment.NewLine
                                + " :effect (and (complete-quest ?p)))" + Environment.NewLine);
                        }

                        if (setting.Equals(Setting.Detective) || setting.Equals(Setting.DefaultDemo))
                        {
                            // Action - Talk 
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action talk" + Environment.NewLine
                                + " :parameters (?a1 ?a2 ?place)" + Environment.NewLine
                                + " :precondition (and (PLAYER ?a1) (AGENT ?a2) (ROOM ?place) (alive ?a1) (alive ?a2) (in-room ?a1 ?place)(in-room ?a2 ?place) " +
                                "(not (angry-at ?a1 ?a2)))" + Environment.NewLine
                                + " :effect (and (talking ?a1 ?a2)))" + Environment.NewLine);

                            // Action - To be a witness
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action to-be-a-witness" + Environment.NewLine
                                + " :parameters (?a ?victim ?k ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?a) (ANTAGONIST ?k) (alive ?a) (alive ?k) (died ?victim) " +
                                "(in-room ?a ?place) (in-room ?victim ?place))"
                                + Environment.NewLine + " :effect (and (thinks-is-a-killer ?a ?k)))" + Environment.NewLine);

                            // Action - Investigate room
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action investigate-room" + Environment.NewLine
                                + " :parameters (?a ?room)" + Environment.NewLine
                                + " :precondition (and (ROOM ?room) (PLAYER ?a) (alive ?a) (in-room ?a ?room) (not (died ?a)))" + Environment.NewLine
                                + " :effect (and (explored-room ?a ?room)))" + Environment.NewLine);

                            // Action - Tell about killer 
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action tell-about-killer" + Environment.NewLine
                                + " :parameters (?p ?a ?place ?k)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (PLAYER ?p) (AGENT ?a) (ANTAGONIST ?k) (alive ?p) (alive ?a) (alive ?k) " +
                                "(in-room ?p ?place) (in-room ?a ?place) (found-evidence-against ?p ?k))" + Environment.NewLine
                                + " :effect (and (thinks-is-a-killer ?a ?k) (angry-at ?a ?k)))" + Environment.NewLine);

                            // Action - Neutralize killer
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action NeutralizeKiller" + Environment.NewLine
                                + " :parameters (?a ?k ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (ANTAGONIST ?k) (PLAYER ?a) (alive ?k) (alive ?a) (in-room ?a ?place) " +
                                "(in-room ?k ?place) (angry-at ?a ?k))" + Environment.NewLine
                                + " :effect (and (died ?k) (not (alive ?k))))" + Environment.NewLine);

                            // Action - Reassure
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action reassure" + Environment.NewLine
                                + " :parameters (?a1 ?p ?a2 ?k ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (AGENT ?a1) (PLAYER ?p) (AGENT ?a2) (ANTAGONIST ?k) (alive ?a1) (alive ?p) " +
                                "(in-room ?a1 ?place) (in-room ?p ?place) (or (angry-at ?a1 ?a2) (angry-at ?a1 ?k)) (not (thinks-is-a-killer ?p ?k)))"
                                + " :effect (and (not (angry-at ?a1 ?a2)) (not (angry-at ?a1 ?k))))" + Environment.NewLine);
                        }
                    }

                    if (role.Equals(AgentRole.USUAL))
                    {
                        // Action - Run
                        actions = actions.Insert(actions.Length, Environment.NewLine + "(:action Run" + Environment.NewLine
                            + " :parameters (?a ?room-from ?room-to)" + Environment.NewLine
                            + " :precondition (and (ROOM ?room-from) (ROOM ?room-to) (AGENT ?a) (alive ?a) (scared ?a) (in-room ?a ?room-from) " +
                            "(not (died ?a)) (not (in-room ?a ?room-to)))" + Environment.NewLine
                            + " :effect (and (in-room ?a ?room-to) (not (in-room ?a ?room-from))))" + Environment.NewLine);

                        // Action - Agent Move To Target
                        actions = actions.Insert(actions.Length, Environment.NewLine + "(:action agent_move_to_target " + Environment.NewLine
                            + " :parameters (?a ?room-from ?room-to)" + Environment.NewLine
                            + " :precondition (and (ROOM ?room-from) (ROOM ?room-to) (AGENT ?a) (alive ?a) (not (scared ?a)) (in-room ?a  ?room-from) (want-go-to ?a ?room-to)" +
                            " (not (died ?a)) (not (in-room ?a ?room-to)) (connected ?room-from ?room-to))" + Environment.NewLine
                             + " :effect (and (in-room ?a ?room-to) (not (in-room ?a ?room-from))))" + Environment.NewLine);

                        if (setting.Equals(Setting.Detective) || setting.Equals(Setting.DefaultDemo))
                        {
                            if (!silentCharacters)
                            {
                                // Action - Talk 
                                actions = actions.Insert(actions.Length, Environment.NewLine + "(:action talk" + Environment.NewLine
                                    + " :parameters (?a1 ?a2 ?place)" + Environment.NewLine
                                    + " :precondition (and (AGENT ?a1) (AGENT ?a2) (ROOM ?place) (alive ?a1) (alive ?a2) (in-room ?a1 ?place)(in-room ?a2 ?place) " +
                                    "(not (angry-at ?a1 ?a2)))" + Environment.NewLine
                                    + " :effect (and (talking ?a1 ?a2)))" + Environment.NewLine);
                            }

                            // Action - To be a witness
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action to-be-a-witness" + Environment.NewLine
                                + " :parameters (?a ?victim ?k ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (AGENT ?a) (ANTAGONIST ?k) (alive ?a) (alive ?k) (died ?victim) " +
                                "(in-room ?a ?place) (in-room ?victim ?place))"
                                + Environment.NewLine + " :effect (and (thinks-is-a-killer ?a ?k)))" + Environment.NewLine);

                            if (cowardlyCharacters)
                            {
                                // Action - Investigate room
                                actions = actions.Insert(actions.Length, Environment.NewLine + "(:action investigate-room" + Environment.NewLine
                                    + " :parameters (?a ?room)" + Environment.NewLine
                                    + " :precondition (and (ROOM ?room) (AGENT ?a) (alive ?a) (in-room ?a ?room) (not (died ?a)))" + Environment.NewLine
                                    + " :effect (and (explored-room ?a ?room)))" + Environment.NewLine);
                            }

                            // Action - Tell about killer 
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action tell-about-killer" + Environment.NewLine
                                + " :parameters (?p ?a ?place ?k)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (AGENT ?p) (AGENT ?a) (ANTAGONIST ?k) (alive ?p) (alive ?a) (alive ?k) " +
                                "(in-room ?p ?place) (in-room ?a ?place) (found-evidence-against ?p ?k))" + Environment.NewLine
                                + " :effect (and (thinks-is-a-killer ?a ?k) (angry-at ?a ?k)))" + Environment.NewLine);

                            if (aggresiveCharacters)
                            {
                                // Action - Neutralize killer
                                actions = actions.Insert(actions.Length, Environment.NewLine + "(:action NeutralizeKiller" + Environment.NewLine
                                    + " :parameters (?a ?k ?place)" + Environment.NewLine
                                    + " :precondition (and (ROOM ?place) (ANTAGONIST ?k) (AGENT ?a) (alive ?k) (alive ?a) (in-room ?a ?place) " +
                                    "(in-room ?k ?place) (angry-at ?a ?k))" + Environment.NewLine
                                    + " :effect (and (died ?k) (not (alive ?k))))" + Environment.NewLine);
                            }

                            // Action - Reassure
                            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action reassure" + Environment.NewLine
                                + " :parameters (?a1 ?p ?a2 ?k ?place)" + Environment.NewLine
                                + " :precondition (and (ROOM ?place) (AGENT ?a1) (AGENT ?p) (AGENT ?a2) (ANTAGONIST ?k) (alive ?a1) (alive ?p) " +
                                "(in-room ?a1 ?place) (in-room ?p ?place) (or (angry-at ?a1 ?a2) (angry-at ?a1 ?k)) (not (thinks-is-a-killer ?p ?k)))"
                                + " :effect (and (not (angry-at ?a1 ?a2)) (not (angry-at ?a1 ?k))))" + Environment.NewLine);
                        }
                    }
                }

                if (role.Equals(AgentRole.USUAL))
                {
                    // Action - Move
                    actions = actions.Insert(actions.Length, Environment.NewLine + "(:action move" + Environment.NewLine
                        + " :parameters (?a ?room-from ?room-to)" + Environment.NewLine
                        + " :precondition (and (ROOM ?room-from) (ROOM ?room-to) (AGENT ?a) (alive ?a)" + Environment.NewLine
                        + " (in-room ?a ?room-from) (not (died ?a)) (not (in-room ?a ?room-to))" + connected + ")" + Environment.NewLine
                        + " :effect (and (in-room ?a ?room-to) (not (in-room ?a ?room-from))))" + Environment.NewLine);

                    // Action - Nothing to do
                    actions = actions.Insert(actions.Length, Environment.NewLine + "(:action nothing-to-do" + Environment.NewLine
                        + " :parameters (?a)" + Environment.NewLine
                        + " :precondition (and (AGENT ?a) (alive ?a))" + Environment.NewLine
                        + " :effect (wait ?a))" + Environment.NewLine);
                }
                else
                {
                    // Action - Move
                    actions = actions.Insert(actions.Length, Environment.NewLine + "(:action move" + Environment.NewLine
                        + " :parameters (?a ?room-from ?room-to)" + Environment.NewLine
                        + " :precondition (and (ROOM ?room-from) (ROOM ?room-to) (" + role.ToString() + " ?a) (alive ?a)" + Environment.NewLine
                        + " (in-room ?a ?room-from) (not (died ?a)) (not (in-room ?a ?room-to))" + connected + ")" + Environment.NewLine
                        + " :effect (and (in-room ?a ?room-to) (not (in-room ?a ?room-from))))" + Environment.NewLine);

                    // Action - Nothing to do
                    actions = actions.Insert(actions.Length, Environment.NewLine + "(:action nothing-to-do" + Environment.NewLine
                        + " :parameters (?a)" + Environment.NewLine
                        + " :precondition (and (" + role.ToString() + " ?a) (alive ?a))" + Environment.NewLine
                        + " :effect (wait ?a))" + Environment.NewLine);
                }

                FileStream file = new FileStream(fileName + ".pddl", FileMode.Create, FileAccess.ReadWrite);
                StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));

                streamWriter.WriteLine("(define (domain " + domainName + ")");
                streamWriter.WriteLine("(:predicates " + predicates + ")");
                streamWriter.WriteLine(actions);
                streamWriter.WriteLine(")");

                streamWriter.Close();

                predicates = "";
                actions = "";
            }
        }

        /// <summary>
        /// Generate a new PDDL file with a problem for the specified agent, based on his beliefs.
        /// </summary>
        public void GeneratePDDLProblem (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                         WorldDynamic currentWorldState,
                                         bool killerCantCreatePlan = false)
        {
            string fileName = "";
            string problemName = "";
            string domainName = "";
            string objects = "";
            string init = "";
            string goal = "";

            fileName = agent.Key.GetRole().ToString() + "Problem";
            problemName = setting.ToString().ToLower() + "-problem";
            domainName = setting.ToString().ToLower() + "-domain";

            foreach (var location in agent.Value.GetBeliefs().GetLocationsInWorld())
            {
                objects = objects.Insert(objects.Length, location.GetName() + " ");
                init = init.Insert(init.Length, Environment.NewLine + "(ROOM " + location.GetName() + ") ");

                if (currentWorldState.GetStaticWorldPart().GetConnectionStatus())
                {
                    foreach (var connectedLocation in location.GetConnectedLocations())
                    {
                        init = init.Insert(init.Length, "(connected " + location.GetName() + " " + connectedLocation.GetName() + ")");
                    }
                }
            }

            foreach (var a in agent.Value.GetBeliefs().GetAgentsInWorld())
            {
                objects = objects.Insert(objects.Length, a.GetInfo().GetName() + " ");

                if (!(agent.Key.GetRole() == AgentRole.PLAYER) && a.GetRole().Equals(AgentRole.PLAYER))
                {
                    init = init.Insert(init.Length, Environment.NewLine + "(AGENT " + a.GetInfo().GetName() + ") ");
                }
                else if (a.GetRole().Equals(AgentRole.USUAL))
                {
                    init = init.Insert(init.Length, Environment.NewLine + "(AGENT " + a.GetInfo().GetName() + ") ");
                }
                else
                {
                    init = init.Insert(init.Length, Environment.NewLine + "(" + a.GetRole().ToString() + " " + a.GetInfo().GetName() + ") ");
                }

                switch (a.CheckStatus())
                {
                    case true:
                        init = init.Insert(init.Length, "(alive " + a.GetInfo().GetName() + ") ");
                        break;
                    case false:
                        init = init.Insert(init.Length, "(died " + a.GetInfo().GetName() + ") ");
                        break;
                }

                if (agent.Key.GetRole().Equals(AgentRole.USUAL) || agent.Key.GetRole().Equals(AgentRole.PLAYER))
                {
                    if (agent.Value.GetBeliefs().SearchAgentAmongLocations(a.GetInfo()) != null)
                    {
                        init = init.Insert(init.Length, "(in-room " + a.GetInfo().GetName() + " " +
                            agent.Value.GetBeliefs().GetLocationByName(agent.Value.GetBeliefs().SearchAgentAmongLocations(a.GetInfo()).GetName()).GetName() + ") ");

                        if (agent.Key.GetName().Equals(a.GetInfo().GetName()))
                        {
                            foreach (var loc in agent.Value.GetExploredLocations())
                            {
                                init = init.Insert(init.Length, "(explored-room " + a.GetInfo().GetName() + " " + loc.GetName() + ")");
                            }

                            if (agent.Value.GetObjectOfAngryComponent() != null && agent.Value.AngryCheck())
                            {
                                init = init.Insert(init.Length, "(angry-at " + agent.Key.GetName() + " " + agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName() + ")");
                            }
                        }
                    }
                }
                else
                {
                    init = init.Insert(init.Length, "(in-room " + a.GetInfo().GetName() + " " +
                        agent.Value.GetBeliefs().GetLocationByName(currentWorldState.SearchAgentAmongLocations(a.GetInfo()).GetName()).GetName() + ") ");
                }
            }

            if (agent.Value.GetGoal().GetGoalType().Equals(GoalTypes.STATUS))
            {
                switch (agent.Key.GetRole())
                {
                    case AgentRole.USUAL:
                        foreach (var a in agent.Value.GetGoal().GetGoalState().GetAgents())
                        {
                            if (a.Key.GetRole().Equals(AgentRole.ANTAGONIST) || a.Key.GetRole().Equals(AgentRole.ENEMY))
                            {
                                if (a.Key.GetName() != null && a.Key.GetName() != "" && a.Key.GetName() != "???")
                                {
                                    goal = goal.Insert(goal.Length, "(died " + a.Key.GetName() + ") ");
                                }
                                else if (agent.Value.GetObjectOfAngryComponent() != null && agent.Value.AngryCheck()
                                    && currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).
                                          Equals(currentWorldState.GetLocationByName(
                                             currentWorldState.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).
                                                Value.GetBeliefs().GetMyLocation().GetName())))
                                {
                                    goal = goal.Insert(goal.Length, "(died " + agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName() + ") ");
                                }
                                else
                                {
                                    if (currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).
                                        Value.CountAliveAgents() >= 2 && !agent.Value.CheckTalking())
                                    {
                                        goal = goal.Insert(goal.Length, "(talking " + agent.Key.GetName() + " "
                                            + currentWorldState.GetRandomAgentInMyLocation(agent).Key.GetName() + ") ");
                                    }
                                    else if (!agent.Value.CheckIfLocationIsExplored(agent.Value.GetMyLocation()) && !agent.Value.GetEvidenceStatus().CheckEvidence())
                                    {
                                        goal = goal.Insert(goal.Length, "(explored-room " + agent.Key.GetName() + " "
                                            + agent.Value.GetMyLocation().GetName() + ") ");
                                    }
                                    else
                                    {
                                        if (agent.Value.GetTimeToMove() == 1000) // 0
                                        {
                                            if (agent.Value.GetTargetLocation() != null)
                                            {
                                                goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                    + agent.Value.GetTargetLocation().GetName() + ") ");
                                            }
                                            else
                                            {
                                                agent.Value.SetTargetLocation(
                                                    currentWorldState.GetRandomLocationWithout(
                                                        currentWorldState.GetLocationByName(
                                                            agent.Value.GetBeliefs().GetMyLocation().GetName())).Key);

                                                goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                    + agent.Value.GetTargetLocation().GetName() + ") ");
                                            }
                                        }
                                        else if (agent.Value.GetTargetLocation() != null && agent.Value.CheckTargetLocation() 
                                            && !currentWorldState.GetStaticWorldPart().GetSetting().Equals(Setting.Detective))
                                        {
                                            goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                + agent.Value.GetTargetLocation().GetName() + ") ");
                                        }
                                        else
                                        {
                                            goal = goal.Insert(goal.Length, "(wait " + agent.Key.GetName() + " " + ") ");
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case AgentRole.PLAYER:
                        foreach (var a in agent.Value.GetGoal().GetGoalState().GetAgents())
                        {
                            if (a.Key.GetRole() == AgentRole.ANTAGONIST || a.Key.GetRole() == AgentRole.ENEMY)
                            {
                                if (a.Key.GetName() != null && a.Key.GetName() != "???" && a.Key.GetName() != "" &&
                                    agent.Value.GetBeliefs().GetMyLocation().GetName() != currentWorldState.SearchAgentAmongLocationsByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).GetName())
                                {
                                    goal = goal.Insert(goal.Length, "(died " + a.Key.GetName() + ") ");
                                }
                                else if (agent.Value.GetObjectOfAngryComponent() != null && agent.Value.AngryCheck() &&
                                    agent.Value.GetBeliefs().GetMyLocation().GetName() != currentWorldState.SearchAgentAmongLocationsByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).GetName())
                                {
                                    goal = goal.Insert(goal.Length, "(died " + agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName() + ") ");
                                }
                                else if (agent.Value.AngryCheck() && currentWorldState.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).Value.GetStatus())
                                {
                                    goal = goal.Insert(goal.Length, "(died " + agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName() + ") ");
                                }
                                else
                                {
                                    if (currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Value.CountAliveAgents() >= 2)
                                    {
                                        goal = goal.Insert(goal.Length, "(talking " + agent.Key.GetName() + " "
                                            + currentWorldState.GetRandomAgent(agent).Key.GetName() + ") ");
                                    }
                                    else if (!agent.Value.CheckIfLocationIsExplored(agent.Value.GetMyLocation()))
                                    {
                                        goal = goal.Insert(goal.Length, "(explored-room " + agent.Key.GetName() + " "
                                            + agent.Value.GetMyLocation().GetName() + ") ");
                                    }
                                    else
                                    {
                                        if (agent.Value.GetTargetLocation() != null)
                                        {
                                            goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                + agent.Value.GetTargetLocation().GetName() + ") ");
                                        }
                                        else
                                        {
                                            agent.Value.SetTargetLocation(
                                                currentWorldState.GetRandomLocationWithout(
                                                    currentWorldState.GetLocationByName(
                                                        agent.Value.GetBeliefs().GetMyLocation().GetName())).Key);

                                            goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                + agent.Value.GetTargetLocation().GetName() + ") ");
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case AgentRole.ANTAGONIST:
                        if (currentWorldState.GetStaticWorldPart().GetSetting().Equals(Setting.Detective) && currentWorldState.GetStaticWorldPart().GetStrictOrderOfVictimSelection())
                        {
                            foreach (var ag in currentWorldState.GetAgents())
                            {
                                if (ag.Key.GetRole().Equals(AgentRole.USUAL) && ag.Value.GetStatus())
                                {
                                    goal = goal.Insert(goal.Length, "(died " + ag.Key.GetName() + ") ");
                                    break;
                                }
                            }
                        }
                        else if (currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Value.CountAliveAgents() == 2 
                            && !currentWorldState.GetStaticWorldPart().GetStrictOrderOfVictimSelection())
                        {
                            foreach (var a in currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Value.GetAgents())
                            {
                                if (a.Value.GetStatus() && (a.Key.GetRole() == AgentRole.USUAL || a.Key.GetRole() == AgentRole.PLAYER))
                                {
                                    goal = goal.Insert(goal.Length, "(died " + a.Key.GetName() + ") ");
                                }
                            }
                        }
                        else if (currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Value.CountAliveAgents() == 1 && 
                            !(currentWorldState.GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge) || 
                              currentWorldState.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy) ||
                              currentWorldState.GetStaticWorldPart().GetSetting().Equals(Setting.Detective)))
                        {
                            goal = goal.Insert(goal.Length, "(in-room " + currentWorldState.GetRandomAgent(agent).Key.GetName() + " "
                                                  + currentWorldState.GetLocationByName(agent.Value.GetMyLocation().GetName()).Key.GetName()
                                                  + ") ");
                        }
                        else if (agent.Value.AngryCheck() && currentWorldState.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).Value.GetStatus() 
                            && !currentWorldState.GetStaticWorldPart().GetStrictOrderOfVictimSelection())
                        {
                            goal = goal.Insert(goal.Length, "(died " + agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName() + ") ");
                        }
                        else
                        {
                            if (agent.Value.GetTimeToMove() == 0 && currentWorldState.GetStaticWorldPart().GetConnectionStatus()  && !currentWorldState.GetStaticWorldPart().GetStrictOrderOfVictimSelection())
                            {
                                goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                   + currentWorldState.GetRandomConnectedLocation(currentWorldState.GetLocationByName(agent.Value.GetMyLocation().GetName())).Key.GetName()
                                                   + ") ");
                            }
                            else
                            {
                                goal = goal.Insert(goal.Length, "(wait " + agent.Key.GetName() + " " + ") ");
                            }
                        }
                        break;
                    case AgentRole.ENEMY:
                        if (currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Value.CountAliveAgents() >= 2
                            && currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Value.playerOrUsualIsHere())
                        {
                            foreach (var a in currentWorldState.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Value.GetAgents())
                            {
                                if (a.Value.GetStatus() && (a.Key.GetRole() == AgentRole.USUAL || a.Key.GetRole() == AgentRole.PLAYER))
                                {
                                    goal = goal.Insert(goal.Length, "(died " + a.Key.GetName() + ") ");
                                }
                            }
                        }
                        else
                        {
                            goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                + currentWorldState.GetLocationByName(currentWorldState.SearchAgentAmongLocationsByRole(AgentRole.PLAYER).GetName()).Key.GetName()
                                + ") ");
                        }
                        break;
                }
            }

            FileStream file = new FileStream(fileName + ".pddl", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));

            streamWriter.WriteLine("(define (problem " + problemName + ")");
            streamWriter.WriteLine("(:domain " + domainName + ")");
            streamWriter.WriteLine("(:objects " + objects + ")");
            streamWriter.WriteLine("(:init " + init + ")");
            streamWriter.WriteLine("(:goal (and " + goal + "))");
            streamWriter.WriteLine(")");

            streamWriter.Close();
        }
    }
}