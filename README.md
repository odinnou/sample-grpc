# sample-grpc
## Environnement de développement

Il faut au préalable installer Docker for Windows [https://hub.docker.com/?overlay=onboarding](https://hub.docker.com/?overlay=onboarding), pas besoin de Kubernetes, on peut laisser les configurations par défaut **il faut juste partager les X local drives de son poste de développement**.

Il faut ensuite créer le volume **grpc-sqldata** qui sera utilisa par le container postgres (pour ne pas perdre de données à chaque clean)

    docker volume create --name=grpc-sqldata

Le plugin VS Code **Docker** est plutôt pratique si on veut avoir une vue d'ensemble de nos containers et volumes ou s'il on souhaite éviter d'avoir à taper certaines lignes de commandes.

Puis, dans Visual Studio 2019 **Tools > Options > Container Tools > Docker Compose**, il est préférable de sélectionner ces options :

![disable run containers on startup](https://affix-test-api.phoceis.com/img/vs_config.png)