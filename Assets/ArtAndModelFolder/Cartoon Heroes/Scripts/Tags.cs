public class Axis
{
    public const string VERTICAL_AXIS = "Vertical";
    public const string HORIZONTAL_AXIS = "Horizontal";
}

public class Tags
{
    public const string PLAYER_TAG = "Player";
    public const string ENEMY_TAG = "Enemy";
    public const string WALL_TAG = "Wall";
    public const string FLOOR_TAG = "Floor";
    public const string PILLAR_TAG = "Pillar";
    public const string STAIR_TAG = "Stair";
    public const string DOOR_TAG = "Door";
    public const string MIRROR_TAG = "RecolettaMirror";
    public const string THERABOX_TAG = "TheraCodeBox";
    public const string ASIMANAS_CODEX = "AsimanaCodex";
    public const string WallBreakSpell_TAG = "WallBreakSpell";
    public const string TheraCode_TAG = "TheraCode";
    public const string PlayerRange_TAG = "PlayerRange";
    public const string KaireTrigger_TAG = "KaireSpawnPoint";
    public const string Kaire_TAG = "Kaire";
    public const string Finish_TAG = "Finish";
    public const string FINALSEQUENCE_TAG = "FinalSequence";
    public const string HEALTH_TAG = "HealthPotion";
    public const string THERA_TAG = "Thera";
    public const string Maze_TAG = "Maze";
    public const string DeadZone = "DeadZone";
}

public class AnimationTags
{
    public const string WALK_PARAMETER = "isWalk";
    public const string BLOCK_PARAMETER = "isBlock";
    public const string ATTACK_TRIGGER1 = "Attack1";
    public const string ATTACK_TRIGGER2 = "Attack2";
    public const string ATTACK_TRIGGER3 = "Attack3";
    public const string Jump_TRIGGER4 = "Jump";
    public const string Shield_TRIGGER4 = "Defend";
}

public class GoblinAnimationTags
{
    public const string IDLE_PARAMETER = "Idle";
    public const string WALK_PARAMETER = "Walk";
    public const string RUN_PARAMETER = "Run";
    public const string SNATCH_PARAMETER = "Snatch";
    public const string JUMP_PARAMETER = "Jump";
    public const string DEAD_PARAMETER = "Dead";
}

public class MapPoints
{
    public const string PointC = "PointC";
    public const string PointCExitUp = "PointCExitUpstair";
    public const string PointCExitDown = "PointCExitDownStair";
    public const string RecolettaMirror = "RecolettaMirror";
    public const string AlkemannaHotspot = "AlkemannaHotspot";
    public const string AlkemannaHotspotSwitch = "AlkemmanaHotspotSwitch";
    public const string PowerCircuits = "PowerCircuit";
    public const string AsimanaCodex_trigger = "AsimanaCodexTrigger";
}

public class WeekTwo
{
    public const string PointA = "PointA";
    public const string StarDevice = "StarDevice";
    public const string BinaryCircle = "BinaryCircle";
    public const string AdacodeCrytex = "AdacodeCrypt";
    public const string OrbsOfKinesis = "OrbsOfKinesis";
    public const string OrbsOfKinesisMiniGame = "OrbsOfKinesisMiniGame";
    public const string AsimanaCryptex = "AsimanaCryptex";
    public const string MiniGamePlatform = "MiniGamePlatform";
    public const string OrbsOfKinesisTwo = "OrbsOfKinesisTwo";
    public const string OrbsOfKinesisThree = "OrbsOfKinesisThree";
    public const string SubmitOrb = "SubmitOrb";
    public const string ShadowBlast = "ShadowBlast";
    public const string ShadowBlastWarning = "ShadowBlastWarning";
    public const string KrakepedeWarning = "KrakepedeWarning";
    public const string KaireFightingWarning = "KaireFightingWarning";
    public const string MonsterEmergence = "MonsterEmergence";
}

