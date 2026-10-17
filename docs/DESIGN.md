# Design Notes

## Level Design/Progression
Runs go from the outer planet inward:
Neptune → Uranus → Saturn → Jupiter → Mars → Earth → Venus → Mercury → Sol (possibly add bosses later).
Each run is longer than the last.

## Enemy Spawning
- Enemies spawn just offscreen on a ring around the ship.
- Most come from ahead (top of the screen), while some come from the upper flanks .
- Nothing spawns below the horizontal middle of the screen. This allows the player breathing room.

## Weapons
- Blasters auto-fire only while the plater is touching and the ship is within the aim tolerance margins.

# Flight feel
- The ship's flight speed is constant. progress = elapsed time / run duration.
- The starfield speeds up with progress (this is purely visual) to build tension near the planet.