
![Logo](https://raw.githubusercontent.com/jitt-team/jitt-me/main/assets/fluent.calculations.git.top.banner.med.png)

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=jitt-team_fluent-calculations-primitives)](https://sonarcloud.io/summary/new_code?id=jitt-team_fluent-calculations-primitives)
[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=jitt-team_fluent-calculations-primitives)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=jitt-team_fluent-calculations-primitives&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=jitt-team_fluent-calculations-primitives)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=jitt-team_fluent-calculations-primitives&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=jitt-team_fluent-calculations-primitives)

![Nuget](https://img.shields.io/nuget/v/Fluent.Calculations.Primitives)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Fluent.Calculations.Primitives)
![Nuget](https://img.shields.io/nuget/dt/Fluent.Calculations.Primitives)



[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](https://opensource.org/license/gpl-3-0/)




# Fluent Calculations

Fluent Calculations is a set of libraries for building traceable calculations and logic.

## Project state & disclaimer

Currently this is an experimental project, authors are exploring it's technical viability and the value proposition.

## What's being solved?

Working on line of business applications projects authors have observed a typical 
difficuty in communicating the details of implementations of more complex business logic. 
Validating the correctness of financial calculations like taxes involves a lot of effort on analyst or tester side. 
Pinpointing problems from only final results is mostly impossible, thus requiring manual debugging. 
TDD falls short of communicating more complex test cases to business people and it's. 
Splitting up some algorithms is not always a good option due to loss of original conciseness. 
Troubleshooting calculations happening in a production environment is impossinle without deploying 
some manual logging that makes code noisy and less readable.

## Features

- Number and Boolean datatypes are supported,
- Seamlessly use C# math and logic operators,
- Build isolated calculation components,
- Close to no boilerplate code,
- Generate DOT graph output of your calculations.


## Get Started

Fluent.Calculations can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package Fluent.Calculations.Primitives
```

## Documentation

- [Project Website](https://fcp-project.jitt.me/)
- [Class Library Documentation](https://fcp-api-browser.jitt.me/)


## Usage/Examples

```c#
    internal class MyCalculation : EvaluationContext<Number>
    {
        public Number
            ConstantOne = Number.Of(2),
            ConstantTwo = Number.Of(3);

        public Condition
            MyCondition = Condition.True();

        Number MyResult => Evaluate(() => MyCondition ? ConstantOne : ConstantTwo);

        public override Number Return() => MyResult;
    }
```


## Roadmap & TODOs
- Explore ways to intorduce concept of units.
- Expand a list of supported operations and math functions.
- Explore ways to optimize lambda expression compilation.
- Explore a ways to reuse existing calculations and attempt to benefit from :
-- cached compiled expressions,
-- cached evaluation results,
-- partiall execution depending on changed parameters.
- Explore thread-safety aspects.
 
## License

[GNU General Public License v3.0](https://github.com/jitt-team/fluent-calculations-primitives/blob/2ada80ea405e5ce6198ef1a8973dc23a83bc20c1/LICENSE)


## Authors

- [@edgars-pivovarenoks](https://www.github.com/edgars-pivovarenoks)
- [@artur-karbone](https://www.github.com/arturkarbone)

## 🚀 About The Team
We are bunch of .NET practitioners allways looking for ways to make code great.

