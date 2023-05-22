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

## Una partida típica
El jugador tiene un pedido de 4 productos. El panadero y el florista tienen un indicador del ingrediente que necesitan. El jugador irá regogiendo uno por uno los ingredientes e irá entregándolos. Acto seguido, cada uno  de los artistas dejará el producto en su lugar de recogida donde el player lo recogerá y se lo tendrá que llevar al rey. Cuando el rey tenga todo, se gana la partida.

## Punto de partida
Para el mapa he usado algunos assets de la [práctica 3](https://github.com/IAV23-G15/IAV23-G15-P3), asi como otros recursos gratuitos del Asset Store de Unity, y algunos modelos gratuitos de Sketchfab.

## Diseño de la solución

He implementado una serie de scripts para el proyecto, pero los más importantes de cara a la inteligencia artificial son los siguientes:
- PanaderoController y FloristaController: Se encargan del merodeo, las animaciones y la gestión de los ingredientes.
- KingManager: Tiene el código del merodeo y la gestión de los productos, que se reflejen en la interfaz de usuario.
Los tres son utilizados en las máquinas de estados.

Para el movimiento de todos los personajes he añadido el sistema de navegación de Unity. Hay 4 zonas delimitadas como áreas: el castillo, la panadería, la floristería y el resto de espacio para el player.

![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/074b295e-6074-4de6-a49b-034f01b82499)
>1. Panadería, 2. Castillo, 3. Floristería, 4. Walkable

A continuación, explicaré las máquinas de estados que he implementado:

**Panadero y Florista**

Ambos tiene una máquina muy similar, que difiere en algunas posiciones a las que van.

- Las máquina de estados son la siguiente:

> Cada NPC tiene un estado en el que espera a recibir todos los ingredientes de la receta, y posteriormente va a las diferentes posiciones (nevera, mesa, etc.) para "preparar" el producto y finalmente el ultimo estado en el que se entrega el producto al jugador.
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/80da3b47-1316-4fc8-ae23-68351be431b1)
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/0e40f0e2-72f4-4ff7-86e1-73eec75c53d1)

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

- La máquina de estados es la siguiente:

> Tiene tres estados, al inicio asigna el pedido, que consiste en 2 productos para cada artista, y los estados de ganar o perder. 
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/96046e77-af0c-4c2a-8dce-480ab2ba4ab9)

- Estado Start:

> En este estado el rey acepta los productos, estos se indican en la barra de arriba y comprueba si el pedido esta completo. Además el rey merodea. Si se consiguen todos los productos se gana la partida. Si se acaba el tiempo se pierde.                                                             
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/e94c88b4-0e87-4da5-9c6a-6bae9f90f7e3)

- Estados Win y Lose:

> En estos estados se ejecutan los sonidos correspondientes y se vuelve al menú principal.
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/824b045f-0a3c-4200-81e4-9a5bb436384f)
![imagen](https://github.com/ssimoanto/IAV23-AntonovaMihaylova/assets/72394611/234b87bb-d2d4-44c7-bd8f-4086f978cb80)

## Pruebas y métricas

[VIDEO](https://youtu.be/nG3ys4XHsJ0)

## Referencias
Los recursos de terceros utilizados son de uso público.

- [Player y decoración](https://kaylousberg.itch.io/kaykit-dungeon)
- https://docs.unity3d.com/bolt/1.4/manual/index.html
- Sonidos de [Zapsplat](https://www.zapsplat.com/)
- Assets de Unity Asset Store: [flores](https://assetstore.unity.com/packages/3d/vegetation/plants/lowpoly-flowers-47083), [gallinas](https://assetstore.unity.com/packages/3d/characters/animals/meshtint-free-chicken-mega-toon-series-151842), [panadero y florista](https://assetstore.unity.com/packages/3d/characters/viass-free-character-pack-141471), [electrodomésticos](https://assetstore.unity.com/packages/3d/props/electronics/kitchen-appliance-low-poly-180419), [mesa](https://assetstore.unity.com/packages/3d/environments/training-table-136070) y [Efectos especiales](https://assetstore.unity.com/packages/vfx/particles/3d-games-effects-pack-free-42285).
- Assets de Sketchfab: [pan](https://skfb.ly/6Suon), [tarta](https://skfb.ly/oG7TL), [tulipanes](https://skfb.ly/o6psE), [jarrón con flores](https://skfb.ly/6WnFC) y [ramo de flores](https://skfb.ly/oBUBA).
