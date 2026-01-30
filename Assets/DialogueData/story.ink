EXTERNAL fadeOutSequence(fadeOutTime, waitTime, fadeInTime) // Fade out to black in fadeOutTime seconds, 
                                                            //  wait an additional waitTime seconds, 
                                                            //  fade in in fadeInTime seconds 
EXTERNAL fadeOut(fadeOutTime)
EXTERNAL fadeIn(fadeInTime)

EXTERNAL wait(waitTime) // Pause ink, hide dialogue boxed, then return

=== NPC1
Anna: I really shouldn't have stayed up <br>all night reading about...
What was that herb?
Cowslip?
More like Howsick if you know what <br>I... meawnmmmm...
[...]
A letter?
Oh it's from Auntie!
NPC: It reads, "Hello Sunshine,
Today I'm starting my yearly winter getaway <br>and leaving the town for a few weeks.
If you could hop on over every once in <br>a while to check on the indoor plants - 
I would be eternally grateful.
I know that you know the drill already, <br>but just to make sure,
Feel free to take or eat or use anything.
Anything, okay?
If nothing will be missing from the pantry, <br>you can count on your demise.
I'm serious! - Auntie"
Anna: I think she drew the point home.
[...]
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
