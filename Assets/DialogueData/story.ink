EXTERNAL fadeOutSequence(fadeOutTime, waitTime, fadeInTime) // Fade out to black in fadeOutTime seconds, 
                                                            //  wait an additional waitTime seconds, 
                                                            //  fade in in fadeInTime seconds 
EXTERNAL fadeOut(fadeOutTime)
EXTERNAL fadeIn(fadeInTime)

EXTERNAL wait(waitTime) // Pause ink, hide dialogue boxed, then return

VAR owner_interest = 0

=== OWNER

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: HUB
}

= INTRODUCTION
Player: What a lovely dress.<br>What's bringing you here tonight?
Owner: Let's get to the point.<br>What do you want?
-> HUB

= HUB
* How can I join the funding?
    -> CHIT_CHAT_FUNDING
* What is your finest liquor?
    -> CHIT_CHAT_LIQUOR
* {owner_interest > 0} [Lure them]
    -> LURE
* Sorry. I have to go.
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
Player: Imagine animations here.
-> DONE
//TODO: fade out
//TODO: animation

= fallback
Owner: you ran out of choces.
Meow.
-> DONE

// SPACE CEO
/*
base choices to talk abt:
* You have a lovely dress, may I know what brand is this?
    CEO: Hmmm, you have a good eye for these things, so I could share... It is VRM, have you heard of it before?
    * 

*/

VAR space_ceo_interest = 0

=== CEO

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: -> HUB
}

= INTRODUCTION
Player: What an alluring guest.<br>What's bringing you here tonight?
Space_CEO: Let's get to the point pretty boy.<br>What do you want?
-> HUB

= HUB
* You have a lovely dress, may I know what brand is this?
    -> CHIT_CHAT_FLATTER
* Did you by any chance see Carmen here?
    -> CHIT_CHAT_DRUGS
* {space_ceo_interest > 0} [Lure them]
    -> LURE
* Sorry. I have to go.
    -> DONE
+ -> fallback

= CHIT_CHAT_FLATTER
Space_CEO: Hmmm, you have a good eye for these things, so I could share... 
Space_CEO: It is VRM, have you heard of it before?
* Isn't that an Italian brand? I might have heard of it.
    ~ space_ceo_interest--
    Space_CEO: Hmpf, maybe you are not worth my time after all. 
* French, isn't it? Camille's spring collection is refined, however I prefer the winter one your dress is from.
    Space_CEO: What a polished taste you have, and the flattery is not lost on me~
    ~ space_ceo_interest++
- -> HUB

= CHIT_CHAT_DRUGS
Space_CEO: Not yet, would you be interested to finding her together? 
* I was just checking that nobody is using these kinds of drugs here.
    Player: Sorry for the assumption.
    Space_CEO: ...
    ~ space_ceo_interest--
* Of course, with your beautiful face I'm certain we will find it fast.
    Space_CEO: I will lead the way.
    ~ space_ceo_interest++
- -> HUB

= LURE
Player: Why don't we continue this at your room?
Player: I could use some guidance under your hands.
Space_CEO: With fingers like that I'm sure you'll be a fast learner.
Player: Imagine lure animations here.
-> DONE
//TODO: fade out
//TODO: animation

= fallback
Space_CEO: You ran out of choces.
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
