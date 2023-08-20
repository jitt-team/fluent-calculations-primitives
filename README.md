
![Logo](https://github.com/jitt-team/jitt-me/blob/main/assets/fluent.calculations.git.top.banner.med.png?raw=true)


# Fluent Calculations

Fluent Calculations is a set of libraries for building traceable calculations and logic.


## Features

- Number and Boolean datatypes are supported
- Seamlessly use C# math and logic operators
- Build isolated calculation components
- Close to no boilerplate code
- Generate DOT graph output of your calculations



## Usage/Examples

```c#
    internal class MyCalculation : Calculation<Number>
    {
        public Number
            ConstantOne = Number.Of(2),
            ConstantTwo = Number.Of(3);

        public Condition
            MyCondition = Condition.True();

        Number MyResult => Is(() => MyCondition ? ConstantOne : ConstantTwo);

        public override Number Return() => MyResult;
    }
```


## Roadmap & TODOs
- Explore new static member interfaces for operator overloads
  - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-11.0/static-abstracts-in-interfaces#interfaces-as-type-arguments
 
## License

[GNU General Public License v3.0](https://github.com/jitt-team/fluent-calculations-primitives/blob/2ada80ea405e5ce6198ef1a8973dc23a83bc20c1/LICENSE)


## Authors

- [@edgars-pivovarenoks](https://www.github.com/edgars-pivovarenoks)
- [@artur-karbone](https://www.github.com/arturkarbone)

## 🚀 About The Team
We are bunch of .NET practitioners allways looking for ways to make code great.

