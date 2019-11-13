# Arkanoid Demo


<h2>Some comments about demo</h2> <br>

<h4>Developing on Unity 2019.2.1f1</h4>
<h4>Major platform: Win(but it can work on mobile)</h4>

* Game stats saving only runtime. (GameData.cs)
* 3 types of enemies. Using simplest pool pattern (EnemyPool.cs и EnemyManager.cs).
* 3 types of perks  - health, shot, speedup.
* Some various of win conditions(timeout not implmented yet). See:  Resources/GameData.asset
* Simplest UI menu: pause, lose, win, wait, main menu screens. 
* Level Generator can generate some levels with different enemies set. (lite example). 
* Input control for win platform, but it can work on mobile, didn't tested yet. (InputControl.cs)
* Perks generator.(PickUpManager.cs) (without pool)
* Enemy's meshes definitely require retop, they are only for example.
* Game states pattern used. (GameState.cs)
* Weapon, Projectile are dummy classes.
* Enemy's class is knowingly really simple (without AI, Controller, etc patterns). Ball, Level classes too.
* There are no sounds, animations, particles, etc.
