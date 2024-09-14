# ODIN
<p align="center">
  <img src="https://github.com/John31415/Odin/blob/main/Odin.png" alt="Odin">
</p>

## 🙂 ¡Bienvenido!

Este proyecto, orientado por la [Facultad de Matemática y Computación (MATCOM)](https://matcom.uh.cu/) de la [Universidad de La Habana](https://www.uh.cu/inicio/), desarrollado como parte de un esfuerzo por optimizar la creación de cartas para un juego ya existente [🃏Gwentverse: Rick and Morty Edition.](https://github.com/John31415/Gwentverse-Rick-and-Morty-Edition/tree/main?tab=readme-ov-file#gwentverse-rick-and-morty-edition), es un **compilador** diseñado para procesar un DSL (Lenguaje de Dominio Específico) enfocado en la definición de cartas de juego. El compilador facilita la creación, modificación y validación de cartas, brindando una herramienta eficaz para desarrolladores y diseñadores de juegos.

## 📝 Características principales:

- **Lenguaje especializado para cartas**: El DSL está diseñado específicamente para definir las propiedades y habilidades de las cartas del juego de manera clara y sencilla.
  
- **Compilador eficiente**: Transforma el código escrito en DSL en una representación que puede ser usada directamente por el motor del juego, asegurando compatibilidad y validación.

- **Extensible**: Facilita futuras expansiones o modificaciones en la definición de cartas sin necesidad de reescribir todo el código.

## 🎯 Descripción del proyecto

Este compilador está orientado a desarrolladores y diseñadores de juegos que necesitan un método eficiente para definir y gestionar cartas en su juego. A través de un DSL personalizado, permite crear cartas de forma rápida, facilitando su integración con el motor del juego.

## 🃏 Crea cartas y efectos como

```py
effect {
    Name: "Damage",
    Params: {
        Amount: Number
    },
    Action: (targets, context) => {
        for target in targets 
            target.Power -= Amount;
    }
}
effect {
    Name: "Draw",
    Action: (targets, context) => {
        context.Hand.Push(context.Deck.Pop());
        context.Hand.Shuffle();
    }
}
effect {
    Name: "ReturnToDeck",
    Action: (targets, context) => {
        for target in targets {
            owner = target.Owner;
            deck = context.DeckOfPlayer(owner);
            deck.Push(target);
            deck.Shuffle();
            context.FieldOfPlayer(owner).Remove(target);
        };
    }
}
card {
    Type: "Oro",
    Name: "Mr. Poopybutthole",
    Faction: "Morty" @@ "Smith",
    Power: (4 + 2 * 5 / 3) ^ 1,
    Range: ["Melee", "Ranged"],
    OnActivation: [
        {
            Effect: {
                Name: "Damage",
                Amount: 2 ~ (1 + 1)
            },
            Selector: {
                Source: "board",
                Single: false,
                Predicate: (unit) => unit.Faction == "Rick Sanchez"
            },
            PostAction: {
                Effect: "ReturnTo"@"Deck",
                Selector: {
                    Source: "parent",
                    Single: false,
                    Predicate: (unit) => unit.Power < (9 % 4) & 6 
                }
            }
        },
        {
            Effect: "Draw"
        }
    ]
}
```

---
### 🚀 ¡Explora este compilador y simplifica el diseño de cartas en tu juego!

> _"Transformando tokens en cartas épicas, porque escribir `if` y `for` es mucho más glamoroso que barajar."_

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white)

