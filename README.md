# IceFlake #
* * *

* [About](#about)
* [Requirements](#requirements)
* [Usage](#usage)
* [Credit](#credit)

* * *

## About
This is a small project aimed to provide a simple memory editing framework for the World of Warcraft 3.3.5a client. The framework is written in C# with .NET 4.0. The code is pretty self-explanatory, and is (in my humble opinion) easy to read. I've tried to structure the framework as logical as I could. The framework is injected, so you will need a way of injecting it. We provide a native CLRHost and a managed DomainManager. The reason this project is being put on GitHub is to encourage other users to parttake in the developement. The source consists highly of other peoples contributions.

## Requirements
* WoW 3.3.5a
* [SlimDX](http://slimdx.org/)
* An injector
* Programming knowledge

## Usage
Check out the source, open the solution, add the missing references, compile and you should be good to go.

## Credit
Credit where credit is due.

* [[TOM_RUS]](https://github.com/tomrus88) - for being an excellent coder, reverse engineer and mentor
* kryso - PyMode
* caytchen - [cleanCore](https://github.com/stschake/cleanCore)
* Apoc - GreyMagic, DomainManager & other snippets
* Cypher - the injector