public class MapPointsWeekThree
{
    public const string DisableForceField = "DisableForceField";
    public const string ClawsRealPower = "ClawsRealPower";
    public const string StripMagicfromClaw = "StripMagicfromClaw";
    public const string MagicScroll = "MagicScroll";
    public const string MonsterEmergence = "MonsterEmergence";
    public const string ClawRange = "ClawRange";
    public const string TheClaw = "TheClaw";
    public const string SafetyStoneEnclosure = "SafetyStoneEnclosure";
    public const string PlayersClaw = "PlayersClaw";
    public const string StoneCourtyard = "StoneCourtyard";
    public const string EpimanaPointOfCourtyard = "EpimanaPointOfCourtyard";
    public const string SimpleClaw = "SimpleClaw";
    public const string ExitCourtyard = "ExitCourtyard";
    public const string TornodoPoint = "TornodoPoint";
}

public class Messages
{
    public const string PointCExitUp = "Go straight up the stairs and take other stairs which will take you up and there you will have to find the Recoleta mirror which will further instruct you.";
    public const string PointCExitDown = "Go down the stairs and take the stairs on your left, now once you have reached the end of the stairs walk some miles straight you will find another stairs which will lead you to the waterways of alkem, where you have to find the recolette mirror.";
    public const string RecolettaMirrorPhare1 = "Hey there, I am Thera, you have to release the ancient spirit which controls this fortress.";
    public const string RecolettaMirrorPhare2 = "You need to battle through the monsters and find lost theracode, which is stored inside a ciphered box";
    public const string BreakSpell1 = "You entered in Alkemia chamber, this chamber is full mysterious artifacts and potion.";
    public const string BreakSpell2 = "You need find the magical potion which will give you power to break wall since there is no way out.";
    public const string CollectTheraCode1 = "You need to place the TheraBox on to the Alkemmana Hotspot which will further disintegrate and transform into Thera code.";
    public const string CollectTheraCode2 = "Collect that stray code it will be usefull later. After succesfully collecting all codes Switch on all the alkemmana hotspot";
    public const string PowerCircuit = "Greetings!! Now that you have switched on all the Alkemmana hotspot please put all the Collected Straycode in the power circuit.";
    public const string PlacedTheraCode1 = "Greetings!!. Placed Theracode.";
    public const string PlacedTheraCode2 = "You need to find the asimana's codex, which will give you magical powers. Asimana codex can be found around the maze.";
    public const string AsimanaCodex = "Access the mainframe and initiate the final sequence using your ancient warrior code.";
    public const string FinalSeq = "Initiate final sequence using your ancient warrior code.";
    public const string TheraUrgeToRevive = "Now that you have initiated the Mainframe, you can release me using your ancient warrior code.";
    public const string TerathianGate = "Gateway to the Terathian has been closed and it will not open again until all the monsters have been eliminated from the Fortress of Eventualities. The remaining enemies are ";
    public const string TheraImportant = "We have to face kaire here, Keep me around I can help you.";
    public const string TheraRevivalRequest = "Thera has been killed, you can revive thera by your magical spell";
    public const string TheraSecondOrbInfo = "Hey, you need to kill all the enemy from this area in order to reveal the second orb.";
    public const string DialogueCollectedLastOrb = "Hey, you need to follow kair and stop her before advancing further.";
    public const string DialogueThirdOrb = "Hey, you need to collect third orb by killing all the monsters emerging from the cave.";
    public const string TheraFrustated = "Curse you Rasveus, and will you stop using me as your damn messenger.";
    public const string UseSpellToShowBinaryHologram = "Hey Rasveus here, now you can use one of your mysterious spells from grimoire to locate the three orbs of kinesis";
    public const string EnterMiniGame = "You need to use adacode cryptex from your grimoire which will take you to the different world where you can remove the magic that is surrounding the orb.";
    public const string FindCryptex = "You need to find the asimana cryptex in order to remove the magic around the orb.";
    public const string TurnOrbsToEmitter = "Hey, Orin this side, now that you have placed all your orbs you just have to use a spell from your grimoire which will turn the orbs to a emitter. Be aware kaire and her monsters can disrupt the process.";
    public const string WarnPowerfullChest = "Hey, I sense something very powerfull inside the chest. It can open a portal to a new world or destroy everything I am not sure.";
    public const string WarnKrakePede = "Hey, Be aware of the krakepede it will be dropping explosive chests on maze which can have effect on your health.";
    public const string KaireFightingArea = "Hey, There will be lot of Kaire monsters ahead you have to kill all of them in order to proceed to next level";
    public const string MonsterEmergence = "Hey, There is unknown chest I sense something wrong about it, dont forget to head back after collecting chest.";
    public const string TheClaw = "Hey, There should be claw artifact around here. The claw is one of Terath's shapeshifting powers enshrouded in darkness, and its power must be neutralized because its power is too great.";
    public const string DialogueClaw = "Hey, claw is stored inside grimore you can check it there. You need to get to the stone enclosure to neutralize the claw.'s power, to stop the disintegration / kinesis magic, which is shrouding you in a purple glow and causing parts of your body to disappear before it abounds to another unknown location.";
    public const string DialogueTheraCastReverseSpell = "We reached the safe enclosure. Now let me cast reverse spell, since you are not experienced enough to do a reverse spell yet.";
    public const string DialogueSignalMilut = "Hey, press signal milut to signal to milut. It will set a homing beacon to milut";
    public const string DialogueMilutWarning = "Hey, Milut here. Avoid interacting with Terath and his right hand Merthone at all costs because their power is too great.";
    public const string DialogueTheraForceField = "Hey, Let me remove the force field";
    public const string DialogueClawsRealPower = "Hey, the claw is encased with Terath's right-hand man's darkness to make it more potent. You will need to use a large amount of their Llummana magic to strip it of its power because the claw is encased with not only Terath's shapeshifting power but with his right-hand man, Merthon'es power over darkness.";
    public const string DialogueClawsRealPowerTwo = "This will weaken you significantly";
    public const string DialogueCreateSpellToStrip = "Hey now that you have collected all 5 theracode and 3 magic scrolls we can create a spell to strip dark magic from claw. Open Grimoire and bring forth Claw.";
    public const string DialogueNotEnoughEnergy = " Hey, It seems you dont have enough energy to spare in order to strip the dark magic.";
    public const string DialogueFreezeOutpouringDarkness = " Hey, freeze the outpouring darkness emerging from the claw using your grimoire.";
    public const string DialogueExtractEnergy = " Hey, you have to travel back to the couryard.";
    public const string DialogueStoreClawInsideGrimoire = " Hey, you can store The claw inside your grimoire now.";
    public const string DialogueEpimanaPoint = " Hey, you need to place the claw here on Epimana point then initiate the sequence to fix courtyard.";
    public const string DialogueDisinetratingCourtyard = " Hey, You need to tap into your ancient warrior codes to alter the claw's shapeshifting, disintegrating kinesis ability.";
    public const string DialogueOrin = " Hey, You know what to do meanwhile I will hold them off.";
    public const string DialogueTornadoWarning = " Hey, You need to use the claws dark energy with your weapon in order defeat the monster, use the claw from grimoire and remember It will only last for 45 seconds.";
    public const string DialoguePlaceOrbAndClaw = " Hey, You need to place the claw over the golden pattern.";
}

