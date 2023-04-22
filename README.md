Designed for PCs running Windows OS.

This program is designed to generate a narrative, the output of which occurs in the form of a story graph. This graph can be visualized. Also is possible to create an interactive story based on it for the Twine platform. In both cases, the output of the program occurs in the form of creating files that contain instructions for the corresponding systems. These instructions are description of the generated story graph in DOT language, intended for visualization using the Graphviz program, and transcoding of the graph into an interactive story format in the form of an HTML file intended for the Twine system.

 ## Python 3

To run this program, you must have Python 3 on your computer, added to the system variable. If you do not know whether you have this software installed or not, then run the command line (cmd.exe) and type "python". As a result, the command line will display the version of Python that is installed on the system, or will display an error if it is missing.

To install Python 3, go to the official website and download the appropriate version (https://www.python.org/downloads/windows/). If you have Windows 10 and above, it will be enough to download the latest version by clicking on "Latest Python 3 Release". At the bottom of the page that opens, there will be links to install versions for 32-bit or 64-bit systems, as a package or as an installer. Choose the most convenient way and click on the corresponding link.

If you have downloaded the installer, run it by double-clicking on python-3.***.exe. On the first screen, check the box “Add Python 3.# to PATH” and click “Install Now” (or “Customize Installation” if you want to choose the location and set of tools to install).

Then you should check if Python was added to the PATH variable, or if you installed it another way, add it now. To do this, open the "System" window, in it click "Advanced system settings". Then open the "System Properties". In the "Advanced" tab, click "Environment Variables". In the window that opens, select Path with a single click and then click "Edit". In the new window that opens, you must add the path to the Python interpreter. Enter the address of the directory where you installed it. This step differs for everyone, but in general it looks like: "C:\Users\UserName\AppData\Local\Programs\Python\Python3". Click "Create" and add 2 paths. To the "Python3" and "Python3\Scripts\" folders.

## Program installation

Download the RAR archive and extract it to a folder of your choice. This will be the working directory of the program, in which it will create and delete some files necessary for its work. Therefore, we recommend extracting the archive into an empty folder.

## Program launch

This program is a portable application. To run it, double-click on the file called NarrativeGenerator.exe and wait for the graphical interface to appear.

## Program settings

The first step in setting up is to select the setting in which the narrative will be generated. In this version of the program, there are two options to choose: the Fantasy setting and the Detective setting. The Fantasy setting is divided into two sub-settings: Dragon Age and Generic Fantasy. The choice of setting affects the sets of used agents, locations and actions available to agents, also different settings use their own individual mechanics. The Dragon Age setting simulates the passage of the plot in the same name game, representing the main decisions made by the player and demonstrating the variety of ways to go through this plot. The Generic Fantasy setting serves to represent the average fantasy, as a result of which all the above features of various settings are expressed in a slightly more vaguely averaged style. The Detective setting is based on Agatha Christie's story "Ten Little Indians", representing the classic detective story.

Then it is necessary to choose the type of agent's goals. In this version of the program, only one option is available for selection - based on the status of agents (i.e., eliminate enemy agents). This choice defines a set of conditions for each agent, upon reaching which it is considered that the goal (final) state of the story has been reached and further generation of this branch of the story is terminated. The "type of goals based on the status of the agents" means that the purpose of the agents will be to neutralize the "enemy team".

Next step: general system settings. Most of them are related to setting the randomization of various parameters. Therefore, the first possible option is to specify the seed for generation. It is also possible to specify the maximum number of nodes in the story graph, after reaching which the generation will stop, and, if the narrative is not built, the system will offer to start the generation again (restart). Then there are the following items:

* Random connection of locations. Not available with the Dragon Age setting. By default, all locations are initially assigned links with other locations. When this setting is enabled, links will be established instead randomly.

* Random events. Available only when choosing the Dragon Age setting. When this setting is activated, additional locations and agents are added to the story, for simulating random encounters that occur when moving between the main locations.

* Random battle results. By default, various aggressive actions always end in success for the agent who performs them. When this setting is activated, the result of such an action is determined randomly.

* Random distribution of initiative. Agents perform actions in turn, according to their own initiative parameter. By default, the order of their action is strictly specified. When this setting is activated, the agent's initiative is distributed randomly, i.e. the order in which they perform actions will be determined randomly.

* Hide actions like "Do nothing". In some states of the storyworld, the agent may decide not to take action, i.e. skip a turn. Technically, this is create in the form of "empty actions" that are displayed in the final result. When this setting is activated, "empty actions" will be removed from the result.

The next block of settings is the generated story settings:

* It is possible to find evidence - available only when choosing the Detective setting. When this setting is activated, agents will be able to search locations to find evidences. If successful, they will know which of the agents is the killer (similarly, the player receives this opportunity). In the field on the right, it is necessary to enter the percentage probability for the success of this type of action (from 0 to 100).

* The protagonist must survive - when this setting is activated, the system guarantees that the protagonist (player) will be protected from the aggressive actions of other agents for the specified period. The duration of the protection can be specified in the field below. If this field is not filled (do not change the value 0), then the protection will be permanent.

* The antagonist must survive - when this setting is activated, the system guarantees that the antagonist will be protected from the aggressive actions of other agents and the player for the specified period. The duration of the protection can be specified in the field below. If this field is not filled (do not change the value 0), then the protection will be permanent.

The following three settings are available only when the Detective setting is selected:

* Unique ways to kill - when this setting is activated, each "Kill" action will receive its own unique description, which will be reflected in the resulting files.

* Strict order of victim selection - when this setting is activated, the antagonist agent will change his behavior, and will try to "kill" other agents not in the most efficient way, but in a certain order.

* Each agent has unique goals - when this setting is activated, the goal states of each agent are modified. Their type does not change, but the target of the status change ("kill") is not strictly the agents of the enemy team (in this case, the antagonist agent), but randomly selected agents.

The last block of settings allows more accurately determine the behavior of agents. These settings are divided in pairs: those on the top left side affect the antagonist agent, those on the right side affect ordinary enemy agents, those on the bottom left side affect the behavior of the player representing agent, those on the right side affect all ordinary agents. These settings are designed in such a way that they add or remove certain actions as available to specific agents, modifying their behavior script in such a way that they start (or stop) using these actions and can plan based on them.

* Talkative - when this setting is activated, "conversational" actions become available to the specified agents, i.e. they have the opportunity to communicate with other agents.

* Cunning - when this setting is activated, actions such as luring other agents become available to the specified agents.

* Peaceful - when this setting is activated, aggressive actions become inaccessible to the specified agents.

* Silent - when this setting is activated, the specified agents lose the ability to perform conversational actions.

* Aggressive - when this setting is activated, aggressive actions become available to the specified agents.

* Cowardly - when this setting is activated, the actions used by agents in the "scared" state (for example, run away) become available to the specified agents, respectively, such behavior will be activated (by default, the behavior of agents in the "scared" state does not differ significantly from the standard one).

The final stage of the configuration is to specify the path to the directory where the program will have to save the resulting files. To do this, use the "Select Path" button.

## Settings presets

Should be taken into account that most of these settings are used to generate a narrative that is more in line with the user's desires, i.e. more flexible and varied, but it are not required by default. On the contrary, when the more complex the narrative needs to be generated, with a large number of agents, locations, actions, and various additional conditions and mechanics, then the more time it will take, and the greater the likelihood of various errors.

For example, protection of the agent representing the player by the system leads to a significant increase the branching in story graph, and if both the protagonist and the antagonist are permanently protected by the system, then the generation cannot be completed in principle, if in all goal states one of them must be "dead". In this regard, below are the basic standard settings presets, in which the generation is the fastest and accurately:

* Fantasy - Dragon Age - kill the antagonist / all enemies - hide NTD - cunning, aggressive
* Fantasy - Generic fantasy - kill the antagonist / all enemies - random battles resultes - hide NTD - protagonist must survive - cunning, aggressive
* Detective - kill the antagonist / all enemies - evidence can be found 100% - cunning, aggressive

## Program work

When the setting is completed, it is necessary to press the "Start" button. The program will start its work.

To start the generation process, some of the settings must be specified mandatory. These include the choice of setting, the choice of the type of agent's goals and the selecting of the path for outputting the resulting files. If something from this list is not performed, then the "Start" button will not be available for clicking.

In the process of work, the program will inform about its current state in a special window in the lower left corner. Also, when selecting settings, this window will display brief descriptions of each of them.

Also, during the generation process, may occur events that require the user's attention. In these cases, the program will notify about them using pop-up windows. Such events include, for example: error notifications, a request to restart in case of unsuccessful generation, and in case of successful generation, at its end, a clarification of whether it is required to generate the resulting file of a certain type, or to proceed to the next stage.

## Visualization (Graphviz)

To visualize the story graph, we recommend use the Graphviz program. It is available both online and as an installable program. Various online versions contain a significant number of restrictions, including restrictions on the size of the graph being built, on its scalability, and others. The portable version contains much fewer of them, so it is better to use it.

To do this, download the version that suits you from the official website (https://graphviz.org/download/).

After installation, in the directory with the program, in the sub-directory bin, you can place a file in DOT format and visualize it using the command line. To do this, you need to call it in this sub-directory, for example, by entering "cmd" in the Explorer address bar, or by changing the active directory in the command line itself. Then, at the command prompt, enter the following: "dot -Tpng -O [FILENAME].dot". The file name must be specified corresponding to the DOT file that you want to visualize. In the case of the story graph visualization, the file name is "newStoryGraph".

## Interactive story run (Twine)

To run an interactive story based on a generated story graph, you must use the Twine framework. It is also available both online and as an installable program. In this case, there is no significant difference between them, so it will be enough to use the online service.

To use Twine, you need to go to https://twinery.org/ and select "Use in your browser". In the editor that opens, for adding the generated story, click on the "Library" menu item, then on the "Import" item, after which a new interface element will pop up in the lower right corner of the screen, offer you to select a file to upload. Click on the corresponding button, and in the dialog box that opens, select the required file. To run a story, select it with a single click, then select the "Build" menu item and click "Play".

## Documentation

Documentation in html format was automatically generated for the program using the Doxygen tool. It is also in the archive, in a folder called Autodocumentation HTML. To view it, it is necessary to find a file named Index in this folder and open it.
