:::meta
titre: Testez et améliorez une application existante
parcours: Développeur Full-stack back-end avancé
numero_projet: P6
variation_scenario: standard
nb_missions: 1
guidage: entierement_guide
duree_supervisee_h: 50
duree_personnelle_h: 50
certifiant: non
evaluation: session de bilan avec le mentor accompagnateur
prepare: le projet de synthèse P7, où tests et sécurisation s'intègrent dans un chantier complet
competence_1: Écrire des tests unitaires [À VALIDER]
competence_2: Écrire des tests d'intégration et e2e [À VALIDER]
competence_3: Sécuriser les données d'une application / Mettre en place l'authentification d'une application [À VALIDER]
:::

# Page d'accueil

## Qu'allez-vous apprendre dans ce projet ?

Une application qui fonctionne aujourd'hui peut casser demain, à la première modification. Vous allez apprendre à vous en prémunir en écrivant des **tests unitaires** : des tests automatisés qui vérifient chaque règle de calcul et chaque comportement isolément, et qui échouent dès qu'une régression apparaît. Vous produirez aussi un **rapport de couverture**, l'outil qui mesure quelles parties du code vos tests exercent réellement.

Vous monterez ensuite d'un cran : les **tests d'intégration**, qui vérifient que les couches de l'application collaborent correctement, et les tests **end-to-end** (E2E), qui rejouent un parcours utilisateur complet de bout en bout. Vous apprendrez surtout à choisir le bon niveau de test pour chaque vérification, plutôt que de tout tester de la même façon.

Enfin, vous sécuriserez l'application : hachage des mots de passe, mise en place d'une authentification, protection des accès aux données. Vous documenterez chaque correction, parce qu'une protection que l'équipe ne comprend pas sera défaite à la prochaine évolution.

## En quoi ces compétences sont-elles importantes pour votre carrière ?

Dans une équipe de développement, les tests automatisés sont ce qui permet de livrer sans peur : ils transforment « je crois que ça marche encore » en « la suite passe au vert ». Savoir décider quoi tester, à quel niveau, et lire honnêtement un rapport de couverture est ce qui distingue un développeur qui écrit du code d'un développeur à qui on confie une base de code.

La sécurité applicative, elle, n'est jamais optionnelle : un mot de passe stocké en clair ou un accès non protégé engage la responsabilité de toute l'entreprise. Un développeur back-end qui sait hacher, authentifier et expliquer ses protections est immédiatement plus employable.

## Comment allez-vous procéder ?

- **Mission 1 : Testez et sécurisez l'application** : vous écrirez des tests unitaires puis des tests d'intégration et E2E sur une application existante, avec leur rapport de couverture, puis vous corrigerez ses failles de sécurité et mettrez en place son authentification.

Ce projet est **entièrement guidé** : chaque étape vous dit quoi produire, avec quoi et dans quel ordre.

Ce projet n'est pas certifiant et ne fait pas l'objet d'une soutenance. À l'issue du projet, vous présenterez votre travail à votre **mentor accompagnateur** lors d'une **session de bilan** : l'objectif est de vérifier ensemble que vous êtes prêt pour le projet de synthèse qui suit, et d'identifier ce qu'il vous reste à consolider.

## Prêt à démarrer votre projet ?

Lancez-vous dans la « Mission 1 - Testez et sécurisez l'application ».

:::encadre type=info
**Votre projet démarre : suivez ces quelques recommandations pour être plus efficace !**

- Coupez dès à présent toutes les sources de distraction : téléphone, messagerie, mails, notifications, etc.
- Évitez les situations de multitâches : n'écoutez pas un podcast ou les informations en travaillant.
- Préparez votre environnement de travail : onglets, documents téléchargés, raccourcis, etc.

Vous avez toutes les cartes en main, c'est parti ! Pour plus de conseils, suivez ce chapitre de cours : Mettez en place votre environnement d'apprentissage.
:::

---

# Mission 3 - Testez et sécurisez l'application (.NET)

