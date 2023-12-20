
![370294914_204744886047728_8815547702196559059_n](https://github.com/Wael145/IslandGenerator/assets/62157910/e5b1c8e4-17b7-48fb-8050-08ce3743572d)

<div>
<h1>Mondes Virtuels : Island Procedural generation with unity</h1>	
Il s'agit de la génération procédurale d'une île et gèrer l'emplacement des objets(plantes, pierres et arbres) sur ce terrain. Ce travail est dans le cadre du module mondes virtuels de la formation GAMAGORA.

<p>La génération procédurale de terrains(les îles dans notre cas) est une approche qui sert à créer des environnements variés,qui sont généralement plus difficile à faire sans code. Elle permet aussi de gagner du temps dans le processus de conception. On a trouvé pas mal de travaux qui générent des terrains procéduralement, on s’est inspriré de quelques uns pour faire notre propre travail.</p>
</div>
<div>
  lien des slides :
	https://docs.google.com/presentation/d/1tQceaV6BTEazL69P0cvXhuQOGixocn1JxH6T-lcDPy4/edit#slide=id.p
	
  lien de la vidéo : 

</div>
<div>
  <h3>Terrain Generation :</h3>
    <br><br>
La génération procédurale de terrains peut être réalisée à l'aide de nombreux algorithmes. Le choix a été fait ici d'utiliser l'algorithme du bruit de Perlin afin de générer un terrain en 3 dimensions. L'avantage du bruit de Perlin est qu'il permet de générer une grille de valeurs continues, ce qui est très pratique pour ensuite générer un terrain cohérent. 
<br><br>

La première étape a donc été de générer une texture 2D sur Unity. Pour cela, on crée un tableau à deux dimensions et chaque case du tableau contiendra un nombre flottant entre 0 et 1 calculé à l'aide de la fonction PerlinNoise de Unity. A partir de ce tableau, on peut générer une texture en deux dimensions en nuances de gris en associant à chaque point du tableau une couleur grise proportionnelle à la valeur en sortie de la fonction PerlinNoise.

<img src = "https://github.com/Wael145/IslandGenerator/assets/144930233/0546fa90-8d28-4820-bef7-302cb70b4263" alt = "Texture 2D en nuances de gris" width = 200>


Nous avons donc à disposition une texture en deux dimensions issu d'un tableau à deux entrées. On veut alors obtenir une texture en couleurs. Pour cela, on peut associer à chaque valeur du tableau une couleur. On peut modifier ces plages de valeur dans l'inspecteur de Unity. Un exemple de valeurs qu'on utilise pour obtenir la texture d'un terrain avec des étendues d'eau et des montagnes :

<img src = "https://github.com/Wael145/IslandGenerator/assets/144930233/3295bb69-e4a6-419e-a40e-c685c7ee4727" width = 200>


Ces régions indiquent que pour une hauteur entre 0 et 0.3, la couleur affichée sera du bleu foncé, puis entre 0.3 et 0.4 ce sera du bleu un peu plus clair etc. On peut changer chaque couleur dans l'inspecteur, ajouter de nouvelles régions ou en supprimer et changer les valeurs des hauteurs minimum et maximum.
Cela permet d'obtenir une image comme cela.
<img src = "https://github.com/Wael145/IslandGenerator/assets/144930233/448d0c0b-d5f5-492c-8a9d-74327ffec0c9" width = 200>


On peut ensuite complexifier le code du bruit de Perlin afin d'ajouter divers paramètres pour avoir une carte avec plus de substance et qui semble plus naturelle. On a alors des paramètres en plus dans l'inspecteur que l'on peut faire varier : le nombre d'octaves, la persistance et la lacunarité. Ces trois paramètres vont en quelque sorte rajouter un bruit par dessus le bruit de Perlin initial et cela donne le résultat suivant : on voit que les plages des îles ont des formes plus naturelles.

<img src = "https://github.com/Wael145/IslandGenerator/assets/144930233/b188c61a-26c2-4b88-93d9-43e925b9cd4f" width = 200>

<div>
<h1>Création du maillage :</h1>

Après avoir obtenu une texture satisfaisante en couleurs, on peut générer le maillage de montagnes assez simplement. Pour cela il suffit de créer un plan en 2D de la même taille que la texture, et à chaque sommet du maillage, on lui donne une hauteur proportionnelle à la valeur du bruit de Perlin en ce point. On peut ensuite appliquer la texture créée précédemment sur le maillage pour obtenir le rendu suivant : 

<img src = "https://github.com/Wael145/IslandGenerator/assets/144930233/309699ac-ef88-4e82-8ddc-6bf96f3ce887" width = 300> <img src = "https://github.com/Wael145/IslandGenerator/assets/144930233/97386c42-1e95-4699-a2be-3204d6f1bf8d", width = 300>




 
</div>

</div>


<div>
  <h3>Objects Placement :</h3>
   


</div>
Le placement des objets est géré par un bruit de perlin pour calculer la densité initiale, ensuite par un multiplicateur correspondant à la hauteur du terrain.

La hauteur du terrain = noiseMap[i,j] et 0<hauteur<1

<img src="https://github.com/Wael145/IslandGenerator/assets/62157910/4c857340-0283-43d7-b567-e9b1dd4a86ed">

-Le nombre d'arbres est plus grand quand la heuteur est minimale, et plus petit dans le cas contraire, (jusqu'à 0 arbres sur la neige).

