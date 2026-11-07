---
name: programacion-contenida
description: Trabaja de forma contenida en este proyecto de programación. Úsala para cualquier tarea de programación, implementación, modificación, corrección o creación de código dentro de este proyecto. Realiza únicamente lo que el usuario pide, evita cambios adicionales, usa código sencillo apropiado para una estudiante y solicita autorización antes de ampliar el alcance de una tarea.
---

# Programación contenida del proyecto

## Objetivo principal

Trabaja únicamente sobre lo que el usuario solicite de forma explícita.

Este es un proyecto estudiantil. El código debe ser sencillo, directo, fácil de leer y fácil de explicar.

No conviertas tareas pequeñas en sistemas grandes.

La prioridad es:

1. Cumplir exactamente la petición.
2. Modificar lo mínimo necesario.
3. Mantener el código sencillo.
4. No alterar otras partes del proyecto.
5. No agregar funcionalidades sin permiso.

---

# Regla principal: no ampliar el alcance

Cuando el usuario solicite una tarea específica:

- Haz únicamente esa tarea.
- No agregues funciones relacionadas que no hayan sido solicitadas.
- No agregues mejoras opcionales automáticamente.
- No continúes desarrollando otros sistemas.
- No aproveches la tarea para reorganizar el proyecto.
- No agregues contenido "por si después sirve".
- No hagas cambios preventivos que el usuario no haya pedido.

Una petición pequeña debe producir un cambio pequeño.

Ejemplo:

Si el usuario pide:

> Haz que una puerta se abra cuando el jugador interactúe.

Implementa únicamente lo necesario para que la puerta se abra.

NO agregues automáticamente:

- sistema de llaves;
- inventario;
- sonidos;
- partículas;
- animaciones adicionales;
- objetivos;
- UI;
- checkpoints;
- guardado;
- diferentes tipos de puertas;
- achievements;
- sistemas de eventos generales.

Estos elementos solamente se agregan si el usuario los solicita explícitamente.

---

# Cambios mínimos

Antes de modificar código o archivos, determina cuál es el cambio mínimo necesario para resolver la petición.

Prefiere:

- modificar menos archivos;
- escribir menos código;
- reutilizar sistemas existentes;
- evitar dependencias nuevas;
- evitar cambios globales;
- mantener intactas las partes no relacionadas.

No interpretes una mejora posible como una tarea necesaria.

---

# Código apropiado para estudiante

El usuario está aprendiendo programación.

El código debe priorizar claridad antes que sofisticación.

Prefiere:

- `if` sencillos;
- variables claras;
- métodos fáciles de seguir;
- referencias directas;
- componentes sencillos;
- lógica explícita;
- nombres descriptivos;
- estructuras pequeñas.

Evita soluciones excesivamente profesionales o abstractas cuando una solución sencilla sea suficiente.

No utilices automáticamente:

- dependency injection;
- service locators;
- reflection;
- arquitecturas complejas;
- sistemas excesivamente genéricos;
- patrones avanzados innecesarios;
- event buses;
- factories;
- sistemas de interfaces grandes;
- abstracciones de varias capas;
- frameworks adicionales.

No escribas código complejo únicamente porque sea considerado una práctica más avanzada.

Si existen dos soluciones correctas, prefiere la más fácil de entender.

---

# No crear scripts innecesarios

No crees un script nuevo cuando la tarea pueda resolverse de manera sencilla utilizando un script existente.

Evita crear automáticamente clases como:

- Manager;
- Controller;
- Handler;
- Service;
- System;
- Helper;
- Utility;

si realmente no son necesarias.

Por ejemplo, no crear:

- DoorManager;
- InteractionManager;
- AudioManager;
- UIManager;
- GameManager;

solamente para implementar una función sencilla.

Crear un script nuevo es válido cuando:

- la funcionalidad realmente necesita su propio componente;
- mantenerla en otro script haría el código confuso;
- el usuario pidió específicamente un script separado.

---

# No modificar elementos no relacionados

No modifiques escenas, prefabs, GameObjects, scripts, materiales, componentes o configuraciones que no tengan relación directa con la petición.

Si una tarea está relacionada con un objeto concreto, intenta mantener los cambios dentro de ese objeto y sus dependencias directas.

No utilices una tarea como oportunidad para corregir o cambiar otras partes del proyecto.

---

# No hacer optimizaciones prematuras

No optimices código automáticamente si el usuario no solicitó optimización.

No agregues solamente por "buena práctica":

- object pooling;
- sistemas de caché complejos;
- ScriptableObjects;
- sistemas globales de eventos;
- singletons;
- estructuras genéricas;
- optimizaciones de memoria;
- optimizaciones prematuras de rendimiento.

Si el sistema actual funciona para el alcance del proyecto, mantenlo sencillo.

---

# No reorganizar variables sin necesidad

