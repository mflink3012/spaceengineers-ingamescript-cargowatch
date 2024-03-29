![Thumbnail](thumb.png)

# Short description
An ingame-script for Space Engineers watching the grids cargo (loaded mass and volume) with watermarks and activates another block on reach.

With this script one is able to get a signal that loaded mass and/or volume is getting critical on the current grid (docked grids and non-functional inventory-blocks will be ignored).

I needed this, because my big drilling-machine got overloaded and my grinding-machine got full. Now I have a LCD or a blinking light activated to warn me.

This script will output some details on current loaded mass and volume to the display of the programmable block.

It will give extra details (like defined blocks) in the debugging-section of the programmable block.

# Example

Add to `Custom Data` on a programmable block attached to a vehicle:

    Mass.Watermark.Value=1400000
    Mass.Watermark.Activate=Chisel.LCD

That will activate the LCD named `Chisel.LCD` (preconfigured with what you like - an exlamation mark for example) when the total mass of the vehicle the programmable block is attached to will increase to more than 1.4 tons (because you might overload it with the drill).

# Custom Data (Properties)
If you change the Custom Data of the programmable block the script is running on, you need to recompile the script to get the changes active.

## Watermark for mass
To watch the mass-load of the grid, give `Mass.Watermark.Value` and `Mass.Watermark.Activate`.

### Mass.Watermark.Value (long)

This `long`-property will define the value for critical mass in kg (kilograms). Once this value is reached (or exceeded) **and** `Mass.Watermark.Activate` is defined, the defined block will be enabled. If the current mass is below the watermark, the defined block will be disabled.

### Mass.Watermark.Activate (string)

This `string`-property will hold the name of the functional block to enable or disable (depending on `Mass.Watermark.Value`).

## Watermark for volume
To watch the volume-load of the grid, give `Volume.Watermark.Value` and `Volume.Watermark.Activate`.

### Volume.Watermark.Value (long)

This `long`-property will define the value for critical volume in L (liters). Once this value is reached (or exceeded) **and** `Volume.Watermark.Activate` is defined, the defined block will be enabled. If the current volume is below the watermark, the defined block will be disabled.

### Volume.Watermark.Activate (string)

This `string`-property will hold the name of the functional block to enable or disable (depending on `Volume.Watermark.Value`).

# Steam Workshop
[https://steamcommunity.com/sharedfiles/filedetails/?id=2447352453](https://steamcommunity.com/sharedfiles/filedetails/?id=2447352453)