public class MessagesWeekFour
{
    public const string DialogueTheraFarAway = " Hey, I am not so far away from you.";
    public const string DialogueTherGuidanceaboutRelicAndGrandFather = "Hey, Retrieve the next shapeshifting ability and to find the magical scrolls containing the details " +
        "to use the Adacode, track grandfather and make sure he is still alive, and to find the scared Meron NFT Relic, using the spell, she has" +
        " gifted them, to help them access greater power to transform the Adacode, when they find the Llumarca device. You will also need to find me," +
        "Since I cannot find you from the time we got in this world. ";
    public const string DialogueTheraClose = "Hey thera this side, I sense you near me.";
    public const string DialogueAutomaticCave = "Hey this cave wall will be moving and the space will be decreased you need to find an escape root and put back thera in your pocket.";
    public const string DialogueWarningForArrows = "Hey, do not to let the archer pierce them with the arrows because it contains Terath's second-in-command, Merthone, the Mesmeriser's darkness. There is only one warrior with such a precise shot, Arturo the Dissolver";
    public const string DialogueWarningForArturo = "Hey, do not to let the archer pierce you. There is only one warrior with such a precise shot, Arturo the Dissolver";
    public const string DialogueCollectIngrediants = "Hey, you need to defeat the monsters effected by dissolving magic then you can collect ingredients which is placed after dissolving magic in order to heal thera.";
    public const string DialogueFollowTrail = "Hey, you need to follow this trail in order to reach destination.";
    public const string DialoguePlaceIngredients = "Hey, you need to place all the ingredients you collected so far in order to heal thera.";
    public const string DialogueCollectandPlaceArtifacts = "Hey, you need to collect Claw and Orb and then place it to the platform in order to activate the portal.";
    public const string DialogueTheraInformingAboutKnockDown = "Hey, I saw a shadow knock you down. It was scary!";
    public const string Dialoguearmor = "Hey, see these broken armor's, we need to collect these. You can open grimoire to check how many parts of armor you have.";
    public const string DialogueGlimmer = "Hey, You just found glimmer artifact you can collect it and place it inside your grimoire.";

