#### DESIGN ####
Es handelt sich bei meinem Projekt um eine 2-Tier Architektur (BusinessLayer, DataAccessLayer), außerdem gibt es dazu zusätzlich die BusinessObjects, auf die in der BusinessLogic zugegriffen werden. Ursprünglich habe ich für dieses Projekt eine 3-Layer Architektur gewählt (mit zusätzlicher Presentation Layer), habe aufgrund der erhöhten Komplexität allerdings darauf verzichtet. Der Datenbankzugriff findet mithilfe des sogenannten "Unit Of Work" und "Repository" Pattern statt: Hierfür definiere ich für die zugehörigen Tabellen eine Entity und dazu ein Repository, sodass mittels der zugehörigen Facade darauf zugegriffen werden kann.

Es schaut also folgendermaßen aus: Facade -> Repository -> Postgres DB

In der BusinessLogic habe ich den HTTP Server mittels HTTP Listener definiert (zunächst mittles TCP Listener, da uns die Verwendung vom HTTP Listener erlaubt wurde, habe ich dann darauf gewechselt).
In der BusinessLayer sind die Endpoints über Controller definiert, die gemeinsam auf Services zugreifen, diese sind als Singletons definiert und werden im Konstruktor des jeweiligen Controllers eingefügt.
Diese Art von Design habe ich ähnlich dem Framework AngularJS gewählt. 

Kurze Informationen zu den Features, die nicht rechtzeitig fertig geworden sind:

- Die gesamte Battle Logik ist implementiert und auch getestet, allerdings habe ich nicht gewusst, wie das Matchmaking zweier Spieler stattfinden soll und habe aus Zeitgründen einen der Spieler als NPC hardcoded, sodass man über HTTP Requests immer noch das Spiel simulieren kann.

- Außerdem habe ich das Tauschen der Spielkarten nicht implementiert.

#### LESSONS LEARNED ####
Ich darf auf jeden Fall das Projekt am Anfang nicht zu umfangreich und komplex gestalten, da dies mir im Nachhinein viel Zeit gestohlen hat, da ich zunächst die 3-Tier Architektur auf eine 2-Tier Architektur reduzieren musste, außerdem ist das Unit of Work und Repository Pattern sehr komplex und aufwendig mit den zusätzlichen Entities geworden, sodass dies ebenfalls sehr viel Zeit in Anspruch genommen hat.

Außerdem war ich mir nicht sicher, wie genau man Unit Tests für meine Controller und Services schreibt, besonders für jene Funktionen, die lediglich mit Datenbankzugriff zu tun haben und sonst keine weitere Logik besitzen, da ich noch kein tieferes Wissen zu Unit Testing und insbesondere Mocking von Klassen, Methoden etc. besitze.

Die Unit Tests insbesondere für dieses Projekt sind nur sehr mager meiner Meinung nach ausgefallen, das nächste Mal will ich dies ändern, eventuell sogar TestDriver Programmieren, sodass ich richtig und effizient Unit Teste und die Vorteile daraus beziehen kann.

#### UNIT TESTING DECISIONS ####

Die Unit Tests, die ich geschrieben habe, waren hauptsächlich für Funktionen/Methoden ohne Datenbankzugriff, da ich hier nicht genau wusste, wie diese zu testen sind. Insbesondere die vielen Außnahmen der Battle-Logik habe ich hier getestet, aber sonst auch einige andere Utility Funktionen der Services, wie das Hashen und vergleichen der Passwörter zum Login und Registrieren, aber auch das Generieren von Kartenpacketen etc.

#### UNIQUE FEATURE ####
Das Unique Feature ist bei mir relativ klein und unbedeutend ausgefallen, da ich vor der endgültigen Deadline relativen Zeitdruck hatte. Ich speichere zusätzlich zu einer ELO der Spieler auch noch die Anzahl der Siege, Niederlagen und Untentschieden in der Datenbank, die bei der Abfrage des eigenen Profils oder des Leaderboards mitgesendet werden.

#### TRACKED TIME ####
TCP Listener - 8h
Umschreiben auf HTTP Listener - 6h
Unit of Work und Repository Pattern - 16h
Session Endpoints für Login/Logout/Registrierung - 10h
Kaufen von Karten und hinzufügen zur Kollektion - 5h
Abrufen der eigenen Kollektion/Deck und konfigurieren des Decks - 8h
Abrufen des eigenen Profils und Leaderboards, konfigurieren des eigenen Profils - 4h
GameLogik - 5h
Unit Testing - 5h

#### GITHUB ####
Zwei Links, da ich ja aufgrund der Komplexität noch einmal neu begonnen habe:

Das aktuelle Projekt auf Abgabe-Stand
https://github.com/RaoulWo/TradingCardGame

Der erste Versuch
https://github.com/RaoulWo/MonsterTradingCardGame