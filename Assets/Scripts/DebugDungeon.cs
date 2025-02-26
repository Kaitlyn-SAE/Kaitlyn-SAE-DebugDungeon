using UnityEngine;
using System;

public class DebugDungeon : MonoBehaviour
{
    // Player Stats Structure
    struct Player // Defines a structure named Player.
    {
        public string characterClass;
        public int level;
        public float strength;
        public int armour;
        public int dodge;
        public int magic;
        public int health;
        public float armourHealth;
        public int attackDamage;
        public int experience;
    }

    // Enemy Stats Structure
    struct Enemy // Essentially just a repeat of the Player struct but for enemies
    {
        public string enemyClass;
        public int level;
        public float strength;
        public float armour;
        public int dodge;
        public int magic;
        public int health;
        public int attackDamage;
        public int experienceReward; // An unt for how much exp the enemy will reward for being defeated
    }

    Player player; // player variable called player to be called upon
    Enemy enemy; // enemy version of the above ^^

    bool classSelected = false; // Bools for class selection and other things \/
    bool isPlayerTurn = false;
    bool isLevelingUp = false;
    bool isBossBattle = false;
    bool gameWon = false;



    int levelUpCost = 20; // Base cost to level up
    int xpToLevel = 20; // How much it costs to get to the next level
    bool isLevelingLimited = false;

    System.Random random = new System.Random(); // Creates a new instance of the Random class for generating random numbers

    void Start() // Start is called before the first frame update
    {
        DisplayClassOptions(); // Calls on the DisplayClassOptions code to present the class choices
    }

    void DisplayClassOptions() // Easier way of having the game output the class choices than doing single lines of debug.log("blahblahblah");
    {
        Debug.Log("Choose your class:"); // Prints the classes (self explanatory)
        Debug.Log("1: Barbarian");
        Debug.Log("2: Paladin");
        Debug.Log("3: Rogue");
        Debug.Log("4: Mage");
    }

    void Update()  // Update is called once per frame
    {
        if (!classSelected) // Checking the class selected bool
        {
            GetPlayerClassInput(); // handles player class input (essentially calling upon input for a class)
        }
        else if (isLevelingUp) // Is a player leveling up?
        {
            GetLevelUpInput(); // handles the leveling up input (which is the section where players choose a stat to increase)
        }
        else if (isBossBattle) // is a boss battle occuring? 
        {
            if (isPlayerTurn && !gameWon) // Checks for player turn and whether the game not is won
            {
                GetPlayerTurnInput(); // player turn input.
            }
        }
        else if (isPlayerTurn && !gameWon && !isLevelingLimited) // is it the player's turn? has the game has not been won? Is leveling limited?
        {
            GetPlayerTurnInput(); // player turn input.
        }
    }