:::encadre type=autoeval
Ce projet n'est pas certifiant : il n'y a pas de critères d'évaluation par compétence.
L'étudiant dispose en revanche d'une **fiche d'autoévaluation** dans la dernière étape de son travail.
Elle lui sert de checklist avant la session de bilan, et de base de discussion avec vous.
:::

## Comment allez-vous procéder ?

Cette mission suit un scénario de projet professionnel. Suivez les étapes dans l'ordre : elles construisent le livrable pièce par pièce.

Avant de démarrer, nous vous conseillons de :

- lire toute la mission et ses documents liés ;
- prendre des notes sur ce que vous avez compris ;
- consulter les étapes pour vous guider ;
- préparer une liste de questions pour votre session de mentorat.

:::encadre type=info
**Quelques conseils de méthodologie de travail :**

**Sur la construction du projet :** soyez prêt à détailler votre processus de décision. Exemples : pourquoi avoir testé cette classe au niveau unitaire plutôt qu'en intégration ? Quel risque précis chaque protection de sécurité couvre-t-elle ?

**Sur l'usage de l'IA :** si vous utilisez des outils d'intelligence artificielle, vous devez être capable d'expliquer comment vous avez vérifié et adapté le contenu produit. L'IA peut vous aider sur la syntaxe d'un test ou d'une configuration ; le choix de quoi tester et de comment sécuriser reste le vôtre, et c'est lui qu'on vous demandera d'expliquer.
:::

## Prêt à mener la mission ?

Vous êtes développeur back-end chez Norlys Mobilité, une PME qui gère des flottes de vélos et de trottinettes partagées pour les collectivités. Son application interne, une application C# construite avec ASP.NET Core, gère les stations, les véhicules et les comptes des agents de terrain. Elle a été développée vite, par une équipe réduite, sans tests automatisés.

Le mois dernier, deux régressions ont atteint la production, et un audit interne a pointé des lacunes de sécurité sur les données des agents. Ce matin, le comité technique a gelé toutes les nouvelles fonctionnalités. Thomas Verdier, le lead developer, doit présenter un plan de fiabilisation à la direction ; c'est vous qu'il choisit pour le mener.

:::artefact canal=email de="Thomas Verdier" fonction="Lead Developer" objet="Fiabilisation de l'app — c'est pour toi"
Salut,

Tu es au courant des deux régressions du mois dernier, et l'audit interne n'a rien arrangé côté sécurité. Le comité technique a tranché ce matin : on ne livre plus rien de neuf tant que l'existant n'est pas fiabilisé. Je te confie le chantier, tu as accès au repo de l'application.

Concrètement, voilà ce que j'attends :

1. Écris des tests automatisés sur les parties critiques de l'application, en suivant le plan de tests que je te joins. Commence par vérifier les règles métier une par une, puis assure-toi que les parcours complets tiennent la route, de la requête jusqu'à la base. Et donne-moi de quoi mesurer ce qui est couvert : je dois pouvoir montrer un chiffre à la direction, pas une promesse.
2. Corrige les problèmes décrits dans le brief sécurité joint. Les données des agents doivent être protégées, et personne n'accède à l'app sans s'identifier. C'est le point qui a fait le plus mal à l'audit.
3. Documente tes corrections dans le README : quelles failles tu as traitées, quelles protections tu as mises en place, et pourquoi. Le reste de l'équipe doit pouvoir te relire sans venir te poser la question.
4. Pousse le tout sur le repo, avec un historique de commits propre : je veux pouvoir suivre ton avancement au fil de l'eau.

On en parle jeudi si tu bloques.

Thomas
Lead Developer, Norlys Mobilité

**Pièces jointes :** plan de tests (plan_de_tests.md), brief sécurité (brief_securite.md)
:::

Cette mission est entièrement guidée. Suivez les étapes ci-dessous, elles vous mènent aux livrables pas à pas.

### Étape 1 - Écrivez des tests unitaires (.NET)

Un **test unitaire** vérifie une classe isolément, sans base de données ni serveur : uniquement sa logique. C'est la fondation de la pyramide de tests, et c'est par là que commence le plan de Thomas.

**Prérequis :**

