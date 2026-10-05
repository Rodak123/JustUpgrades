# Upgrade System

**THIS IS AN IN-DEVELOPMENT LIBRARY**

<img width="531" height="1535" alt="image" src="https://github.com/user-attachments/assets/78ac3c0f-2ee9-4702-8299-d6aa2cf52d2d" />

My goals:
- provide a solid essentials core
- allow easily adding new features / custom behaviour
- have events and hooks for new scripts 
- saving/loading support
- ascension/resets support
- work for WebGL, Linux, Windows (and Android?)

## Contents

- [Upgrade System](#upgrade-system)
  - [Contents](#contents)
  - [Dependencies](#dependencies)
  - [Disclaimer](#disclaimer)
  - [Currency](#currency)
  - [Dynamic Values](#dynamic-values)
  - [Scaled Values](#scaled-values)
  - [Upgrades](#upgrades)
    - [Upgrade Container](#upgrade-container)
    - [Upgrade Properties](#upgrade-properties)
      - [Core properties](#core-properties)
      - [Exposable Properties](#exposable-properties)
    - [Upgrade Level](#upgrade-level)
    - [Upgrade Cost](#upgrade-cost)
    - [Upgrade Requirements](#upgrade-requirements)
      - [Upgrade Dependency](#upgrade-dependency)
      - [Currency Dependency \[not implemented by default\]](#currency-dependency-not-implemented-by-default)
    - [Upgrade Modifiers](#upgrade-modifiers)
      - [Dynamic Value Modifier](#dynamic-value-modifier)
      - [Currency Unlock \[not implemented by default\]](#currency-unlock-not-implemented-by-default)
      - [Currency Add \[not implemented by default\]](#currency-add-not-implemented-by-default)
  - [Core Runtime Systems](#core-runtime-systems)
    - [Currency Wallet](#currency-wallet)
    - [Dynamic Value Registry](#dynamic-value-registry)
    - [Upgrade Tracker](#upgrade-tracker)
  - [Currency Generators and UI](#currency-generators-and-ui)
  - [Ascensions / Resets / Prestige](#ascensions--resets--prestige)

## Dependencies

- [Newtonsoft Json Unity Package](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/manual/index.html) - Install by name: `com.unity.nuget.newtonsoft-json`
- Text Mesh Pro (only for demos, can use your own variant)


## Disclaimer

I tired to make this library as versatile as possible, that doesn't mean it covers all needs. So feel free to change its code for you game.

Whenever I say 'float', what I actually mean is a non-integer or decimal or fraction. In c# code, it actually uses the BigDouble. 
I just don't want to type out 'whole number' and 'decimal number'.  

## Currency

A currency is a value that can be used for upgrade costs. 
Currencies are stored in a "[currency wallet](#currency-wallet)" where they can be read, spent or gained.

Each currency can be locked. Locked status is used in currency generators or UI. 

## Dynamic Values

Each dynamic value [TYPE value, OPTIONS] has:
- Unique Id [string]
- Type [bool/float/int]
- Base amount
- Min amount
- Max amount

The values are stored/managed by the central "[dynamic value registry](#dynamic-value-registry)". This registry allows modifiers to subscribe to the mediator of each value. This is so that no value is fixed, rather they are queried.

So the current amount of a value is:
$$
Amount = Base + Modifiers
$$
The base is also implemented as a modifier

Modifiers can only affect exposed values.
These values are defined in:
- upgrades (their properties)
- generators (gain per second)
- other game elements like:
  - player movement speed (some game where that matters)
  - gun damage (maybe this game is built around shooting)
  - etc.

Dynamic value is implemented to only store a float number, other types are extracted:
- float = value
- boolean = value > 0
- int = floor(value)

## Scaled Values

A scaled value is a value type that defines how a number scales with each level.
This value is only used when a value is clearly linked to an upgrade.

Scalers:
- Constant - Level has no affect on this value, can be set to 0 at level 0
- Linear - Linearly scales with the level
- Exponential - Exponentially scales with the level
- Graph - Uses a graph to determine current value
- List - Level is the index of an value array

Each scaler can expose 

## Upgrades

### Upgrade Container

An upgrade container holds upgrades. Used to organize.

- Upgrade - Is a upgrade container of itself
- Upgrade Group - Holds N upgrades

### Upgrade Properties

#### Core properties

- Id - unique upgrade id
- Name - title of the upgrade
- Custom - some custom properties, for example:
  - Upgrade Visual - used for UI, like description

#### Exposable Properties

- Level [level]
- Costs [cost array] - they are added together
- Requirements [requirement array] - they all must be fulfilled
- Modifiers [modifier array]

Requirements and modifiers have their own upgrade properties, these get added to the base exposable properties. All properties under an upgrade can be "exposed". 
By exposing a property, it can be modified.

### Upgrade Level

Defines how the upgrade is allowed to well, upgrade.

- Level Cap [int value, 1+] - max level
- Infinite [bool value]

### Upgrade Cost

The cost is for a single currency.

- Cost Value [float value, 0+]
- Currency [currency reference]
- Is Spent [bool value] - when false, buying doesn't reduce the currency (this is for requirement upgrade late game)

### Upgrade Requirements

All requirements must be met to unlock the upgrade.

#### Upgrade Dependency

If referenced upgrade has the needed level.

- Upgrade [upgrade reference]
- Target Level [int value, 0+] - target level to achieve
- Max Level [bool value] - if target level is upgrades max level

If max level is false, target level is used, otherwise target level is ignored.

#### Currency Dependency [not implemented by default]

If referenced currency is unlocked.

- Currency [currency reference]

### Upgrade Modifiers

Each upgrade has some modifiers. These make the upgrade "do" stuff. 
Usually it's modifying a value (hence the name modifiers).

#### Dynamic Value Modifier

Modifies some dynamic value.

- Target Value [value reference]
- Value Change - an array of scaled values

This modifier will interact with the value manager to change the values.

For example:
- Additive gains - +N/s
- Multiplicative gain - +Nx 
- Exponential - +^N
- Improve Upgrade - increase other upgrades properties (+2/s -> +3/s or max level 5 -> 10)

This can modify any dynamic value, even itself. 

#### Currency Unlock [not implemented by default]

Can be also done with dynamic value modifier. 

Unlocks a currency after a level.

- Target Level [int value, 1+] 
- Currency [currency reference]

#### Currency Add [not implemented by default]

Can be also done with dynamic value modifier. 

One time currency gain per level.

- Currency [currency reference]
- Gain [float value] 

## Core Runtime Systems

These systems use the data above (scriptable objects) 
and then act as a single source of truth for the upgrade system game state.

### Currency Wallet

Just like the analogy, it stores the exact amount of each currency.

Provides functions to gain, check and spend currencies.

### Dynamic Value Registry

This registry stores all dynamic values and their modifiers.

It does not store state, each dynamic value is made up from the modifiers. Provides functions to query dynamic values.

### Upgrade Tracker

The upgrade tracker holds all registered upgrades. 

It stores each upgrades state and provides functions to access them.

## Currency Generators and UI

A currency generator is a term I made up, it is a script which handles how a currency is gained. This can be per second, once per click or anything. 
The main point is that the generator uses dynamic values so that the upgrades can interact with it (upgrade it) and UI can ask it about how much are we generating over time. 

Since each incremental is very different, it's basically impossible to make a generic solution for generating currencies and UI. (also, I don't think that making something for ALL games is a good idea anyway)

Because of this, this library lets you make your own generators and UI, which will use the core systems and data above. 
I will implement few demos which have various generator and UI implementations.

## Ascensions / Resets / Prestige

Most incrementals allow to reset some part of the game to gain some new currency or unlock a new area.

This is also very differently implemented across incrementals. I want to provide a simple system where you can specify "tiers" of ascension.

