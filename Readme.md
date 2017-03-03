OGame Bot
=======

A bot for OGame. Contributors welcome.

## Note
This bot is in a **non-working condition**. It is not functional, and cannot "just be used". 

### Disclaimer

I do not work for Gameforge, or intend to do Gameforge harm. I just like making stuff, and this time around it's a bot.

Much of the design in this bot is from a longer period of thinking on how to design a pipeline to consume and act upon data scraped or parsed from the internet. Some of the code will also be generalized in this direction, and not be OGame-specific.

### Index

- [Code](#code)
- [Interventions](#interventions)
- [Parsers](#parsers)
- [Savers](#savers)
- [Commands](#commands)
- [Workers](#workers)

### Code

In the `OGameBot` project, you'll find a console application that runs the thing. Initially it loads from `config.json`, and then proceeds to run the program. A lot of things are undecided, so it'll most likely fail a few times (it's by no means production ready).

The architecture is centered around a pipeline. Every time a web request is completed, it runs through a number of intercepters, parsers, and savers. The idea is to scrape as much information from the webpage in question as possible, unlike many bots which make a single web request to get a single piece of information. (like seeing if enemies are inbound - why not update any ressources you have at the same time?)

#### Interventions
An interceptor can tell the client to retry a request, abort it or to continue as normal. The intended work here is to determine if a request has failed (timeout, server failure, etc.) or if the session has been logged out. 

All interventions inherit from `IInterventionHandler`.

#### Parsers
A parser will scrape the given page for information and return a series of `DataObject`s. All DataObjects go into the pipeline and are made available later on to various consumers of the data. Examples of DataObjects are:

* Galaxy information (occupied planets)
* Moving fleets
* Ressources available
* Built defences, ships, buildings
* The users planets
* Individual messages
* Server settings like speed, name etc.

Parsers do not make lookups into databases, other objects etc. Their only concern is parsing the one page we're looking at. A problem we'll most likely face is this: Which planet are we on? (f.ex. finding the relevant planet for ressources parsed), and we'll just have to figure out how to get that info from that one page.

All parsers inherit from `BaseParser`. All returnde objects inherit from `DataObject`.

#### Savers
A saver will receive all the DataObjects parsed from a page. Common use cases for a saver is to save data to a database, or otherwise make it available. Imagine if we had a tracker of fleets that we want to update - a saver is the correct place to process information from parsers and make them usable.

All savers inherit from `SaverBase`.

#### Commands
A command is a series of web requests necessary to do some job. A command will take all relevant information in its constructor, and then when `Run`, it will lock the client and do its stuff. Examples of commands:

* Change planet
* Build some things
* Send a fleet
* Browse to a galaxy

All commands inherit from `CommandBase`.

#### Workers
Workers are various working classes that will issue commands. I envision workers to always run in the background at scheduled intervals. Exaples of workers could be: 

* Checking planet states
* Scanning a range of galaxies (every 24 hours f.ex.) - downloading from the API?
* Process a queue of build orders
* Process farming and hunter-like tasks (ogameautomizer functions)

All workers inherit from `WorkerBase`.
