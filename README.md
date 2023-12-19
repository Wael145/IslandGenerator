
![370294914_204744886047728_8815547702196559059_n](https://github.com/Wael145/IslandGenerator/assets/62157910/e5b1c8e4-17b7-48fb-8050-08ce3743572d)

<div>
<h1>Island Procedural generation with unity</h1>	
 Il s'agit de la génération procédurale d'un terrain et gèrer l'emplacement des objets sur ce terrain. Ce travail est dans le cadre du module mondes virtuels de la formation GAMAGORA.


<p>La génération procédurale de terrains(les iles dans notre cas) est une approche qui sert à créer des environnements variés,qui sont généralement plus difficile à faire sans code. Elle permet aussi de gagner du temps dans le processus de conception. On a trouvé pas mal de travaux qui générent des iles procéduralement, on s’est inspriré de quelques uns pour faire notre propre travail.</p>
</div>
<div>
  lien des slides :
	https://docs.google.com/presentation/d/1tQceaV6BTEazL69P0cvXhuQOGixocn1JxH6T-lcDPy4/edit#slide=id.p
	
  lien de la vidéo : 

</div>
<div>
  <h3>Terrain Generation :</h3>
    <br><br>
<img src = "https://github.com/Wael145/IslandGenerator/assets/144930233/0546fa90-8d28-4820-bef7-302cb70b4263">
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
