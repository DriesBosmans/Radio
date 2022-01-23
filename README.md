# Project C# Mobile 2021-2022: RadioApp
* Naam: Dries Bosmans
* Onderwerp: <gekozen&goedgekeurd onderwerp>



![Schermafbeelding 2022-01-22 150922](https://user-images.githubusercontent.com/73179981/150642596-8a9966c9-8b06-4f1b-941d-e8979c2c0603.png)
![Schermafbeelding 2022-01-22 150739](https://user-images.githubusercontent.com/73179981/150642597-ab9c52fd-9cbe-4d06-b78d-2f10d0cdb4fe.png)
![Schermafbeelding 2022-01-22 150835](https://user-images.githubusercontent.com/73179981/150642601-014adc9e-abde-430e-8a82-5ed1efee7fad.png)
![Schermafbeelding 2022-01-22 150948](https://user-images.githubusercontent.com/73179981/150642594-d95c1afd-c840-44b0-8a3e-16406ad77834.png)

## Korte uitleg doel applicatie 
Mijn project wordt een app om radio te streamen en is gebaseerd op Radio Belgium van Simon Schellaert. Hij wordt voorlopig enkel voor android ontwikkeld. De app zal de volgende schermen bevatten:
•	Een mainpage: een collectionview van radiozenders met zoek-, en filterfunctionaliteiten (filter op taal en genre)
•	Een pagina met de favoriete zenders van de gebruiker
•	Een pagina met instellingen, waar ook de login in zal komen
•	Een detailpagina van de zender die momenteel gestreamd wordt
•	Een splash die in het androidgedeelte van het project wordt geladen
Navigatie zal gebeuren met tabview. Ik ben met flyout en shell begonnen, maar dit kon ik niet met tabview combineren dus ben ik herbegonnen.
De Api die hoofdzakelijk gebruikt wordt is DarFm. Ook die van Spotify zou ik mogelijks nog kunnen aanspreken. 
De data die wordt bijgehouden zijn naast de inloggegevens, een geschiedenis van beluisterde nummers met links naar youtube en spotify. Een geschiedenis van beluisterde zenders (+ aantal minuten geluisterd). Een lijst van favoriete zenders die de gebruiker zelf kan samenstellen. De regio van de gebruiker, zodat hij standaard enkel lokale zenders voorgeschoteld krijgt. Gebruikersgegevens zou ik opslaan op Firebase. Ik ga uitzoeken hoe het zit met afbeeldingen te cachen. Ik kan later eventueel uitbreiden naar podcasts, maar voorlopig ga ik het bij enkel radiozenders houden.
   
Het is mijn bedoeling om de best mogelijke app te ontwikkelen. Ik steek er op dit moment ook zowat al mijn tijd in. Twee schermen zijn min of meer klaar. Ik zit nu al aan 18 commits. MVVM zal volledig geïmplementeerd worden. 
De app van Simon Schellaert is al vrij uitgebreid. Qua functionaliteiten ga ik niet heel veel meer kunnen doen dan hij deed. In tegenstelling tot Schellaert ga ik wel voorzien dat de app ook werkt met andere zenders dan die uit België. Ook ga ik proberen een filterfunctie te voorzien en ga ik mijn app er proberen beter te laten uitzien dan die van Schellaert, die ziet er vrij basic uit. Tot slot zal er geen extra reclame gestreamd worden. Edit: Uiteindelijk ben ik dezelfde api gaan gebruiken als Schellaert, de reclame zit daar ingebakken.

## Toekomst

Ik ben van plan dit op Google Play te zetten na een paar aanpassingen. Het inloggen moet nuttig zijn of ik haal het er helemaal uit. De achtergronden kunnen duidelijker. Verder nog wat styling, het kan allemaal nog net iets beter. Ik ben met bluetooth bezig, maar is nog niet af. 
Het zou ook iets performanter mogen, het is te traag, al weet ik niet meteen hoe ik dit zou kunnen verbeteren.

## Inbegrepen

In dit project aanwezig: 8 schermen (splashscreen en popups meegeteld)
- Bevat een vorm van inloggen (Firebase Auth)
- Styles worden gebruikt (resourcedictionary)
- Twee CollectionViews met zelf gedefinieerde cellen
- er kan gefilterd en gezocht worden
- Veel databinding
- Settingspagina
- Preferences
- Compiled binding aanwezig
- Twee behaviors
- MVVM
- API
- Sqlite voor favorieten op te slaan
- Converter (stringlength naar fontsize)

Extra's
- Media-element
- Splash screen (SplashActivity.cs in Android) (wordt volledig overgeslagen om sneller te kunnen starten, maar het was een korte animatie)
- Messagingcenter
- Commands
- Magic gradients
- Button animations
- Popup vensters
- refreshview (compleet overbodig)
- Tabview navigatie
- custom renderers
- Css
- Controls
- Custom font
- Meertalig


## Logboek

28-10-2021
Ik ben op 18 oktober aan mijn project begonnen. 
Ik heb 7 commits gedaan op mijn eerste repo, en 7 op mijn tweede. Mijn eerste project was gebouwd in een shell,
tot bleek dat ik geen tabview kon gebruiken, dus ben ik een nieuw project begonnen zonder shell.
Mijn eerste versie was gebouwd rond de api van radio-browser.info. Er ontbraken veel afbeeldingen.
Ik heb contact opgenomen met de eigenaar of ik ze kon aanvullen. Dit kon niet, dus heb ik gezocht naar een nieuwe api.
Die heb ik gevonden: OnRad.io. Tot nu toe heb ik een splash en een collectionview van radiozenders.

28-10-2021
Ik heb aan de zoekbalk in het mainwindow gewerkt.

29-10-2021
De zoekfunctie is in orde. Nu ga ik kijken of ik ook een filterfunctie kan fixen.
Expander krijg ik niet geregeld, ik ga proberen settings te maken in een pop up.

31-10-2021
Ben twee dagen aan de filter functie bezig geweest, maar het werkt nu

22-11-2021
Joke geholpen

6-12-2021
Debuggen: error ivm databinding

20-12-2021
Api probleem opgelost, media-element geimplementeerd.

26-12-2021
Ben gestopt met het gebruik van het MediaElement van Xamarin communitytoolkit wegens niet mogelijk om op achtergrond te spelen
en verschillende formaten niet ondersteund.
Nu gebruiken we de Xamarin Media Manager, wat geen view is, dus werken we met custom transportknoppen.

27-12-2021
Verder gewerkt aan mediamanager

28-12-21
Authenticatie met firebase gemaakt. Registreren, inloggen en uitloggen lukt. Het is mij verder niet gelukt hier nog iets anders
mee te doen. Hier ben ik lang aan bezig geweest.

1-1-22
Lijst met favorieten toegevoegd. Favorieten kunnen aangeduid worden met dubbeltap

13-1-22 SQLite database toegevoegd om favorieten op te slaan.

15-1-22 SQLite database verder uitgewerkt

16-1-22 Na verschillende pogingen een picker met landen te installeren ben ik teruggevallen op de countrypicker, die ik ooit heb
gebruikt om de popup vensters te maken. Met weinig aanpassingen heb ik deze kunnen implementeren in de radio-app.
Nu kunnen radiozender van over de hele wereld opgezocht worden.
Daarnaast heb ik de solution wat opgeruimd, summaries gemaakt en voorzien van commentaar.
Ik twijfel eraan om VLC media player nog te installeren, omdat niet alle mediaformaten ondersteund worden. 

Verder rest enkel nog een beetje styling, het zou er allemaal nog wat beter mogen uitzien.

Het is mij gelukt om de gradient achtergronden in een apart css-bestand te zetten

17-1-22 Verder aan styling gewerkt en knopanimaties verzorgd
VLC mediaplayer geinstalleerd wegens ondersteunt meer streamingformaten

18-1-22
Commentaar toegevoegd
Begonnen aan overgang naar tweetalig

21-1-22
De app is nu drietalig. Het brengt weinig werk met zich mee om meerdere talen toe te voegen. Achtergronden ook trouwens.

22-1-22
De video opgenomen. Ik ken wel wat van audio en heb er ook wat mee gespeeld. De bron van de muziek komt van de app(op het einde van het clipje hakkelt hij) maar is niet live. De zender in kwestie is er een uit Chicago. Ik zocht iets jazzy maar hetgeen er op nostalgie bezig was, was mij te druk.

23-1-22
Laatste commit voor de review
Heb er toch nog een converter in verwerkt (fontsize past zich aan aan de lengte van de string)

## Link video

https://www.youtube.com/watch?v=JrWUEvThohs

## Bronnen

UI design inspiration
grialkit.com

api
https://api.radio-browser.info/ (oude)

Colors
https://coolors.co/a53182-6c38d7-5e1dae-ffffff-39116e
https://devblogs.microsoft.com/xamarin/xamarinforms-4-8-gradients-brushes/
https://www.gradientmagic.com/
Magic gradients NuGet package

Animated background (werd enkel in het begin gebruikt, vertraagd de app te veel)
https://www.youtube.com/watch?v=ejDwwc1Iq1s&ab_channel=OludayoAlli

set background status bar
https://stackoverflow.com/questions/37993741/xamarin-forms-change-statusbar-color

Fixed navigation issue
https://www.youtube.com/watch?v=f1uYhDjvem4&ab_channel=DavidOrtinau

Animated Image
https://www.andrewhoefling.com/Blog/Post/scaling-animations-in-xamarin-forms

xamarin community toolkit
https://docs.microsoft.com/en-us/xamarin/community-toolkit/

ffimageloading library
https://devlinduldulao.pro/how-to-use-ffimageloading/

ObservableCollection.AddRange()
https://nugetmusthaves.com/Package/RangedObservableCollection

RefreshView
https://devblogs.microsoft.com/xamarin/refreshview-xamarin-forms/

SplashScreen
https://www.serkanseker.com/animated-splash-screen/
https://lottiefiles.com/81568-vpn-metro
https://lottiefiles.com/82130-radio

MVVMhelpers
https://github.com/jamesmontemagno/mvvm-helpers

json to csv voor data analyse
https://www.convertcsv.com/json-to-csv.htm

NEW API
https://docs.google.com/document/d/12HNoXI-z40QLQiSi30g2qSWb8U9YbMd4u5_jrre-YTw/edit

custom search bar
https://gist.github.com/kevinmutlow/77cf36ff2b3ddeabddeda5115020bb8e
custom renderer
https://www.youtube.com/watch?v=iRwR87j3Sfc&ab_channel=SPTutorials
remove searchbar underline
https://stackoverflow.com/questions/42845478/xamarin-remove-searchbar-underline-in-android
Search method
https://adityadeshpandeadi.wordpress.com/2018/01/14/search-through-listview-items-in-xamarin-forms/

orkney font (niet meer gebruikt, ik gebruik Quicksand nu)
https://www.1001fonts.com/orkney-font.html

Popup page and country picker
https://medium.com/nerd-for-tech/xamarin-forms-country-picker-with-rg-plugins-popup-fec1a045f7c

Xamarin Media Manager: (niet meer gebruikt, overgestapt op VLC)
https://github.com/Baseflow/XamarinMediaManager

Firebase
firebase.google.com
https://www.youtube.com/watch?v=liGTuuau29Y

Adding database with SQLite-net
https://www.youtube.com/watch?v=XFP8Np-uRWc

Button animations
https://xamgirl.com/plug-and-play-animations-in-xamarin-forms/

vlc mediaplayer
https://github.com/coolc0ders/VLCXamarinDemo/tree/main/VLCDemo/VLCDemo

Selecting other languages
https://www.youtube.com/watch?v=-t8sss0BHqo

Converter
https://askxammy.com/learning-about-converters-in-xamarin-forms/

## Eerdere commits

Commits on Oct 29, 2021
deleted the expander

Worked on the expander with filter options

Implemented search functionality

Commits on Oct 28, 2021
worked on the searchbar

Commits on Oct 27, 2021
Implemented new api DarFMApi
 
Commits on Oct 26, 2021
Deleted some commented code
 
implemented pull to refresh
 
created a new splash screen, migrated to a more mvvm approach
 
Commits on Oct 23, 2021
added refreshview, started migration to mvvm

Commits on Oct 21, 2021
Started working on the collectionview

Add project files.
 
Add .gitignore and .gitattributes.

Commits on Oct 20, 2021
Been working on the tabbar. It turned out I can't use TabView with Sh… …
 
Animated image zoom
 
Splashpage tweaked
 
Commits on Oct 19, 2021
styled splash page

Worked on colors

Add project files.
 
Add .gitignore and .gitattributes.

