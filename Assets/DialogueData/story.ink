EXTERNAL fadeOutSequence(fadeOutTime, waitTime, fadeInTime) // Fade out to black in fadeOutTime seconds, 
                                                            //  wait an additional waitTime seconds, 
                                                            //  fade in in fadeInTime seconds 
EXTERNAL fadeOut(fadeOutTime)
EXTERNAL fadeIn(fadeInTime)

EXTERNAL wait(waitTime) // Pause ink, hide dialogue boxed, then return

VAR npc1_interest = 0

=== NPC1
Player: What a lovely dress.<br>What's bringing you here tonight?
NPCTest: Thank you!<br>Let's get to the point.<br>What do you want?
* [Your blood.]
    ~ npc1_interest++
    NPCTest: Oh.... How intriguing.
* [Your mask.]
    ~ npc1_interest--
    NPCTest: W-What do you mean?
* [You know me... The usual.]
    ~ npc1_interest += 2

- NPCTest: Wait.
What is this test for?

{
    - npc1_interest > 0: 
        You won. Good job.
    - else:
        You lost. meow.
}
Player: bye

-> DONE


//##################################################################################


// this is purely to make the errors go away in the Ink Player, will be overriden by unity, ignore
=== function fadeOutSequence(x,y,z) ===
~ return 0
=== function fadeOut(fadeOutTime) ===
~ return 0
=== function fadeIn(fadeInTime) ===
~ return 0

=== function wait(waitTime) ===
~ return 0
