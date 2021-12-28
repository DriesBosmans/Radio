# Project C# Mobile 2021-2022: RadioApp
* Naam: Dries Bosmans
* Onderwerp: <gekozen&goedgekeurd onderwerp>
## Korte uitleg doel applicatie (uiteindelijk aangevuld met screenshots)
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
De app van Simon Schellaert is al vrij uitgebreid. Qua functionaliteiten ga ik niet heel veel meer kunnen doen dan hij deed. In tegenstelling tot Schellaert ga ik wel voorzien dat de app ook werkt met andere zenders dan die uit België. Ook ga ik proberen een filterfunctie te voorzien en ga ik mijn app er proberen beter te laten uitzien dan die van Schellaert, die ziet er vrij basic uit. Tot slot zal er geen extra reclame gestreamd worden. 

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

## Link video
## Bronnen
UI design inspiration
grialkit.com

api
https://api.radio-browser.info/

Colors
https://coolors.co/a53182-6c38d7-5e1dae-ffffff-39116e
https://devblogs.microsoft.com/xamarin/xamarinforms-4-8-gradients-brushes/
https://www.gradientmagic.com/
Magic gradients NuGet package

Animated background
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

for data analysis
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

orkney font
https://www.1001fonts.com/orkney-font.html

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

