EXTERNAL OpenChest(chestNumber)
EXTERNAL StartEncounter(encounter)
-> yo


==yo==
- Open Chest?
*yes -> openChest
*no -> DONE


==openChest==
~OpenChest(1)
-> DONE