    void GetPlayerClassInput() // This is what allows players to choose a class
    {
        //Store Input
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Has key "1" been pressed?
        {
            InitializePlayer("Barbarian"); //   Starts the player intialization as a barbarian
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // All of these are gonna be the same as the example above but for different classes 
        {
            InitializePlayer("Paladin");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InitializePlayer("Rogue");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            InitializePlayer("Mage");
        }
    }

    void InitializePlayer(string chosenClass) // Defines InitializePlayer to which initalizes the player character
    {
        // These 3 set the player class to their chosen class, their level to start at one and their starting health at 100
        player.characterClass = chosenClass;
        player.level = 1;
        player.health = 100;

        // Switch case below if for the randomising of initial player stats
        switch (chosenClass)
        {
            case "Barbarian": // Barbarian Case
                player.strength = UnityEngine.Random.Range(5, 20) + 20; // Random strength value with a bonus
                player.armour = UnityEngine.Random.Range(1, 10);
                player.dodge = UnityEngine.Random.Range(1, 10);
                player.magic = 0; // Mage is the only class with access to the magic stat
                break;
            case "Paladin":
                player.strength = UnityEngine.Random.Range(1, 10) + 10;
                player.armour = UnityEngine.Random.Range(5, 20) + 10; // Rand armour value with a bonus
                player.dodge = UnityEngine.Random.Range(1, 10) + 5;
                player.magic = 0;
                break;
            case "Rogue":
                player.strength = UnityEngine.Random.Range(1, 10) + 8;
                player.armour = UnityEngine.Random.Range(1, 10) + 3;
                player.dodge = UnityEngine.Random.Range(5, 25) + 15; // Rand Dodge value with a bonus
                player.magic = 0;
                break;
            case "Mage": // Mage is the only class with the magic stat
                player.strength = UnityEngine.Random.Range(1, 10) + 5;
                player.armour = UnityEngine.Random.Range(1, 10) + 2;
                player.dodge = UnityEngine.Random.Range(1, 10) + 3;
                player.magic = UnityEngine.Random.Range(5, 20) + 15; // Rand Magic value with a bonus
                break;
        }
        Debug.Log("You have chosen " + player.characterClass + "!"); // Shows what class has been chosen
        Debug.Log("Strength: " + player.strength + " Armor: " + player.armour + " Dodge: " + player.dodge + " Magic: " + player.magic + " Health: " + player.health); // Shows the inital player stats prior to conversion

        classSelected = true; // Player has now chosen a class and the game can continue
        ConvertStats(); // Calls on convertstats stuff to begin the conversion of these initial stats into gameplay values (like strength to attack damage, etc)
        StartCombat(); // Calls on the start combat code (Note: Consider making this occur after players do some sort of confirmation of their stats - maybe an option to reselect class if they didn't like the stats?)
                       // This would stop the kinda flooding of the console with messages making it somewhat hard to read things nicely.
    }

    void ConvertStats() // Converts player stats into gameplay values
    {

        player.attackDamage = (int)(player.strength * 1.45); // player strength multiplied by 1.45 is their attack damage 
        player.armourHealth = (int)(player.armour * 5.45); // Player armour health is armour multiplied by 5.45 which gives their armour health

        Debug.Log("Attack Damage = " + player.strength + " * 1.45 = " + player.attackDamage); // Shows the conversion in the console
        Debug.Log("Armor Health = " + player.armour + " * 5.45 = " + player.armourHealth); // same as above ^

        float dodgePercent = (float)(player.dodge * 1.25); // dodge stat * 1.25 is player dodge %
        float reflect = (float)(player.magic * 1.25); // magic stat * 1.25 is player deflect %
        Debug.Log("Dodge Chance = " + dodgePercent + "%"); // Puts the % chance to dodge in the console
        Debug.Log("Deflect Chance = " + reflect + "%"); // ^ pretty much the same but for magic
    }

    void StartCombat() // This where the combat begins 
    {
        if (player.level >= 5 && !isBossBattle) // Essentially checking to see if players level is above or equal to 5 or whether it is not a boss battle (it will then call for it to be a boss battle if they are above or equal to level 5)
        {
            StartBossBattle(); // Starts the boss battle stuff
            return; // Exits start combat so a normal non boss enemy doesnt spawn
        }
        if (gameWon) { return; } // Has the game been won? Well if it has we dont want another enemy to spawn

        GenerateEnemy(player.level); // Calls for the code handling the generation of an enemy (player.level) making the enemy match the level of the player
        Debug.Log("A " + enemy.enemyClass + " appears!");
        Debug.Log("Enemy Stats: Strength: " + enemy.strength + " Armor: " + enemy.armour + " Dodge: " + enemy.dodge + " Magic: " + enemy.magic + " Health: " + enemy.health + " Attack Damage: " + enemy.attackDamage + " Armor Health: " + enemy.armour); // Enemy stats shown in console
        isPlayerTurn = true; // Sets the playerturn bool to true to begin the combat.
        DisplayPlayerTurnOptions(); // Calls for the player turn options code (essentially just asking for the player to attack)(more advanced we could add more player options, like using items or something)
    }

    void GenerateEnemy(int levelScale) // Defining the generate enemy code, the level scale is so that we have progressively harder enemies
    {
        enemy = new Enemy(); // Creates a new enemy, defined eariler in the code "Enemy enemy;"
        string[] enemyNames = { "Goblin", "Orc", "Skeleton", "Zombie" }; // Array of enemy names
        System.Random random = new System.Random(); // New instance of random class for generating the random stats for the enemy 
        int index = random.Next(enemyNames.Length); // random index within the range of the enemy names array.
        enemy.enemyClass = enemyNames[index]; // random enemy class name from the array to the enemy's enemyClass field.

        //This is for getting all the stats of the enemies
        enemy.level = levelScale; // Enemy stats scale with player level
        enemy.strength = UnityEngine.Random.Range(3 * levelScale, 7 * levelScale); // Rand strength value within a range
        enemy.armour = UnityEngine.Random.Range(1 * levelScale, 4 * levelScale);
        enemy.dodge = UnityEngine.Random.Range(0, 3 * levelScale);
        enemy.magic = 0; // Only skeletons are capable of doing magic - their magic code is a lil below 
        enemy.health = UnityEngine.Random.Range(20 * levelScale, 45 * levelScale); // Rand health value within range
        enemy.attackDamage = (int)enemy.strength; // Converting their strength into attack damage
        enemy.experienceReward = UnityEngine.Random.Range(1, 8 * levelScale); // Random Scaling xp rewards for defeating them

        //The mentioned skeleton magic code
        if (enemy.enemyClass == "Skeleton") // Checks if the enemy is a skele
        {
            enemy.magic = UnityEngine.Random.Range(1, 8 * levelScale); // Rand magic within a range scaling with level
        }

        Debug.Log("Enemy Level: " + levelScale); // Shows the enemy level in the console
    }

    void DisplayPlayerTurnOptions() // Code for showing the player options (see previous mention of this code for possible improvements)
    {
        Debug.Log("Your turn, press 5 to attack"); // Puts prompt to attack into console
    }

    void GetPlayerTurnInput() // Code for getting the actual input for attacking
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) // Has '5' been pressed? if yes \/
        {
            PlayerAttack(); // If they pressed 5 - call player attack code
        }
    }

    void PlayerAttack() // Code for player attack
    {
        int dodgeChance = random.Next(0, 100); // Rand number to see if enemy will dodge

        if (dodgeChance <= enemy.dodge) // Is the rand number within the enemy dodge range
        {
            Debug.Log("Enemy dodged!"); // If it was they dodge
            EnemyTurn(); // Starts the enemy turn
        }
        else // Enemy did not dodge
        {
            // Calculates the damage and applies it to enemy
            int damage = player.attackDamage; // Assigning player attack damage into a local variable 
            Debug.Log("You attack the " + enemy.enemyClass + " for " + damage + " damage!");

            if (enemy.armour > 0) // Checking for enenmy armour being above 0 so that we have to destroy their armour before their health
            {
                enemy.armour -= damage; // Enemy armour - damage
                Debug.Log("You Dealt " + damage + " damage to the " + enemy.enemyClass + "'s Armor!"); // output that armor dmg was dealt
                Debug.Log("Enemy Armor: " + enemy.armour); // Show how much armour they have left (an improvement here would be checking if it is <= 0, if so it doesn't print a negative number)

            }
            else // No armour? Damage health
            {
                enemy.health -= damage; // Enemy health - damage
                Debug.Log("You Dealt " + damage + " damage to the " + enemy.enemyClass + "'s Health!"); // output that health dmg was dealt
                Debug.Log("Enemy health: " + enemy.health); // Show how much health enemy has left (same improvement as armour would be useful)
            }
        }
        if (enemy.health <= 0 && !gameWon && !isBossBattle) // Check if enemy health is <= 0, game [IS NOT] won and it [IS NOT] a boss fight
        {
            Debug.Log("Enemy " + enemy.enemyClass + " was defeated!");
            AwardExperience(); // Calls for the award experience code
        }
        else if (enemy.health <= 0 && isBossBattle) // Check if enemy health is <= 0 and it [IS] a boss fight
        {
            Debug.Log("You sigh a breath of relief, you defeated the powerful " + enemy.enemyClass + ". You Win!"); // You won the game (look how very good you are) 
            GameEnded(); // Calls on the game ended code so that we don't have new enemies spawn
        }
        else // Enemy health is above 0 so its their turn now
        {
            EnemyTurn(); // Calls for Enemy turn code
        }
    }

    void EnemyTurn() // Code for executing enemy turn
    {
        int dodgeChance = random.Next(0, 100); // Rand number to see if player will dodge

        if (dodgeChance <= player.dodge) // Is the rand number within the player dodge range?
        {
            Debug.Log("You dodged!");
            isPlayerTurn = true; // Sets it back to player turn
            DisplayPlayerTurnOptions(); // Calls for the player turn option code
        }
        else // Player didn't dodge
        {
            int damage = enemy.attackDamage; // Enemy damage into a local variable
            Debug.Log("The " + enemy.enemyClass + " attacks you for " + damage + " damage!");

            // \/ Checking for whether the attack is gonna be reflected (exclusive to mage)
            bool didReflect = false; // Checking if the damage is going to be reflected on the enemy 
            if ((player.characterClass == "Mage" || enemy.enemyClass == "Skeleton") && player.magic > 0) // Checking magic stat
            {
                int reflectChance = random.Next(0, 100); // Rand number to see if player will reflect
                if (reflectChance <= player.magic) // If reflectchance is less than or equal to player magic \/
                {
                    Debug.Log("The attack was reflected!");
                    enemy.health -= damage; // Enemy takes the damage
                    Debug.Log("Enemy health is now " + enemy.health);
                    didReflect = true;
                    if (enemy.health <= 0) // checks to see their health
                    {
                        Debug.Log(enemy.enemyClass + " was defeated by the reflected damage!"); // Shows that the enemy was defeated by the reflected damage
                        AwardExperience(); //  Calls for the award experience code
                        return; // ends turn
                    }

                }
            }
            if (!didReflect) //If damage wasn't reflected \/
            {
                if (player.armourHealth > 0) // Checking for player armour being below 0
               {
                    player.armourHealth -= damage; // player armour - damage

                    if (player.armourHealth < 0)
                    {
                        Debug.Log(damage + " damage dealt to armour"); // message to test output to armor
                        player.health += (int)player.armourHealth; //remaining damage is negative, so add it to health
                        player.armourHealth = 0; // set to 0 after dmg is applied so it doesnt become negative
                    }

                }
                else
                {
                    player.health -= damage;
                }
                Debug.Log("Your health: " + player.health);
                Debug.Log("Your armor health is now: " + player.armourHealth);
            }

        }

        if (player.health <= 0 && !gameWon) // Is player health less than or equal to 0. has game not been won?
        {
            Debug.Log("You have been defeated!");
            Debug.Log("Game Over!");
            isBossBattle = false; // Stops the boss battle
            gameWon = true; // Stops game from being played
            isPlayerTurn = false; // Stops player turn from occuring
        }
        else // player health above 0
        {
            isPlayerTurn = true;
            DisplayPlayerTurnOptions(); // Go back to player turn option code
        }
    }

    void AwardExperience() // This is where player is given xp
    {
        Debug.Log("You gained " + enemy.experienceReward + " experience!");
        player.experience += enemy.experienceReward; // add enemy xp to player rewarded xp

        if (player.experience >= xpToLevel && !gameWon && !isBossBattle) // is player experience greater than or equal to the xp required to level up? checks they haven't already won the game and if it is not a boss battle
        {
            LevelUp(); // If they have met the required to level up and they haven't one - call the level up code
        }
        else
        {
            StartCombat(); // If they haven't met the thresehold start combat again
        }
    }

    void LevelUp() // Level up code
    {
        isLevelingUp = true; // Set levelingup bool to true
        isLevelingLimited = true;
        levelUpCost = (int)(levelUpCost * 1.50f); // Increase the leveling scaling
        xpToLevel = levelUpCost;
        player.health = (int)(player.health * 1.25f);
        player.armourHealth = (int)(player.armourHealth * 1.35f);

        player.level++; // Increase player level by 1
        Debug.Log("You leveled up! You're now level: " + player.level);
        Debug.Log("Health restored by 25%!");
        Debug.Log("Armor restored by 35%!");
        Debug.Log("Health: " + player.health + " Armor: " + player.armourHealth);

        DisplayLevelUpOptions(); // Calls display options code \/
    }

    void DisplayLevelUpOptions() // Code for displaying the stat options for leveling up
    {
        Debug.Log("Choose a stat to increase:");

        if (player.characterClass == "Mage") // checking if player is mage
        {
            Debug.Log("1: Strength");
            Debug.Log("2: Armor");
            Debug.Log("3: Dodge");
            Debug.Log("4: Magic");

        }
        else // player is not a mage
        {
            Debug.Log("1: Strength");
            Debug.Log("2: Armor");
            Debug.Log("3: Dodge");
        }
    }

    void GetLevelUpInput() // Getting the input for chosing what stat is being increased
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            IncreaseStat(1); // the (1), (2), etc represent the case that is chosen in the increase stat section
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            IncreaseStat(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            IncreaseStat(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && player.characterClass == "Mage") // Checking if '4' is prsssed and player class is a mage
        {
            IncreaseStat(4);
        }
    }

    void IncreaseStat(int statChoice) // Code for increasing the stat chosen by player
    {
        switch (statChoice) // Switch that is for the stat the player chooses to upgrade
        {
            case 1:
                player.strength += 1; // increase player strength stat by 1
                Debug.Log("Strength increased by 1.");
                break;
            case 2:
                player.armour += 1;
                Debug.Log("Armor increased by 1.");
                break;
            case 3:
                player.dodge += 1;
                Debug.Log("Dodge increased by 1.");
                break;
            case 4: // This one checks for both the '4' key press and if player class is mage \/
                if (player.characterClass == "Mage")
                {
                    player.magic += 1;
                    Debug.Log("Magic increased by 1.");
                }
                break;
        }

        ConvertStats(); // Calls back to convert stats code to to calculations again based on these new stats
        isLevelingUp = false; // Changes the leveling up bool to false 
        isLevelingLimited = false;
        StartCombat(); // Begin combat again (again as mentioned prior a confirmation of stats or something to happen which would require player input prior to starting this combat again would make the game more readable)
    }

    void StartBossBattle() // Boss battle Code
    {
        isBossBattle = true; // sets boss battle bool to true
        GenerateBoss(); // Calls on generate boss code
        Debug.Log("A powerful " + enemy.enemyClass + " appears!");
        Debug.Log("Boss Stats: Strength: " + enemy.strength + " Armor: " + enemy.armour + " Dodge: " + enemy.dodge + " Magic: " + enemy.magic + " Health: " + enemy.health + " Attack Damage: " + enemy.attackDamage + " Armor Health: " + enemy.armour * 5);
        isPlayerTurn = true; // Starts on player turn
        DisplayPlayerTurnOptions();
    }

    void GenerateBoss() // Boss Generation stats
    {
        enemy = new Enemy();
        string[] enemyNames = { "Goblin", "Orc", "Skeleton", "Zombie" };
        System.Random random = new System.Random();
        int index = random.Next(enemyNames.Length);
        enemy.enemyClass = enemyNames[index];

        //Boss stats
        enemy.level = player.level; // boss will be the same level as player
        enemy.strength = UnityEngine.Random.Range(7 * player.level, 10 * player.level);  // boss stats, high rand number range than regular enemies to make it more challenging
        enemy.armour = UnityEngine.Random.Range(3 * player.level, 5 * player.level);
        enemy.dodge = UnityEngine.Random.Range(0, 3 * player.level);
        enemy.magic = 0;
        enemy.health = UnityEngine.Random.Range(75, 100);
        enemy.attackDamage = (int)enemy.strength * 2;

        if (enemy.enemyClass == "Skeleton") //Skeleton boss has magic
        {
            enemy.magic = UnityEngine.Random.Range(3, 8 * player.level);
        }
        else // Not a skeleton? No magic
        {
            enemy.magic = 0;
        }
    }
    void GameEnded() // game ended function
    {
        gameWon = true;
        isBossBattle = false;
        Debug.Log("You Win!");
    }
}