- avoir cloné la codebase, l'avoir compilée avec `dotnet build` et l'avoir lancée avec `dotnet run` ;
- avoir repéré les couches de l'application (`Controllers`, `Services`, `Data`, `Models`) et lu le plan de tests fourni.

**Résultats attendus :**

- des tests unitaires écrits avec xUnit sur les classes désignées par le plan de tests, placés dans un projet de tests dédié (`.Tests.csproj`) en miroir des namespaces du code ;
- les dépendances de chaque classe testée remplacées par des doublures Moq (`Mock<T>`, `.Object`), de sorte qu'aucun test unitaire ne touche la base ;
- une suite qui passe au vert avec `dotnet test` ;
- un **rapport de couverture** généré avec Coverlet (`dotnet test --collect:"XPlat Code Coverage"`, résultat dans `TestResults/`), atteignant `[À COMPLÉTER : taux de couverture attendu sur le périmètre du plan de tests]`, avec un court commentaire dans le README sur ce que ce chiffre dit et ne dit pas.

**Recommandations :**

- Structurez chaque test en trois temps : préparation, action, vérification. Un test lisible se relit comme une phrase : « étant donné ceci, quand je fais cela, alors j'obtiens ça ».
- Les services de la codebase reçoivent leurs dépendances par constructeur : c'est précisément ce qui permet de les instancier dans un test avec des doublures, sans démarrer l'hôte ASP.NET Core. Profitez-en.
- Vous pouvez vous appuyer sur un outil d'intelligence artificielle pour écrire plus vite le squelette d'un test ; mais le choix des cas à couvrir (cas nominal, cas limite, cas d'erreur) vient du plan de tests et de votre lecture du code.

**Points de vigilance :**

- N'utilisez pas `WebApplicationFactory` à ce stade : cette classe démarre l'hôte ASP.NET Core, et un test qui charge toute l'application pour vérifier une règle de calcul est un test lent pour rien. Une doublure Moq suffit tant que vous testez une classe isolée.
- Un taux de couverture élevé ne prouve pas que les tests vérifient quelque chose : un test sans assertion couvre des lignes et ne teste rien. Relisez vos assertions avant de regarder le chiffre.

### Étape 2 - Écrivez des tests d'intégration et E2E (.NET)

Vos tests unitaires prouvent que chaque pièce fonctionne. Ils ne prouvent pas que les pièces s'emboîtent : c'est le rôle des **tests d'intégration**, qui traversent plusieurs couches, et des tests **end-to-end**, qui rejouent un parcours utilisateur complet.

**Prérequis :**

- avoir une suite de tests unitaires verte (étape 1) ;
- avoir identifié dans le plan de tests les parcours à vérifier de bout en bout.

**Résultats attendus :**

- des tests d'intégration écrits avec `WebApplicationFactory` et un `HttpClient`, vérifiant les parcours désignés par le plan de tests à travers les couches, du contrôleur au `DbContext` ;
- au moins un test exécuté contre une vraie base de données éphémère montée avec Testcontainers, plutôt que contre une base simulée ;
- au moins un scénario E2E rejouant un parcours complet du plan de tests, avec `[À COMPLÉTER : parcours E2E cible défini par la codebase, par exemple création puis consultation d'un véhicule par un agent authentifié]` ;
- une organisation du code qui matérialise la distinction entre tests unitaires, d'intégration et E2E (projets ou suffixes de nommage distincts) ;
- le rapport de couverture Coverlet régénéré, avec un commentaire sur ce que l'intégration a couvert que l'unitaire ne couvrait pas.

**Recommandations :**

- C'est ici que remplacer un service dans le conteneur devient légitime : `ConfigureTestServices` sur la `WebApplicationFactory` substitue un composant dans un hôte réel, par exemple pour neutraliser un appel externe dans un test d'intégration.
- Testcontainers a besoin d'un environnement Docker fonctionnel : vérifiez-le avant d'écrire le test, pas au moment de le lancer devant votre mentor.
- Un test d'intégration qui échoue est une information, pas un accident. Notez au moins un échec rencontré et sa résolution : vous en parlerez en session de bilan.

**Points de vigilance :**

- Si un test échoue sur une propriété de navigation à `null` ou sur une `ObjectDisposedException`, c'est qu'une entité EF Core est lue hors de la portée de son `DbContext` (un `Include` manquant, ou un contexte libéré trop tôt). Comprenez le mécanisme et corrigez-le en connaissance de cause ; contourner l'erreur au hasard la fera revenir en production.
- Ne dupliquez pas au niveau intégration ce que l'unitaire vérifie déjà : chaque niveau de la pyramide a son travail. Si vous ne savez pas dire pourquoi un test est un test d'intégration, il est probablement au mauvais étage.

### Étape 3 - Sécurisez l'application et mettez en place l'authentification (.NET)

Le brief sécurité de Thomas décrit ce que l'audit a trouvé. Votre suite de tests est verte : vous pouvez maintenant modifier le code sans crainte de casser en silence, et c'est exactement pour cela que la sécurisation vient en dernier.

**Prérequis :**

- avoir une suite de tests complète et verte (étapes 1 et 2) ;
- avoir lu le brief sécurité et repéré dans la codebase chaque point qu'il décrit.

**Résultats attendus :**

- les mots de passe des agents hachés avec le `PasswordHasher` d'ASP.NET Core Identity : plus aucun mot de passe en clair, ni dans le code, ni dans la base, ni dans les données d'initialisation ;
- une **authentification** fonctionnelle mise en place avec ASP.NET Core Identity : aucun accès à l'application sans s'être identifié, et une configuration (`AddAuthentication`, `UseAuthentication`, `UseAuthorization` dans `Program.cs`, attributs `[Authorize]`) que vous savez expliquer ligne à ligne ;
- les vulnérabilités décrites dans le brief sécurité corrigées : `[À COMPLÉTER : liste des vulnérabilités présentes dans la codebase de départ, par exemple endpoints non protégés, données des agents exposées, absence de validation des entrées]` ;
- dans le README : une courte note expliquant les vulnérabilités traitées et les protections mises en place, en reliant chacune à la catégorie OWASP correspondante (l'OWASP est le référentiel des risques de sécurité applicative les plus courants) ;
- la suite de tests toujours verte après vos modifications, ajustée là où l'authentification change le comportement des endpoints.

**Recommandations :**

- Vérifiez le hachage sur les données elles-mêmes : ouvrez la base et regardez la colonne des mots de passe. Un hachage Identity se reconnaît à sa forme de chaîne encodée en base64 ; un mot de passe lisible signifie que quelque chose contourne le hacheur.
- Les entrées utilisateur se valident avec les attributs de modèle (`[Required]`, `[StringLength]`) et la vérification de `ModelState` dans le contrôleur, pas avec des `if` en tête de méthode : la contrainte est déclarée là où la donnée entre.
- Rédigez la note du README au fil des corrections, pas à la fin : une faille, une protection, une phrase sur le risque couvert. C'est ce format que l'équipe pourra relire.

**Points de vigilance :**

- Ne laissez pas une entité EF Core traverser le contrôleur : exposez des DTO. Sinon le modèle de base fuit dans les réponses de l'API, et un champ sensible ajouté demain sera exposé sans que personne ne l'ait décidé ; c'est une faille de sécurité en devenir, pas un détail de style.
- Une exception métier se traduit en réponse HTTP dans un gestionnaire d'exceptions centralisé (`UseExceptionHandler` ou `IExceptionHandler`), pas dans le service : une erreur d'authentification qui laisse fuiter une stack trace renseigne un attaquant.
- Restez dans le périmètre du brief sécurité. Le déploiement, la supervision et la sécurisation globale de l'infrastructure viendront dans le projet suivant : ici, vous corrigez des failles identifiées, une par une, en comprenant chacune.

**Vérifications finales :**

- Complétez votre fiche d'autoévaluation pour identifier d'éventuels oublis avant l'échange avec votre mentor.
- Relancez `dotnet test` une dernière fois et régénérez le rapport de couverture : c'est l'état final du repo que vous présenterez.
- Relisez votre historique de commits : il doit raconter le chantier dans l'ordre où Thomas l'a demandé.

L'application que l'équipe n'osait plus toucher est redevenue un terrain sûr : chaque règle métier est gardée par un test, chaque accès par une authentification. C'est ce travail-là qui permettra à Norlys de livrer à nouveau.

---

# Renforcez vos connaissances

Dans ce projet, vous avez appris à écrire des tests unitaires, d'intégration et E2E, à interpréter un rapport de couverture, et à sécuriser une application en documentant vos protections.

La reformulation fait partie des techniques d'apprentissage qui fonctionnent et qui permettent de renforcer vos connaissances et compétences.

Nous vous proposons donc un outil qui vous permet de travailler cette reformulation et ainsi ancrer votre apprentissage.

Pour cela, vous allez pouvoir utiliser l'outil Companion, qui a été spécialement entraîné pour vous permettre de reformuler et d'affiner votre pensée. Cet engagement cognitif plus fort vous permettra de renforcer vos connaissances et d'être plus à l'aise lors de votre session de bilan.

Quand vous serez parvenu à une formulation claire et satisfaisante des notions que vous souhaitez retravailler (par exemple : la frontière entre test unitaire et test d'intégration, ce qu'un rapport de couverture mesure vraiment, la différence entre authentification et autorisation), faites une capture d'écran de la fin de la conversation et intégrez-la dans vos livrables.

Cliquez sur le bouton ci-dessous et commencez à échanger avec Companion.

:::encadre type=info
Si des notions du projet vous semblent suffisamment claires pour ne pas nécessiter d'échange d'approfondissement, cela signifie que vous vous sentez suffisamment solide pour présenter votre travail.
:::

---

# Livrables et session de bilan

## Livrables

1. **Repo GitHub** de l'application, contenant : le code des tests (unitaires, intégration et E2E, organisés distinctement), le rapport de couverture généré, le code sécurisé (mots de passe hachés, authentification en place, failles du brief corrigées), et dans le README une courte note expliquant les vulnérabilités traitées et les protections mises en place. L'historique de commits doit permettre de suivre l'avancement du chantier. Déposez le lien du repo dans un fichier texte.
2. **Capture(s) d'écran de vos échanges avec Companion** montrant votre compréhension des notions du projet que vous aviez besoin de retravailler.

:::encadre type=info
Déposez sur la plateforme, dans un dossier zip nommé **Titre_du_projet_nom_prénom**, tous les livrables du projet comme suit : **Nom_Prénom_n° du livrable_nom du livrable_date de démarrage du projet**.

Cela donnera :

- Nom_Prénom_1_lien_repo_github_mmaaaa
- Nom_Prénom_2_captures_companion_mmaaaa

Par exemple, le premier livrable peut être nommé comme suit : Dupont_Jean_1_lien_repo_github_012026.
:::

## Session de bilan

Ce projet ne fait pas l'objet d'une soutenance. Vous présentez votre travail à votre mentor accompagnateur, qui vous suit depuis le début du projet.

**Ce que vous montrez**

- Votre suite de tests, lancée en direct devant votre mentor, et le rapport de couverture ouvert. Vous commentez ce que chaque niveau de test vérifie.
- Votre authentification, démontrée en direct : un accès refusé sans identification, un accès accordé après.
- Votre note du README, présentée comme vous la présenteriez à l'équipe.

**Ce dont vous parlez**

- Vos choix de niveaux de test : pourquoi telle vérification est unitaire, telle autre en intégration.
- Un échec de test qui vous a résisté, et comment vous l'avez résolu.
- Les protections mises en place : quel risque chacune couvre, et ce qui se passerait sans elle.
- Ce sur quoi vous vous sentez encore fragile. Le dire ne vous pénalise pas : c'est ce qui rend la session utile.

**Ce que vous en retirez**

Votre mentor vous dira où vous en êtes par rapport au projet de synthèse qui suit, et ce qu'il vous conseille de consolider avant de vous y lancer.

:::encadre type=info
Comptez une trentaine de minutes. Cette session n'a pas de format imposé : venez avec votre repo ouvert, votre environnement prêt pour la démonstration, et vos questions.
:::

---
