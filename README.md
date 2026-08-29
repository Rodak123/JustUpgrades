# Upgrade System

My goals:
- provide a solid essentials core
- allow easily adding new features / custom behaviour
- ascension/resets support
- saving/loading support
- WebGL, Linux, Windows

## Disclaimer

Whenever I say 'float', what I actually mean is a non-integer or decimal or fraction. In c# code, it actually uses the BigDouble. 
I just don't want to type out 'whole number' and 'decimal number'. 

## Currency

A currency is a value that can be used for upgrade costs. 
Currencies are stored in a "currency wallet" where they can be read, spent or gained.

Each currency can be locked. Locked status is used in currency generators or UI. 

## Dynamic Value

Each dynamic value [TYPE value, OPTIONS] has:
- Unique Id [string]
- Type [bool/float/int]
- Base amount
- Min amount
- Max amount

The values are stored/managed by the central "value manager". This manager allows modifiers to subscribe to the mediator of each value. This is so that no value is fixed, rather they are queried.

So the current amount of a value is:
Amount = Base + Modifiers

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

## Scaled Value

A scaled value is a value type that defines how a number scales with each level.
This value is only used when a value is clearly linked to an upgrade.

Scalers:
- Constant - Level has no affect on this value, can be set to 0 at level 0
- Linear - Linearly scales with the level
- Exponential - Exponentially scales with the level
- Graph - Uses a graph to determine current value
- List - Level is the index of an value array

Each scaler can expose 

## Upgrade Properties

### Core properties

- Id - unique upgrade id
- Name - title of the upgrade
- Custom - some custom properties, for example:
  - Upgrade Visual - used for UI, like description

### Exposable Properties

- Level [level]
- Costs [cost array] - they are added together
- Requirements [requirement array] - they all must be fulfilled
- Modifiers [modifier array]

Requirements and modifiers have their own upgrade properties, these get added to the base exposable properties. All properties under an upgrade can be "exposed". 
By exposing a property, it can be modified.

## Upgrade Level

Defines how the upgrade is allowed to well, upgrade.

- Level Cap [int value, 1+] - max level
- Infinite [bool value]

## Upgrade Cost

The cost is for a single currency.

- Cost Value [float value, 0+]
- Currency [currency reference]
- Is Spent [bool value] - when false, buying doesn't reduce the currency (this is for requirement upgrade late game)

## Upgrade Requirement [requirement]

All requirements must be met to unlock the upgrade.

### Upgrade Dependency

If referenced upgrade has the needed level.

- Upgrade [upgrade reference]
- Target Level [int value, 0+] - target level to achieve
- Max Level [bool value] - if target level is upgrades max level

If max level is false, target level is used, otherwise target level is ignored.

### Currency Dependency [not implemented]

If referenced currency is unlocked.

- Currency [currency reference]

## Upgrade Modifiers [modifier]

Each upgrade has some modifiers. These make the upgrade "do" stuff. 
Usually it's modifying a value (hence the name modifiers).

### Dynamic Value Modifier

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

### Currency Unlock [not implemented]

Can be also done with dynamic value modifier. 

Unlocks a currency after a level.

- Target Level [int value, 1+] 
- Currency [currency reference]

### Currency Add [not implemented]

Can be also done with dynamic value modifier. 

One time currency gain per level.

- Currency [currency reference]
- Gain [float value]
