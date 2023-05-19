# IAV23-AntonovaMihaylova
Simona Antonova Mihaylova


# El Rey Goloso

## Propuesta
Para el proyecto final de la asignatura de Inteligencia Artificial 2023 he decidido hacer un divertido juego llamado "El Rey Goloso", en el que el jugador es un intrépido aventurero en el Reino de Golosus. El objetivo principal del juego es ayudar al rey a satisfacer sus antojos de tartas y ramos de flores, recolectando los objetos necesarios para que el panadero y el florista puedan hacer los pedidos necesarios dentro del tiempo otorgado.
 

Los personajes principales son:
- Player: es el encargado de recoger una variedad de flores exóticas y huevos de gallinas mágicas, asi como llevar al rey sus deliciosas tartas y sus preciosos ramos.
- Panadero: NPC que recoge trigo y huevos para hacer tartas, galletas o pan.
- Fantasma: NPC que recoge flores para adornar los ramos.
- Rey Golosus: NPC dorado que hace el pedido de tartas y ramos.

He utilizado múltiples máquinas de estados para programar la inteligencia artificial de los NPC, así como para el crecimiento y generación de recursos, y la gestión de objetos.

## Punto de partida
Para el mapa he usado algunos assets de la [práctica 3](https://github.com/IAV23-G15/IAV23-G15-P3), asi como otros recursos gratuitos del Asset Store de Unity, y algunos modelos gratuitos de Sketchfab.

## Diseño de la solución

Para el movimiento de todos los personajes he añadido el sistema de navegación de Unity. Hay 4 zonas delimitadas como áreas: el castillo, la panadería, la floristería y el resto de espacio para el player.

![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/074b295e-6074-4de6-a49b-034f01b82499)
>1. Panadería, 2. Castillo, 3. Floristería, 4. Walkable

A continuación, explicaré las máquinas de estados que he implementado:

**Panadero y Florista**

Ambos tiene una máquina muy similar, que difiere en algunas posiciones a las que van.

- La máquina de estados es la siguiente:

> Cada NPC tiene un estado en el que espera a recibir todos los ingredientes de la receta, y posteriormente va a las diferentes posiciones (nevera, mesa, etc.) para "preparar" el producto y finalmente el ultimo estado en el que se entrega el producto al jugador.
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/80da3b47-1316-4fc8-ae23-68351be431b1)

- Estado Esperando Ingredientes:

> Al entrar en el estado se comprueba si ya estan hechos todos los productos. Si es así el NPC se pone a merodear. Si no, merodea mientras que dice el ingrediente que necesita. Sólo acepta el correcto, si se le ofrece uno que no quiere se enfada. Para pasar al siguiente estado se tienen que recoger todos los ingredientes para la receta.
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/73addc8d-35a0-4065-8d39-91ca7605a338)

- Estados Ir Nevera, Ir Mesa, Ir Horno, Producto Terminado, etc.

> Funcionan igual, se le añade como destino la posición del objeto al que tiene que ir. Pasa de estado cuando se cumpla el cooldown de cada uno.
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/567791a0-a03d-4df7-9d94-3371ebd0285d)

**Flores, trigo y huevo**

- La máquina de estados es la siguiente:

> Primero las plantas crecen a lo largo de varios segundos, pasan a crecidas, y si el jugador las coge vuelven al estado de crecer.
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/cca5adb0-9ec1-41b8-b7b0-4f61bc1107f8)

- Estado Growing:
> El collider esta desactivado para que el player no pueda recoger la flor mientras esta creciendo. A continuación se hace un bucle para simular el crecimiento a través del aumento de la escala cada 1 segundo hasta llegar a la escala máxima.
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/e093fe9f-8b2b-4a94-b720-b69c2c0fe887)

- Estado Grown:
> Activa el collider hasta que el player lo recoge.                                                                     
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/76ae5cc3-c36b-424c-818e-178ce9129e9a)

**Rey Goloso**

Tiene dos estados, en el inicial asigna el pedido, que consiste en 2 productos para cada artista. El otro estado es un merodeo por el castillo. Pienso incoportar un tercer estado para la victoria/derrota.

## Pruebas y métricas

VIDEO

## Referencias
Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington.
- https://docs.unity3d.com/bolt/1.4/manual/index.html
- 
