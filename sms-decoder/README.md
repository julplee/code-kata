# Le décodeur de SMS

Certaines personnes compressent le texte des messages en ne conservant que les voyelles qui débutent un mot, en supprimant les accentuations et en remplaçant les lettres doublées par des lettres simples.

Ainsi, le texte ci-dessus devient:

`crtns prsns cmprsnt l txt ds msgs en n cnsrvnt q ls vls q dbtnt un mt en sprmnt ls acnttns et en rmplcnt ls ltrs dbls pr ds ltrs smpls`

L'objectif est d'écrire un "décodeur de SMS" qui tente de recomposer la phrase d'origine, en proposant les différentes alternatives quand une même simplification correspond à plusieurs possibilité.

Exemple, avec un dictionnaire réduit:

`(certains certaines) personnes compressent (le la) texte des messages en (ne ni n’a) conservant (que qui quoi) (les lus lis lys) (villes voyelles viols voiles ...)`

Le dictionnaire peut se télécharger [ici](/sms-decorder/dictionnaire.txt)