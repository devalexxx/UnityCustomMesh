# TP1

Il y a un script par mesh, chaque script a des paramètres pour configurer ce même mesh.

Chaque script peut s'attacher à un GameObject, à condition qu'a celui-ci soit attché deux composants. Le Mesh Renderer et le Mesh Filter.

Dans la scene, un GameObject nommé MeshScriptHolder est présent. Il comporte tous le néscessaire ainsi que tous les scripts de ce TP. Vous avez simplement à activer le script voulu et lancé le jeu.

## Plan

Un simple script qui affiche un carré.

## SizeablePlan

Un script qui affiche un plan de size configurable et prend 2 paramètres:
- nRow: nombre de lignes (uint > 0)
- nCol: nombre de colonnes (uint > 0)

## Cylinder

Un script qui affiche un cylindre et prend 3 paramètres:
- radius: Le rayon du cylindre (float > 0)
- height: La hauteur du cylindre (float > 0)
- nMeridians: Le nombre de méridiens du cylindre (int >= 3)

## Sphere

Un script qui affiche une sphère et prend 3 paramètres:
- radius: Le rayon de la sphère (float > 0)
- nMeridians: Le nombre de méridiens de la sphère (int >= 3)
- nParallels: Le nomvre de parallèles de la sphère (int >= 2)

## Cone

Un script qui affiche un cône et prend 3 paramètres:
- radius: Le rayon du cône (float > 0)
- height: La hauteur du cône (float > 0)
- nMeridians: Le nombre de méridiens du cône (int >= 3)

## TruncatedCone

Un script qui affiche un cône tronqué et prend 4 paramètres:
- ... (cf. Cone)
- cuttingHeight: La hauteur à laquelle le cône est coupé (0 < float < height)

## TruncatedSphere (WIP)

Un script qui affiche une sphère tronqué et prend 4 paramètres:
- ... (cf. Sphere)
- nCuttingMeridians: Le nombre de méridiens à tronquer (0 < int < nMeridians - 1)