-Les rochers sont placés uniquement sur la hauteur moyenne du terrain(hauteur >= 0.65 && hauteur < 0.8)

-Les plantes sont placées dans les parties vertes du terrain(hauteur<0.6)

-On ne peut pas positionner des objets sur l'eau et sur les sable

-Nous pouvons résumer les méthodes utilisées pour chaque objet par le diagramme suivant : 

<img src="https://github.com/Wael145/IslandGenerator/assets/62157910/d64815e6-9be1-43e7-a69b-f4b4f9a65595">

-Exemple de code pour le placement d'arbres : 

<img src="https://github.com/Wael145/IslandGenerator/assets/62157910/2d0aa3af-a3d1-4f5b-b29a-956d49205f96">
<div>
  <h3>How to run</h3>
<p>Version Unity : LTS Release 2022.3.* (Long-term support)
	
Ouvrir git bash et écrire :
	
	git clone https://github.com/Wael145/IslandGenerator.git
Ou simplement télécharger le projet en archive et extraire les fichiers. 

<h3>Tips</h3>
il faut modifier les valeurs des densités avant de lancer le play mode.Elle ne se modifient pas en temps réel.

Sachant que  :

La hauteur est entre 0 et 1.

Level1TreeDensity veut dire que 0.45<hauteur<=0.6

Level2TreeDensity veut dire que 0.6<hauteur<8

Level3TreeDensity veut dire que 0.8<=hauteur

<img src="https://github.com/Wael145/IslandGenerator/assets/62157910/ce81eb15-5d30-4650-b508-8adda0c09c5d">

Si le terrain a été modifier vous pouvez replacer tout les objets en cliquant sur le bouton Update Placement
<img src="https://github.com/Wael145/IslandGenerator/assets/62157910/20817b2a-19e2-443f-8733-c80cc51d0465">
</p>
</div>
<div>
<h3>Réferences :</h3>
-Procedural Landmass Generation
https://www.youtube.com/watch?v=wbpMiKiSKm8
	
-Perlin noise and Unity
https://www.youtube.com/watch?v=BO7x58NwGaU

-Colours & Spawning random objects
https://www.youtube.com/watch?v=bd4P5suj-L0

-Volcanoes
https://www.world-machine.com/tutorials/volcano/volcano.html
<h3>Assets :</h3>
<p>
	-Trees : https://assetstore.unity.com/packages/3d/vegetation/trees/low-poly-tree-pack-57866
	<br>
	-Rocks : https://assetstore.unity.com/packages/3d/environments/landscapes/lowpoly-style-free-rocks-and-plants-145133
	<br>
	-Plants : https://assetstore.unity.com/packages/3d/vegetation/simple-low-poly-decorative-plant-assets-252714
</p>  
	<br><br>
</div>