/* Known issues:
 * 1. Occasionally we have the enemy attack loop essentially just skipping the player turn which most of the time seems to happen in the boss fight.
 * 2. Skeleton reflection of damage does not work.
 * 3. When the boss gets killed by their own reflected damage, the game spawns another regular enemy - once this regular enemy is defeated we win the game. (Line 353) [SOLVED - Changed line 349 to include !isBossBattle]
 * 4. Pretty much all of the time when player's dodge the enemy attack the prompt to attack by pressing 5 is printed twice.
 * 5. When you beat an enemy you get your health and armour health increased by 25 and 35% respectively, however when you choose to increase your amour stat it will reset your armour
 * to whatever the new armourhealth value it generates is. (e.g. barbarian fights starting with 20 armour health, they dodge enemy attack and then kill the enemy increasing it by 35%
 * to 27, they then level up and choose to increase their armour which then re-does the calculation and brings their armour health down. this can also happen the opposite way where 
 * the player can exploit the fact they will have their armour value set to the new calculation.)
 */
/* Possible improvements:
 * 1. A proper UI that is done in unity and different buttons using the textmeshpro buttons, etc.
 * 2. Better scaling of stats - essentially in things like dodge when you increase your stat the percentage is only increasing by 1 which makes a negligible difference when you  go from 13% dodge to 14%. [COMPLETED - Added multiplier like armorhealth and attackdmg]
 * 3. More player options when its their turn - think items, running away from the fight, etc
 * 4. Player movement? - Something like giving prompts on where they can go after their fights (like an exploration mechanic)
 * 5. Slowed down gameplay with more halts to stop information from flooding the console log (This is something that should be implemented with the UI ideally)
 */
