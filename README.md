# Submarin Game
## Feladat
Készítsünk programot a következő játékra.
A játékban egy tengeralattjárót kell irányítanunk a képernyőn (balra, jobbra, fel, illetve le), amely
felett ellenséges hajók köröznek, és folyamatosan aknákat dobnak a tengerbe. Az aknáknak három
típusa van (könnyű, közepes, nehéz), amely meghatározza, hogy milyen gyorsan süllyednek a vízben
(minél nehezebb, annál gyorsabban).
Az aknákat véletlenszerűen dobják a tengerbe, ám mivel a hajóskapitányok egyre türelmetlenebbek,
egyre gyorsabban kerül egyre több akna a vízbe. A játékos célja az, hogy minél tovább elkerülje az
aknákat. A játék addig tart, ameddig a tengeralattjárót el nem találta egy akna.
A program biztosítson lehetőséget új játék kezdésére, valamint játék szüneteltetésére (ekkor nem
telik az idő, és nem mozog semmi a játékban). Ismerje fel, ha vége a játéknak, és jelenítse meg,
mennyi volt a játékidő. Ezen felül szüneteltetés alatt legyen lehetőség a játék elmentésére, valamint
betöltésére.
## A feladat elemzése
- A játékban egy játkos vesz részt, valamint egy nehézségi szint van, ugyanakkor az aknák
három típus valamelyikét képviselik: könnyű, közepes, nehéz.
- A program indításkor automatikusan új játékot kezd, és hat darab véletlenszerűen generált
nehézségű aknát indít a pálya aljáról induló tengeralattjáróra.
- A feladatot egyablakos asztali alkalmazásként Windows Presentation Foundation (WPF)
grafikus felülettel valósítjuk meg.
- Az ablakban elhelyezünk egy menüt egyetlen menüponttal: File (New game, Load game…,
Save game…, Exit). Az ablak alján pedig a státuszsor kap helyet, amely az eltelt időt, valamint
a már felrobbant aknák számát jelzi.
- Mind a tengeralattjárót, mind az aknákat képszerűen jelenítjük meg, és a tengeralattjáró
billentyűzeten történő gombnyomásra (ez történhet a nyilakkal, vagy az AWSD betűkkel)
változtatja a pozícióját a pályán belül. Ezen felül a Space lenyomására tudjuk szüneteltetni a
játékot, ekkor egy dialógusablak felugrik, valamint az idő és a játék menete megáll. Az ablak
lezárása után a játék folytatódik.
- A játék automatikusan feldob egy dialógusablakot, amikor vége a játéknak, ezen megjelenik
az eltelt idő, valamint a felrobbant aknák száma. Elfogadása után új játék kezdődik.
- Szintén dialógusablakkal végezzük el a mentést, illetve a betöltést, a fájlneveket a
felhasználó adja meg. Valamint a játék menüsorból való bezárása esetén is egy dialógusablak
bizonyosodik meg a szándékunkról.
## Tervezés
### Programszerkezet:
- A programot háromrétegű (perzisztencia réteggel együtt négyrétegű)
architektúrában valósítjuk meg. A megjelenítés a **SubmarineGame.View**, a
nézetmodell a **SubmarineGame.ViewModel**, a modell a **SubmarineGame.Model**,
míg a perzisztencia a **SubmarineGame.Persistence** névtérben helyezkedik el.
Továbbá a rétegeket külön projektként adjuk hozzá az újrafelhasználhatóság
érdekében.
### Perzisztencia:
- Az adatkezelés feladata a tengeralattjáró, valamint az aknák helyzetével,
tulajdonságaival kapcsolatos információk tárolása, valamint a betöltés/mentés
biztosítása
- A **Shape** osztály valósítja meg a tengeralattjáró, valamint az aknák típusát, tárolja a
koordinátákat (**X**, **Y**), méreteket (**Width**, **Height**), az aknák esetén a súlyt (**Weight**),
valamint, hogy aknáról vagy tengeralattjáróról van-e szó (**ShapeType.Submarine**,
**ShapeType.Mine**)
- A hosszú távú adattárolás lehetőségeit az **IPersistence** interfész adja meg, amely
lehetőséget ad a játékállás (vagyis a tengeralattjáró, valamint aknák
tulajdonságainak) betöltésére (**Load**), valamint mentésére (**Save**)
- Az interfészt szöveges fájl alapú adatkezelésre a **TextFilePersistence** osztály
valósítja meg. A fájlkezelés során fellépő hibákat a **DataException** kivétel jelzi.
- Program az adatokat szöveges fájlként tudja eltárolni, melyek az **smg** kiterjesztést
kapják. Ezeket az adatokat a programban bármikor be lehet tölteni, illetve ki lehet
menteni az aktuális állást.
- A fájl első sora megadja a játékidőt, valamint a már felrobbant aknák számát. A
második sora a tengeralattjáró adatait, a többi sora pedig az aknák adatait,
szóközökkel elválasztva. Az első soron kívül mindegyik sor 6 számot tartalmaz (típus,
x, y, szélesség, magasság, súly).
### Modell:
- A modell lényegi részét a **SubmarineGameModel** osztály valósítja meg, amely
beállítja a megfelelő koordinátákat (ellenőrzés után), valamint a többi paramétert,
mint például a játékidő (**_gameTime**), felrobbant aknák száma
(**_destroyedMineCount**). A típus lehetőséget ad új játék kezdésére (**NewGame**),
valamint lépésre a tengeralattjáróval (**Submarine_MoveUp**,
**Submarine_MoveDown**, **Submarine_MoveLeft**, **Submarine_MoveRight**), valamint
beállítja az aknák koordinátáit (**MoveMines**), és generálja őket
(**GenerateStartingMines**, **AddMine**). Ezen túl a játékot is ellenőrzi minden
koordinátaváltozás után (**CheckGame**).
- A tengeralattjáró koordinátaváltozásáról a **SubmarineMoved** esemény, egy akna
pályaelhagyásáról a **MineDestroyed** esemény, koordinátaváltozásáról a
**MineMoved** esemény, új akna generálásáról a MineAdded esemény, míg a játék
végéről a **GameOver** esemény, a játék szüneteltetéséről a **TimePaused** esemény,
játék kezdéséről pedig a **GameCreated** esemény tájékoztat. Ezen kívül a játékidő
múlásához is bevezettük a **GameTimeElapsed** eseményt. Az események
argumentuma (**SubmarineEventArgs**) tárolja a játékidőt, a már felrobban aknák
számát, valamint a mozgás megfelelő irányát.
- A modell példányosításkor megkapja az adatkezelés felületét, amelynek
segítségével lehetőséget ad betöltésre (**LoadGame**) és mentésre (**SaveGame**). Ezt a
**_persistence** adattagban tárolja.
### Nézetmodell:
- A nézetmodell tulajdonképpen a modell és a nézet közötti kommunkációt és
megjelenítést biztosítja a **SubmarineGameViewModel** osztály megvalósításával. Ez
az osztály a **ViewModelBase** interfészt valósítja meg, ami pedig az
**INotifyPropertyChanged** osztály leszármazottja. Aggregálja a modellt, valamint
parancs- (**NewGameCommand**, **LoadGameCommand**, **SaveGameCommand**,
**ExitCommand**) és adatkötésekkel (**Mines**, **Submarine**, **DestroyedMineCount**,
**GameTime**) kommunikál a nézettel.
- Az **App** osztállyal való kommunikáció végett rendelkezik a **NewGame**, **LoadGame**,
**SaveGame** és **ExitGame** eseményekkel, amelyek a nevükhöz megfelelő események
kiváltásáért felelősek.
- Az aknákat a Mines megfigyelhető kollekcióval jeleníti meg, valamint egy akna
típusa a perzisztenciában található Shape osztályhoz nagyon hasonló **Shape**. A
tengeralattjáró számára egy külön osztály lett létrehozva (**Submarine**), ami a Shape
osztályból származik le, hozzáadva a **StepCommand** parancsot.
- A **DelegateCommand** osztály az **ICommand** interfészt valósítja meg, és biztosítja a
parancsok általános leírását.
- A nézetmodell egyetlen privát metódusa a **SubmarineStep(String)** metódus, amely
a billentyűlenyomásokra reagálva meghívja a megfelelő modell-beli metódusokat a
tengeralattjáró mozgatására
- Tartalmaz eseménykezelőket is, amelyek a modell eseményeit kezelik le. Ilyen a
**Model_SubmarineMoved**, **Model_MineMoved**, **Model_MineDestroyed**,
**Model_MineAdded**, **Model_GameCreated** és a **Model_GameTimeElapsed**
### Nézet:
- A nézetet a **MainWindow** osztály biztosítja, amely XAML leíró nyelv segítségével lett
megvalósítva
- Parancs- és adatkötésekkel kommunikál a nézetmodellel a már fent említett módon
- Az ablak nem átméretezhető
- Az ablak három fő részre bontható:
  - Az egy menüpontot tartalmazó menü (**Menu**), melynek alpontjai: **New
game**, **Load game…**, **Save game…**, **Exit**
  - A játékteret biztosító **Canvas** vezérlő, amely tartalmazza a tengeralattjárót
megvalósító **Image** vezérlőt, valamint egy **ItemsControl**-t a dinamikusan
megjelenítendő aknák számára
  - A státuszsor (**Statusbar**), amely az eltelt játékidőt és a már felrobbant aknák
számát jeleníti meg
### Alkalmazás (környezet):
- A modell, nézetmodell, nézet és perzisztencia közötti kapcsolatot teremti meg,
valamint tartalmazza a megfelelő időzítőket, és lekezeli a nézet (**View_Closing**), a
modell (**Model_GameOver**, **Model_TimePaused**) és a nézetmodell
(**ViewModel_NewGame**, **ViewModel_LoadGame**, **ViewModel_SaveGame**,
**ViewModel_ExitGame**) által kiváltott eseményeket
