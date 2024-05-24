EXTERNAL OpenChest(chestNumber)
EXTERNAL StartEncounter(encounter)
-> yo


==yo==
- Open Chest?
*yes -> openChest
*no -> cancer


==openChest==
~OpenChest(1)
-> DONE

==cancer==
-want to fight an enemy?
*yes -> startEncouter
*no -> bruh

==startEncouter==
~ StartEncounter(0)
->DONE

==bruh==
-ok
*.
->DONE
