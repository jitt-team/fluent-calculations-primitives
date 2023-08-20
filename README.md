
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


## Authors

- [@edgars-pivovarenoks](https://www.github.com/edgars-pivovarenoks)
- [@artur-karbone](https://www.github.com/arturkarbone)

## 🚀 About The Team
We are bunch of .NET practitioners allways looking for ways to make code great.