No cambies automáticamente la forma en que están declaradas las variables solamente porque consideres que existe una forma más profesional.

No conviertas variables automáticamente entre:

- `public`;
- `private`;
- `[SerializeField] private`;
- propiedades;
- getters;
- setters;

salvo que sea necesario para la tarea.

No reorganices código existente solamente por estilo.

---

# Comentarios sencillos

Puedes agregar comentarios cuando realmente ayuden a comprender una parte del código.

Los comentarios deben ser:

- cortos;
- claros;
- útiles para una estudiante.

No agregues comentarios a cada línea.

No conviertas un script pequeño en un documento lleno de explicaciones.

Ejemplo apropiado:

```csharp
// Abre la puerta cuando el jugador interactúa
```

Evita comentarios innecesarios como:

```csharp
// Declaramos una variable de tipo float llamada speed que almacenará
// el valor numérico correspondiente a la velocidad de movimiento...
```

---

# Validaciones sencillas

No agregues grandes cantidades de comprobaciones, fallbacks o sistemas defensivos si no son necesarios.

Es válido utilizar una comprobación sencilla como:

```csharp
if (target != null)
```

cuando tenga sentido.

Evita llenar una función sencilla con:

- múltiples validaciones;
- warnings;
- try/catch innecesarios;
- sistemas de recuperación;
- valores fallback;
- múltiples condiciones defensivas;

solamente para cubrir situaciones hipotéticas.

Mantén las comprobaciones proporcionales al proyecto.

---

# No modificar UI sin permiso

No agregues ni modifiques automáticamente:

- textos;
- botones;
- barras;
- iconos;
- mensajes;
- popups;
- pantallas;
- HUD;
- indicadores;
- menús.

Una mecánica no implica automáticamente que necesite UI.

Si consideras que una interfaz sería útil, puedes sugerirla, pero no implementarla.

---

# No agregar efectos audiovisuales

No agregues automáticamente:

- sonidos;
- música;
- partículas;
- animaciones;
- camera shake;
- postprocesado;
- flashes;
- transiciones;
- efectos visuales;
- efectos de pantalla.

Aunque puedan mejorar una mecánica, requieren autorización del usuario.

---

# No crear documentación adicional

No crees automáticamente:

- README;
- archivos de documentación;
- changelogs;
- diagramas;
- documentos técnicos;
- tutoriales externos;
- archivos de notas;
- archivos de configuración adicionales.

Solamente créalos cuando el usuario los solicite.

---

# No completar sistemas incompletos

Si encuentras una mecánica, script o sistema que parece estar incompleto, no asumas que debes terminarlo.

Trabaja solamente sobre la parte solicitada.

Ejemplo:

Si encuentras un inventario incompleto mientras estás trabajando en una puerta, no continúes desarrollando el inventario.

---

# No hacer cambios preventivos

No modifiques cosas con argumentos como:

- "para evitar problemas futuros";
- "por si después necesitas...";
- "para que sea escalable";
- "para prepararlo para futuras funciones";
- "para hacerlo más profesional";
- "para que después sea más fácil agregar...".

El proyecto debe resolver las necesidades actuales indicadas por el usuario.

No diseñes sistemas para necesidades hipotéticas.

---

# No usar complejidad profesional innecesaria

Este proyecto debe poder ser comprendido por una estudiante.

Evita transformar código sencillo en código empresarial o de producción profesional.

No introduzcas patrones avanzados únicamente para demostrar buenas prácticas.

Prioriza que el usuario pueda:

- leer el código;
- entender qué hace;
- modificarlo después;
- explicarlo frente a un profesor.

Una solución corta y clara es preferible a una arquitectura sofisticada cuando ambas resuelven correctamente la tarea.

---

# No modificar demasiados archivos sin autorización

Antes de realizar una tarea, estima cuántos archivos será necesario modificar.

Si puedes resolverla modificando uno o dos archivos, hazlo de esa manera.

Si una tarea aparentemente sencilla requiere modificar muchos archivos, detente antes de aplicar esos cambios.

Considera como cambio amplio:

- modificar 5 o más archivos existentes;
- cambiar múltiples sistemas;
- reorganizar una parte importante del proyecto;
- modificar varias escenas;
- modificar varios prefabs;
- cambiar arquitectura existente.

En esos casos:

1. Explica brevemente por qué se necesitan tantos cambios.
2. Indica qué archivos o sistemas tendrían que modificarse.
3. Solicita autorización antes de realizar el cambio amplio.

No realices una modificación grande silenciosamente.

---

# No refactorizar sin permiso

No refactorices código únicamente porque consideres que puede escribirse mejor.

No:

- renombres scripts;
- renombres clases;
- renombres métodos;
- renombres variables existentes;
- reorganices carpetas;
- cambies namespaces;
- dividas scripts;
- combines scripts;
- reemplaces sistemas existentes;
- cambies arquitectura.

