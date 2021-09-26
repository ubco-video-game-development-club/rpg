# RPG
Explore a divided world and develop your hidden powers as you hunt for the ultimate reward in this beautiful RPG traumedy.

## How to Contribute
### Setting up your development environment
To set up the required programs to contribute code to the game, follow these instructions:
1. [Install Unity Hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe?_ga=2.44506595.613451577.1609220232-1621664689.1584733812)
2. After installing Unity Hub, install [Unity version 2020.2.1](https://unity3d.com/get-unity/download/archive)
3. Sign into Unity Hub and go to Manage License, and follow the steps to activate either a free license or your student license if you have one
4. Install [GitHub Desktop](https://desktop.github.com/)
5. Login to GitHub Desktop with your GitHub credentials
6. In GitHub Desktop, go to File > Clone repository > URL and enter `https://github.com/ubco-video-game-development-club/rpg`
7. Go back to Unity Hub, and under the Projects tab click "Add" and select your newly cloned project folder (the file location you chose in step 6)
8. Click on the RPG project in your projects list to open it for the first time
9. Install [Visual Studio Code](https://code.visualstudio.com/download)
10. Follow the [instructions to setup Visual Studio Code for Unity](https://code.visualstudio.com/docs/other/unity)

Let us know on Discord if you have any difficulties with the setup!

### Git Standards
Our team uses Feature Branches for our git flow. This means that each feature (typically described by a single Issue) should have its own branch that may be worked on by multiple collaborators. All features will be merged into the `dev` branch using Pull Requests, and each PR must be reviewed by at least 1 individual before it can be merged.

Naming conventions should follow Git standards: branches should be written in `kebab-case` and commit messages should be written in imperative form (i.e. "Update item assets" not "Updated item assets" or otherwise).

Our repository uses a `master` branch and a `dev` branch. The `master` branch should always contain stable code that represents a complete set of features. The `dev` branch contains stable code that may contain parts of in-progress features, and is used for testing different features together before they are merged to `master`.

### Uploading your code changes to the project
If you are unfamiliar with Git use, you can go through a summary of it [here](https://github.com/ubco-video-game-development-club/rpg/wiki/Using-GitHub) or talk to one of the team leaders (preferably through Discord) to help you get started. This is a basic overview of the process of submitting your code changes:
1. Ensure you are working on the branch pertaining to your current feature
2. After each set of changes, commit those changes to the branch
3. Once your feature is complete, create a new pull request for your feature and request a code review

## FAQ (Updated Sept 10th 2021)

**What is club-wide-rpg?**  
The RPG is a VGDC club project that is currently in early development. The premise is a duck-themed top-down roleplaying game which includes story-focused gameplay with a detailed character progression system. Check out the [Wiki tab](https://github.com/ubco-video-game-development-club/rpg/wiki) of the GitHub repository for more documentation, as well as the #rpg-faq channel in our [Discord server](https://discord.gg/ydXaAjQ).

**Who runs this project?**  
This project is run by several of the club executives: [Jaden Balogh](https://github.com/JadenBalogh), [Adam Collins](https://github.com/wubbadukky), [Megan Nguyen](https://github.com/lilmergo), and [Logan Parker](https://github.com/LoganParker). The repository, meetings, and tasks are all organized by this team. However, any club member is welcome to contribute (see below)!

**How can I join the team?**  
The weekly RPG meeting details can be found in #event-info on [Discord](https://discord.gg/ydXaAjQ). All members are welcome to join, regardless of experience! Please message #rpg-discussion or [Jaden Balogh](https://github.com/JadenBalogh) about the roles you are interested in (see below).

**What parts of the game are currently being worked on?**  
As we move into the 2021W session, we're excited to start focusing development on the game's content and storyline. This will require art, sounds, dialogue, level design, game scripting and more, so ANY club member is more than welcome to join the game's development regardless of previous experience level.

These are types of roles that you can expect to see for this project, however if you're at all interested in joining the team, let us know your interests and we'll find a position for you! You can work one or many of these areas:
 - **Level Designer**: Creating the layouts of landscapes and other in-game areas, as well as NPC and enemy placement, and working on quest design. No prior experience necessary.
 - **Story Designer**: This a heavily creative position which will involve creating storylines and world lore to fit within the context of the game world. You can work with or double as a quest/level designer to decide exactly where and how the player will interact with these stories. No prior experience needed.
 - **Writer**: This is a pure writing position focused on using our custom-made dialogue tools to bring the game's stories to life by writing NPC dialogue, voice lines and readable in-game text. Good eye for grammar+spelling highly encouraged.
 - **Systems Designer**: The RPG will need loads of items, classes, monsters, abilities and more. This position is for creating, balancing and implementing any such content for use in the game's quests and general gameplay. No experience needed.
 - **Sound Designer**: Recording and mastering sound effects for all parts of the game; ambience, combat, UI, etc. Some experience with sound design recommended but not required.
 - **Composer**: Creating music tracks to bring the game world to life from the main menu to combat tracks. Some experience with music composition highly recommended.
 - **World Artist**: This position involves creating 2D sprites and tiles to build terrain, buildings, foliage, and other objects that will make up the majority of the visible game world. Prior 2D art experience recommended.
 - **Character Artist**: This role involves 2D art and animation for the player, enemies, NPCs and other dynamic parts of the game world. Prior 2D animation experience recommended.
 - **UI Artist**: This role involves designing and creating 2D assets for use in the User Interface, such as healthbars or menu buttons, with a focus on optimizing the user experience. 2D art experience recommended, UI/UX design knowledge a plus.
 - **UI Scripter**: This is a programming role involved in developing menus and other UI functionality. Will double as or work with a UI artist to develop clear and usable User Interfaces in Unity. Some general programming experience *required*.
 - **Gameplay Scripter**: This is a more in-depth programming role which involves developing and improving in-game systems, and scripting specific elements such as special monster or ability logic. Some Unity coding experience *required*.
