EXTERNAL OpenChest(chestNumber)
EXTERNAL StartEncounter(encounter)
-> yo


==yo==
- yo wassup do  you like pizza
*[yes]
*[no] -> cancer
- good you're a human
*.
~OpenChest(0)
-> DONE

==cancer==
-are you sure demon?
*[yes] -> bruh
*[no] -> yo
*fuck you
- fuck you too
*.
~ StartEncounter(0)
->DONE

==bruh==
-go to hell
*.
-bitch
->DONE
