# Beehive Utilities Revived
A few customisation options for beehives including amount of honey a hive can hold, time it takes to generate the honey and disabling the proximity build restrictions even as the Biome bees are happy in.

This is just a revive/rewrite of Smallo's BeehiveUtilits to make this mod work with current Valheim Version again; the deprecated Version blocked connecting to a servicer with a black screen.

## Manual Installation
To install this mod, you need to have BepInEx. After installing BepInEx, extract BeehiveUtilities.dll into games install **"\Valheim\BepInEx\plugins"**

## Config
Before the config file is generated, you must run the game with the mod installed. The config file is located at **"\Valheim\BepInEx\config\mennowar.mods.beehiveutilities_revived.cfg"**

#### There are serveral config options available;

| Config Option | Type | Default Value | Description |
|:-------------:|:-----------:|:-----------:|:-----------|
| Enable Mod | bool | true | Enable or disable the mod |
| Disable Proximity Check | bool | true | Disables the "Bees need more space" check |
| Max Honey | int | 4 | The maximum amount of honey a beehive can generate (default is 4) |
| Minutes Per Creation | double | 20.0 | The amount of minutes it takes to generate 1 piece of honey (default is 20) |
| Remove Biome Check | bool | false | Allows beehives to work in any biome |
| Spawn Honey In Front | bool | false | Spawns the honey in front of the hive instead of on top of it. Here is a picture to show which way is the front of the hive, it's the side where the thatch overlaps from the sides on top. |

## Thanks
Thanks go to smallo for the original mod ["BeehiveUtilies"](https://valheim.thunderstore.io/package/Smallo/BeehiveUtilities/)
