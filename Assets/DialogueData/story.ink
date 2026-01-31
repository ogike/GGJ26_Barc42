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

VAR space_ceo_interest = 0

=== SPACE_CEO

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: -> HUB
}

= INTRODUCTION
Player: What an alluring guest.<br>What's bringing you here tonight?
Ceo: Better question...
Ceo: What's bringing you here pretty boy?
Ceo: What do you want from me?
-> HUB

= HUB
* You have a lovely dress, may I know what brand is it?
    -> CHIT_CHAT_FLATTER
* Did you by any chance see any "Carmen" here?
    -> CHIT_CHAT_DRUGS
* {space_ceo_interest > 0} [Lure them]
    -> LURE
* Sorry. I have to go.
    -> DONE
+ -> fallback

= CHIT_CHAT_FLATTER
Ceo: Hmmm, you have a good eye for these things, so I could share... 
Ceo: It is VRM, have you heard of it before?
* Isn't that an Italian brand? I might have heard of it, not sure.
    ~ space_ceo_interest--
    Ceo: Hmpf, maybe you are not worth my time after all. 
* French, isn't it? Camille's spring collection is refined, however I prefer the winter one where your dress is from.
    Ceo: What a polished taste you have
    Ceo: And the flattery is not lost on me darling~
    ~ space_ceo_interest++
- -> HUB

= CHIT_CHAT_DRUGS
Ceo: Not yet, would you be interested in finding her together? 
* I was just checking that nobody is using these kinds of drugs here.
    Player: Sorry for the assumption.
    Ceo: ...
    ~ space_ceo_interest--
* Of course, with your beautiful face I'm certain we will find it fast.
    Ceo: I will lead the way.
    ~ space_ceo_interest++
- -> HUB

= LURE
Player: Want to join to my room?
    ~ fadeOut(0.5)
    Player: bite bite bite bite
    Ceo: nooooooooooooo
    ~ killNpc("SpaceCEO")
    ~ teleportPlayer("PostKillPosition")
    ~ fadeIn(0.5)
-> DONE
//TODO: fade out
//TODO: animation

= fallback
Space_CEO: You ran out of choces.
Meow.
-> DONE

VAR minister_war_interest = 0

=== MINISTER_WAR

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: 
        Player: So...
        -> HUB
}

= INTRODUCTION
Player: You have a strong presence.<br>What's bringing you here tonight?
Minister: Flattery won't get you anywhere.<br>What is that you want?
-> HUB

= HUB
* Are you the bodyguard here?
    -> CHIT_CHAT_GUARD
* What is your goal here at the fundraiser?
    -> CHIT_CHAT_FUNDRAISE
* {minister_war_interest > 0} [Lure them]
    -> LURE
+ Sorry. I have to go.
    -> DONE
+ -> fallback

= CHIT_CHAT_GUARD
Minister: I can't talk about my work, it is off limits.
Minister: Why do you ask?
* Was hoping to get a bodyguard myself, but then this is not the right place for it.
    Minister: Yes, that is not a topic you should bring up out of the blue here.
    ~ minister_war_interest--
* I was thinking of networking around security companies.
    Player: My next big investment will be in a bar chain across the city.<br>Hoped you could recommend me some services?
    Minister: I have a few people I could ask.
    Player: That would be great, thank you.
    ~ minister_war_interest++
- -> HUB

= CHIT_CHAT_FUNDRAISE
Minister: I care about the country's children.<br>You?
* I would like to help them too. I have big hopes in my next investment,<br>hope the masses see the potential as well.
    ~ minister_war_interest++
-> HUB

= LURE
Player: Why don't we continue this at your room?
Minister: I agree that we continue this without the crowd watching.
~ fadeOut(0.5)
Player: Imagine animations here.
~ killNpc("MinisterWar")
~ teleportPlayer("PostKillPosition")
~ fadeIn(0.5)
-> DONE
//TODO: fade out
//TODO: animation

= fallback
Minister: you ran out of choces.
Meow.
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

=== function killNpc(npcName) ===
~ return 0
=== function teleportPlayer(placeName) ===
~ return 0

