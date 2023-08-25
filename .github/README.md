
![Logo](https://raw.githubusercontent.com/jitt-team/jitt-me/main/assets/fluent.calculations.git.top.banner.med.png)

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=jitt-team_fluent-calculations-primitives)

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=jitt-team_fluent-calculations-primitives)](https://sonarcloud.io/summary/new_code?id=jitt-team_fluent-calculations-primitives)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=jitt-team_fluent-calculations-primitives&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=jitt-team_fluent-calculations-primitives)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=jitt-team_fluent-calculations-primitives&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=jitt-team_fluent-calculations-primitives)

[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](https://opensource.org/licenses/)




# Fluent Calculations

Fluent Calculations is a set of libraries for building traceable calculations and logic.


## Features

- Number and Boolean datatypes are supported
- Seamlessly use C# math and logic operators
- Build isolated calculation components
- Close to no boilerplate code
- Generate DOT graph output of your calculations



## Get Started

Fluent.Calculations can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package Fluent.Calculations.Primitives
```
    
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

