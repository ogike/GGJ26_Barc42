EXTERNAL fadeOutSequence(fadeOutTime, waitTime, fadeInTime) // Fade out to black in fadeOutTime seconds, 
                                                            //  wait an additional waitTime seconds, 
                                                            //  fade in in fadeInTime seconds 
EXTERNAL fadeOut(fadeOutTime)
EXTERNAL fadeIn(fadeInTime)

EXTERNAL wait(waitTime) // Pause ink, hide dialogue boxed, then return

EXTERNAL killNpc(npcName)
EXTERNAL teleportPlayer(placeName)

VAR owner_interest = 0

=== OWNER

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: 
        Player: So...
        -> HUB
}

= INTRODUCTION
Player: What a lovely dress.<br>What's bringing you here tonight?
Owner: Let's get to the point.<br>What do you want?
-> HUB

= HUB
* How can I join the funding?
    -> CHIT_CHAT_FUNDING
* [*stares*]
    Owner: *leaves*.
    ~ owner_interest--
    -> HUB
* [Nothing.]
    Owner: Uh.... sure.
    -> HUB
* What is your finest liquor?
    -> CHIT_CHAT_LIQUOR
* {owner_interest > 0} [Lure them]
    -> LURE
+ Sorry. I have to go.
    -> DONE
+ -> fallback

= CHIT_CHAT_FUNDING
Owner: Oh! You can leave a cheque at the receptionist!
* That's too much work...
    ~ owner_interest--
* Thank you! I will leave a gazillion dollars.
    The children of the future war need it.
    ~ owner_interest++
- -> HUB

= CHIT_CHAT_LIQUOR
I bet your establishment has some good wine.
Owner: Thank you! 
~ owner_interest++
-> HUB

= LURE
Player: Why don't we continue this at your room?
Owner: Of course! I trust you so much now!
~ fadeOut(0.5)
Player: Imagine animations here.
~ killNpc("Owner")
~ teleportPlayer("PostKillPosition")
~ fadeIn(0.5)
-> DONE
//TODO: fade out
//TODO: animation

= fallback
Owner: you ran out of choces.
Meow.
-> DONE


=== SPACE_CEO
CEO: fuck them kids.

* [Agree]
    Player: So true! 
* [Kill.]
    Player: Want to join to my room?
    ~ fadeOut(0.5)
    Player: bite bite bite bite
    Ceo: nooooooooooooo
    ~ killNpc("SpaceCEO")
    ~ teleportPlayer("PostKillPosition")
    ~ fadeIn(0.5)
- -> DONE


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

=== function killNpc(npcName) ===
~ return 0
=== function teleportPlayer(placeName) ===
~ return 0

