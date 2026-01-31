EXTERNAL fadeOutSequence(fadeOutTime, waitTime, fadeInTime) // Fade out to black in fadeOutTime seconds, 
                                                            //  wait an additional waitTime seconds, 
                                                            //  fade in in fadeInTime seconds 
EXTERNAL fadeOut(fadeOutTime)
EXTERNAL fadeIn(fadeInTime)

EXTERNAL wait(waitTime) // Pause ink, hide dialogue boxed, then return

EXTERNAL killNpc(npcName)
EXTERNAL teleportPlayer(placeName)

VAR lion_interest_bear = -1
VAR lion_interest_fox = 4
VAR lion_interest_alien = -1
VAR bear_interest_lion = 0
VAR bear_interest_fox = 0
VAR bear_interest_alien = 0
VAR fox_interest_lion = 1
VAR fox_interest_bear = 4
VAR fox_interest_alien = -1

VAR current_mask = "alien"

//-> GOSSIPNPC1

=== LION

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: 
        Player: So...
        -> HUB
}

= INTRODUCTION
Player: What a lovely dress.<br>What's bringing you here tonight?
Lion: Let's get to the point.<br>What do you want?
-> HUB

= HUB
* How can I join the funding?
    -> CHIT_CHAT_FUNDING
* [*stares*]
    Lion: *leaves*.
    ~ changeCurrentInterestLion(-1)
    -> HUB
* [Nothing.]
    Lion: Uh.... sure.
    -> HUB
* What is your finest liquor?
    -> CHIT_CHAT_LIQUOR
* {getCurrentInterestLion > 0} [Lure them]
    -> LURE
+ Sorry. I have to go.
    -> DONE
+ -> fallback

= CHIT_CHAT_FUNDING
Lion: Oh! You can leave a cheque at the receptionist!
* That's too much work...
    ~ changeCurrentInterestLion(-1)
* Thank you! I will leave a gazillion dollars.
    The children of the future war need it.
    ~ changeCurrentInterestLion(1)
- -> HUB

= CHIT_CHAT_LIQUOR
I bet your establishment has some good wine.
Lion: Thank you! 
~ changeCurrentInterestLion(1)
-> HUB

= LURE
Player: Why don't we continue this at your room?
Lion: Of course! I trust you so much now!
~ fadeOut(0.5)
Player: Imagine animations here.
~ killNpc("Lion")
~ teleportPlayer("PostKillPosition")
~ fadeIn(0.5)
-> DONE
//TODO: fade out
//TODO: animation

= fallback
Lion: you ran out of choces.
Meow.
-> DONE


=== FOX

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: -> HUB
}

= INTRODUCTION
Player: What an alluring guest.<br>What's bringing you here tonight?
Fox: Better question...
Fox: What's bringing you here to me, pretty thing?
Fox: What do you want from me?
-> HUB

= HUB
* You have a lovely dress, may I know what brand is it?
    -> CHIT_CHAT_FLATTER
* Did you by any chance see any "Carmen" here?
    -> CHIT_CHAT_DRUGS
* {getCurrentInterestFox > 6} [Lure them]
    -> LURE
* Sorry. I have to go.
    -> DONE
+ -> fallback

= CHIT_CHAT_FLATTER
Fox: Hmmm, you have a good eye for these things, so I could share... 
Fox: It is VRM, have you heard of it before?
* Isn't that an Italian brand? I might have heard of it, not sure.
    ~ changeCurrentInterestFox(-1)
    Fox: Hmpf, maybe you are not worth my time after all. 
* French, isn't it? Camille's spring collection is refined, however I prefer the winter one where your dress is from.
    Fox: What a polished taste you have
    Fox: And the flattery is not lost on me darling~
    ~ changeCurrentInterestFox(1)
- -> HUB

= CHIT_CHAT_DRUGS
Fox: Not yet, would you be interested in finding her together? 
* I was just checking that nobody is using these kinds of drugs here.
    Player: Sorry for the assumption.
    Fox: ...
    ~ changeCurrentInterestFox(-1)
* Of course, with your beautiful face I'm certain we will find it fast.
    Fox: I will lead the way.
    ~ changeCurrentInterestFox(1)
- -> HUB

= LURE
Player: Want to join to my room?
    ~ fadeOut(0.5)
    Player: bite bite bite bite
    Fox: nooooooooooooo
    ~ killNpc("Fox")
    ~ teleportPlayer("PostKillPosition")
    ~ fadeIn(0.5)
- -> DONE

= fallback
fox: You ran out of choces.
Meow.
-> DONE

VAR bear_war_interest = 0

=== BEAR

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: 
        Player: So...
        -> HUB
}

= INTRODUCTION
Player: You have a strong presence.<br>What's bringing you here tonight?
Bear: Flattery won't get you anywhere.<br>What is that you want?
-> HUB

= HUB
* Are you the bodyguard here?
    -> CHIT_CHAT_GUARD