Hazlo solamente cuando:

- sea estrictamente necesario para cumplir la tarea;
- o el usuario lo solicite.

---

# No instalar dependencias sin permiso

No instales automáticamente:

- paquetes;
- plugins;
- assets;
- librerías;
- frameworks;
- herramientas externas;
- dependencias adicionales.

Si una dependencia parece necesaria:

1. Explica cuál sería necesaria.
2. Explica brevemente para qué sirve.
3. Espera autorización antes de instalarla.

Siempre intenta primero resolver la tarea con lo que ya existe en el proyecto.

---

# No modificar configuraciones globales sin permiso

No cambies automáticamente:

- Project Settings;
- Input Settings;
- Input System;
- Physics Settings;
- Quality Settings;
- Tags;
- Layers;
- Render Pipeline;
- Player Settings;
- Build Settings;
- Package Manager;
- configuraciones globales del proyecto.

Si una tarea realmente requiere uno de estos cambios, indícalo antes de realizarlo.

---

# Respetar sistemas existentes

Antes de crear un sistema nuevo, revisa si ya existe una solución dentro del proyecto que pueda reutilizarse.

No dupliques sistemas.

No crees versiones paralelas de algo que ya existe.

Ejemplo:

Si ya existe un sistema de interacción, utiliza ese sistema antes de crear un segundo sistema de interacción.

Mantén las modificaciones dentro del estilo actual del proyecto cuando sea razonablemente sencillo hacerlo.

---

# Sugerencias permitidas

Puedes detectar mejoras potenciales.

Sin embargo, una sugerencia NO equivale a permiso para implementarla.

Puedes mencionarla brevemente después de completar la tarea.

Ejemplo:

> Sugerencia opcional: esta interacción también podría tener un sonido. No lo agregué porque no lo pediste.

Nunca implementes una sugerencia automáticamente.

---

# Cuando una petición sea ambigua

No amplíes la petición intentando adivinar características adicionales.

Elige la interpretación más pequeña y sencilla que cumpla lo solicitado.

Si existen varias implementaciones posibles, elige la que:

1. modifique menos cosas;
2. utilice menos código;
3. sea más sencilla;
4. sea más fácil de explicar;
5. dependa menos de otros sistemas.

---

# Al trabajar con código existente

Antes de modificar un archivo:

1. Lee el código relacionado.
2. Identifica qué parte controla la función solicitada.
3. Modifica solamente esa parte cuando sea posible.
4. Conserva el resto del comportamiento existente.
5. No limpies, reformatees o reestructures código no relacionado.

No reemplaces un script completo si basta con cambiar unas pocas líneas.

---

# Al crear código nuevo

Cuando realmente sea necesario crear código nuevo:

- mantenlo pequeño;
- utiliza nombres descriptivos;
- evita arquitecturas innecesarias;
- expón solamente las variables necesarias;
- evita dependencias que no sean necesarias;
- no agregues funciones que el usuario todavía no necesita.

No diseñes el script pensando en diez futuras funcionalidades.

Diseña el script para la tarea actual.

---

# Formato de respuesta después de trabajar

Después de completar una tarea, responde de forma breve.

Indica:

### Cambios realizados
Qué modificaste.

### Archivos modificados
Qué archivos fueron creados o cambiados.

### Qué debes hacer tú
Solo si el usuario necesita realizar algún paso manual dentro del editor o proyecto.

### Sugerencias opcionales
Solamente si encontraste algo realmente relevante.

No escribas una explicación técnica extensa salvo que el usuario la solicite.

---

# Verificación final obligatoria

Antes de terminar cualquier tarea, comprueba:

- ¿Hice exactamente lo que pidió el usuario?
- ¿Agregué alguna funcionalidad que no solicitó?
- ¿Modifiqué archivos que no eran necesarios?
- ¿Creé scripts innecesarios?
- ¿Hice una optimización no solicitada?
- ¿Modifiqué UI sin permiso?
- ¿Agregué sonido, partículas o efectos sin permiso?
- ¿Reorganicé variables o código sin necesidad?
- ¿Hice cambios preventivos?
- ¿Utilicé una solución demasiado avanzada?
- ¿Existe una solución más sencilla?
- ¿El usuario podría entender y explicar este código?
- ¿Estoy modificando demasiados archivos para una tarea pequeña?

Si alguna respuesta indica que el alcance fue mayor de lo solicitado, reduce los cambios antes de finalizar.

---

# Regla final

NO ASUMAS QUE "SERÍA MEJOR" SIGNIFICA "ESTÁ AUTORIZADO".

Si no fue solicitado y no es estrictamente necesario para que funcione la tarea pedida, no lo implementes.

Primero cumple exactamente la petición.

Después, si es útil, puedes sugerir mejoras opcionales para que el usuario decida.