    public const string DialogueGlimmerTypeOne = "Hey, here is a fun fact about glimmer artifact.There are five types of glimmer artifact and every kind has its own spacial power and this one reveals hidden chest with loot.";
    public const string DialogueGlimmerTypeTwo = "Hey, here is a fun fact about glimmer artifact.There are five types of glimmer artifact and every kind has its own spacial power and this one reveals hidden Potions.";
    public const string DialogueGlimmerTypeThree = "Hey, here is a fun fact about glimmer artifact.There are five types of glimmer artifact and every kind has its own spacial power and this one reveals hidden artifacts.";
    public const string DialogueGlimmerTypeFour = "Hey, here is a fun fact about glimmer artifact.There are five types of glimmer artifact and every kind has its own spacial power and this one reveals hidden pathways.";
    public const string DialogueGlimmerTypeFive = "Hey, here is a fun fact about glimmer artifact.There are five types of glimmer artifact and every kind has its own spacial power and this one reveals hidden information.";
}

public class MessagesWeekFive
{
    public const string DialogueTheraScan = " You have to talk to ghosts to get information about eliminating monsters and listen to the clues to find Mira, the Mediator.";
    public const string DialogueAsimanaShaft = " Collect information from asimana light shafts.";
    public const string DialogueAsimanaShaftWarning = " Collect as much information as you can from the asimana light shaft";
    public const string DialogueFindKey = " You need to find the key in order to remove the force field.";
    public const string DialogueRevealMonster = " Enter minigame, by completing the game you can get the power in your weapon which can reveal monster hidden inside ghosts. Gold glow means monster is camouflaged inside ghosts and white glow is just regular ghosts";
    public const string DialogueStarDeviceMiraFind = "Look up to star device to find the location of mira. It will be beeping on and off.";
    public const string DialogueMirasChallenge = "Hey mira this side, In order to take my help you have to complete a small challenge and then I will show you the way.";
    public const string DialogueOrinAndRasveus = "Hey Orin and Rasveus arrived, they would help them fix mira.";
    public const string DialogueMirasFinalTarget = "Hey, you need to defeat the monster guarding the maze enterance then collect all the theracodes from the maze and then use star device to locate the yellow dot that is beeping.";
    public const string DialogueFinal = "you might find some clue there to open a portal to the new world.";
    public const string DialogueCollectMagicalKey = "Greetings! Acquire the enchanted key, it holds the power to unlock new paths and propel you forward in your journey.";
}