* What is your goal here at the fundraiser?
    -> CHIT_CHAT_FUNDRAISE
* {bear_war_interest > 0} [Lure them]
    -> LURE
+ Sorry. I have to go.
    -> DONE
+ -> fallback

= CHIT_CHAT_GUARD
Bear: I can't talk about my work, it is off limits.
Bear: Why do you ask?
* Was hoping to get a bodyguard myself, but then this is not the right place for it.
    Bear: Yes, that is not a topic you should bring up out of the blue here.
    ~ bear_war_interest--
* I was thinking of networking around security companies.
    Player: My next big investment will be in a bar chain across the city.<br>Hoped you could recommend me some services?
    Bear: I have a few people I could ask.
    Player: That would be great, thank you.
    ~ bear_war_interest++
- -> HUB

= CHIT_CHAT_FUNDRAISE
Bear: I care about the country's children.<br>You?
* I would like to help them too. I have big hopes in my next investment,<br>hope the masses see the potential as well.
    ~ bear_war_interest++
-> HUB

= LURE
Player: Why don't we continue this at your room?
Bear: I agree that we continue this without the crowd watching.
~ fadeOut(0.5)
Player: Imagine animations here.
~ killNpc("Bear")
~ teleportPlayer("PostKillPosition")
~ fadeIn(0.5)
-> DONE
//TODO: fade out
//TODO: animation

= fallback
Bear: you ran out of choces.
Meow.
-> DONE
//##################################################################################


VAR goss = -> GOSSIP1
=== GOSSIPNPC1
{
    - not CHOOSEGOSSIP: -> CHOOSEGOSSIP
    - else: 
        Gossiper: We should not draw more attention.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC2
{
    - not CHOOSEGOSSIP: -> CHOOSEGOSSIP
    - else: 
        Gossiper: We should not draw more attention.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC3
{
    - not CHOOSEGOSSIP: -> CHOOSEGOSSIP
    - else: 
        Gossiper: We should not draw more attention.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC4
{
    - not CHOOSEGOSSIP: -> CHOOSEGOSSIP
    - else: 
        Gossiper: We should not draw more attention.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC5
{
    - not CHOOSEGOSSIP: -> CHOOSEGOSSIP
    - else: 
        Gossiper: We should not draw more attention.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC6
{
    - not CHOOSEGOSSIP: -> CHOOSEGOSSIP
    - else: 
        Gossiper: We should not draw more attention.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC7
{
    - not CHOOSEGOSSIP: -> CHOOSEGOSSIP
    - else: 
        Gossiper: We should not draw more attention.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC8
{
    - not CHOOSEGOSSIP: -> CHOOSEGOSSIP
    - else: 
        Gossiper: We should not draw more attention.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIP1
    Gossiper: Hahaha1
-> DONE
=== GOSSIP2
    Gossiper: Hahaha2
-> DONE
=== GOSSIP3
    Gossiper: Hahaha3
-> DONE
=== GOSSIP4
    Gossiper: Hahaha4
-> DONE
=== GOSSIP5
    Gossiper: Hahaha5
-> DONE
=== GOSSIP6
    Gossiper: Hahaha6
-> DONE
    


=== function RandomGossip(num) ===
    { num:
        - 0: 
            ~ return -> GOSSIP1
        - 1: 
            ~ return -> GOSSIP2
        - 2:
            ~ return -> GOSSIP3
        - 3:
            ~ return -> GOSSIP4
        - 4:
            ~ return -> GOSSIP5
        - 5:
            ~ return -> GOSSIP6
        - else:
            ~ return -> GOSSIP1
    }

=== function getCurrentInterestLion() ===
{ current_mask:
    - "bear": 
        ~ return lion_interest_bear
    - "fox": 
        ~ return lion_interest_fox
    - else:
        ~ return lion_interest_alien 
}
=== function changeCurrentInterestLion(value)
{ current_mask:
    - "bear": 
        ~ lion_interest_bear += value
    - "fox": 
        ~ lion_interest_fox += value
    - else:
        ~ lion_interest_alien += value
}
=== function getCurrentInterestBear() ===
{ current_mask:
    - "lion": 
        ~ return bear_interest_lion
    - "fox": 
        ~ return bear_interest_fox
    - else:
        ~ return bear_interest_alien 
}
=== function changeCurrentInterestBear(value)
{ current_mask:
    - "lion": 
        ~ bear_interest_lion += value
    - "fox": 
        ~ bear_interest_fox += value
    - else:
        ~ bear_interest_alien += value
}
=== function getCurrentInterestFox() ===
{ current_mask:
    - "lion": 
        ~ return fox_interest_lion
    - "bear": 
        ~ return fox_interest_bear
    - else:
        ~ return fox_interest_alien 
}
=== function changeCurrentInterestFox(value)
{ current_mask:
    - "lion": 
        ~ fox_interest_lion += value
    - "bear": 
        ~ fox_interest_bear += value
    - else:
        ~ fox_interest_alien += value
}

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

