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
* {getCurrentInterestLion() > 0} [Lure them]
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
* {getCurrentInterestFox() > 6} [Lure them]
    -> LURE
* {getCurrentInterestFox > 3} The perfume you are wearing is exquisite.
    -> CHIT_CHAT_PERFUME
* {getCurrentInterestFox > 4} Would you be interested in a proposal?
    -> CHIT_CHAT_PROPOSE
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

= CHIT_CHAT_PERFUME
Player: Could you help out an ordinary guy like myself,<br>to choose a good cologne?
Fox: Of course, what vision are you going for?
* What scent would make a woman like you swoon?
    Fox: What a sly question. I'd definitely go for something forest and smokey fragrance.
    ~ changeCurrentInterestFox(1)
* I don't want to stand out too much.
    Fox: That's no fun. But to answer your question -
    Fox: Some fresh laundry then. I'm not well versed in subtle matters.
    ~ changeCurrentInterestFox(-1)

- -> HUB

= CHIT_CHAT_PROPOSE
Fox: A marriage proposal? Might be too soon darling~
* For that I'd choose a better place and time, don't worry. It's about a business.
    Fox: Oh, do tell.
    Player: I'd like to know more about some of your sources.
    Player: If you know what I mean.
    Fox: I understand you perfectly and clear.<br>Lend me some of your time then, if you don't mind.
    ~ changeCurrentInterestFox(1)
    Player: [I see her relax and ease into talking about all the alinments she can get<br>at the snap of her finger.]
* That's not my style, I'd like to stay in my line. It's about a business.  
    Fox: You are the uptight kind...
    Fox: What is it?
    Player: I'd like to know more about some of your sources.
    Player: If you know what I mean.
    Fox: I don't have much time, so I'll be brief.
    Player: [She speedruns through her contacts, looking right through me.<br>I can't comprehend any of it, it's that fast]
    ~ changeCurrentInterestFox(-1)
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


=== BEAR

{
    - not INTRODUCTION: -> INTRODUCTION
    - else: 
        Player: So...
        -> HUB
}

= INTRODUCTION
Player: You strike an imposing figure, Mr Bear.<br>What brings you here tonight?
Bear: Flattery won't get you anywhere.<br>What is it that you want?
-> HUB

= HUB
* Are you the bodyguard here?
    -> CHIT_CHAT_GUARD
* What is your goal here at the fundraiser?
    -> CHIT_CHAT_FUNDRAISE
* {getCurrentInterestBear() > 0} [Lure them]
    -> LURE
+ Sorry. I have to go.
    -> DONE
+ -> fallback

= CHIT_CHAT_GUARD
Bear: I can't talk about my work, it is off limits.
Bear: Why do you ask?
* Was hoping to get a bodyguard myself, but then this is not the right place for it.
    Bear: Yes, that is not a topic you should bring up out of the blue here.
    ~ changeCurrentInterestBear(-1)
* I was thinking of networking around security companies.
    Player: My next big investment will be in a bar chain across the city.<br>Hoped you could recommend me some services?
    Bear: I have a few people I could ask.
    Player: That would be great, thank you.
    ~ changeCurrentInterestBear(1)
- -> HUB

= CHIT_CHAT_FUNDRAISE
Bear: I care about the country's children.<br>You?
* I would like to help them too. I have big hopes in my next investment,<br>hope the masses see the potential as well.
    ~ changeCurrentInterestBear(1)
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
    - CHOOSEGOSSIP <= 2: -> CHOOSEGOSSIP
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
    - CHOOSEGOSSIP <= 2: -> CHOOSEGOSSIP
    - else: 
        Gossiper: Oh, stop. You are much too curious.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC3
{
    - CHOOSEGOSSIP <= 2: -> CHOOSEGOSSIP
    - else: 
        Gossiper: Shhh, people are staring.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC4
{
    - CHOOSEGOSSIP <= 2: -> CHOOSEGOSSIP
    - else: 
        Gossiper: I should maybe stop gossiping.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC5
{
    - CHOOSEGOSSIP <= 2: -> CHOOSEGOSSIP
    - else: 
        Gossiper: With all this talk, the ice in my drink is melting.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC6
{
    - CHOOSEGOSSIP <= 2: -> CHOOSEGOSSIP
    - else: 
        Gossiper: Ah, to be young again.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC7
{
    - CHOOSEGOSSIP <= 2: -> CHOOSEGOSSIP
    - else: 
        Gossiper: Ah... I wish my mask was prettier.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIPNPC8
{
    - CHOOSEGOSSIP <= 2: -> CHOOSEGOSSIP
    - else: 
        Gossiper: Just one more drink, I promise.
        -> DONE
}
= CHOOSEGOSSIP
    ~ goss = RandomGossip(RANDOM(0,5))
    -> goss
-> DONE

=== GOSSIP1
    Gossiper: {~That one with the bear mask is the minister. I heard him shouting to a valet after he offered to help him with his suitcase ohohoho.|I saw the minister is wearing a bear mask. They must have run out of insecure baboon masks.}
-> DONE
=== GOSSIP2
    Gossiper: {~I can't believe the CEO of RealESpace is wearing a genuine Camille Lecourt design. She slayin', too.|Have you seen the one with the fox mask? She must be loaded, to be wearing a Camille Lecourt dress.}
-> DONE
=== GOSSIP3
    Gossiper: {~The owner looks stressed. I have not seen them even stop for a drink today. They're really earning the lion monicker.|The Lion has been running up and down until now. Must be hard to run a hotel}
-> DONE
=== GOSSIP4
    Gossiper: {~I haven't seen Sandra tonight. But it's no wonder the owner of the Royal Cat Hotel would not<br>want her or their kids in the fish tank when the sharks are swimming.|I do hope Sandra is feeling better, but I don't quite buy the hotel owner's story. They were surely trying to keep her away from the bear.}
-> DONE
=== GOSSIP5
    Gossiper: {~The minister might think he can garner enough support if he secures donations, but I can tell his ship is sinking.|Who has the gall to attend a fundraiser and ask for money? The bear's re-election might be teetering on the edge.}
-> DONE
=== GOSSIP6
    Gossiper: {~Have you seen Carmen? She promised me a hit.|I really need to ask the Fox lady where she got her stuff.}
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

