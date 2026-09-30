Document transmis à l'étudiant par Thomas Verdier, en pièce jointe de son message.

> [WARNING] [PRODUCTION — cette page doit être reconvertie en fichier `plan\_de\_tests.md` et jointe au starter kit du projet — alt : —]
> 

<aside>
ℹ️

Bulle info : ce plan dit ce qui doit être vérifié et à quel niveau. Il donne un exemple rédigé par niveau de test, pas la solution : les cas restants sont à écrire par l'étudiant.

</aside>

## Ce que ce plan couvre

L'application de Norlys Mobilité gère les stations, les véhicules et les comptes des agents de terrain. Les deux régressions parties en production venaient de règles métier modifiées sans filet. Le plan cible donc en priorité ces règles, puis les parcours qui les traversent.

Trois niveaux sont attendus :

- les tests unitaires, qui vérifient une règle métier isolément, sans base de données ni serveur ;
- les tests d'intégration, qui vérifient qu'un parcours traverse correctement les couches, jusqu'à la base ;
- un test end-to-end, qui rejoue un parcours complet tel qu'un agent le vivrait.

## Ce qui doit être vérifié

### Niveau unitaire

Pour chaque règle métier ciblée : le cas nominal, au moins un cas limite et au moins un cas d'erreur. Les dépendances de la classe testée sont remplacées par des doublures.

> [WARNING] [PRODUCTION — lister ici les classes et règles métier ciblées, le nombre de tests unitaires attendu et un exemple de test rédigé — alt : —]
> 

### Niveau intégration

Les parcours qui vont du point d'entrée de l'API jusqu'à la base de données, dont au moins un exécuté contre une base réelle éphémère plutôt que simulée.

> [WARNING] [PRODUCTION — lister ici les parcours d'intégration à couvrir et un exemple rédigé — alt : —]
> 

### Niveau end-to-end

Un parcours complet, de l'identification de l'agent jusqu'au résultat attendu.

> [WARNING] [PRODUCTION — préciser ici le parcours end-to-end retenu — alt : —]
> 

## Couverture attendue

Un rapport de couverture doit être généré sur le périmètre de ce plan, et commenté dans le README : ce que le chiffre dit, ce qu'il ne dit pas, et au moins une classe peu couverte assumée.

> [WARNING] [PRODUCTION — indiquer ici le taux de couverture attendu — alt : —]
> 

<aside>
⚠️

Bulle attention : un test sans assertion s'exécute, passe au vert et ne vérifie rien. Il augmente la couverture sans protéger l'application.

</aside>