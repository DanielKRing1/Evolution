# Evolution
Unity simulation of Evolution:
Find enough energy to reproduce. Offspring possess mutations in their qualities of size, speed, vision, attack.
Only the fittest can survive.

As the overseer of this simulation, you can edit the:
- Size of your world
- Starting number of animals
- Max amount of available food
- Number of turns before replenishing food
- Max and min starting values for each animal quality
- Speed of the simulation

Additionally, you can toggle the "auto" option to either let the simulation run on its own or to require that you press Space to simulate a single step.

To add more strain to the evolution of the animals, I've also included an "oscillate" option. If active, the max number of food fluctuate between increasing and descreasing by "foodChangeMagnitude" units of food each step, for "cycleLength" # of steps. After "cycleLength" steps have occured, the "oscillationDirection" will change (1 or -1).
