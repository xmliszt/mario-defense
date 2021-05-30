# mario-defense
50.033 Game Design Lab 2 Handout

---

<center>
by Li Yuxuan 1003607
<h1>
<a href="https://xmliszt.itch.io/mario-defense">Play It Here!</a>
</h1>
<h3>
<a href="https://github.com/xmliszt/mario-defense">GitHub Repo</a>
</h3>
<h3>
<a href="https://youtu.be/eU3NOmXS1kA">Gameplay Video</a>
</h3>
</center>

---

## Lab Competition

I would like to take part in the competition if it is possible :D

## Game Design

- Title: **Mario Defense**
- Single Player
- 4 Levels shooting game inspired by Angry Birds
- Concept: Player use mouse to drag Mario from the slingshot to shoot out Mario towards approaching enemies. When an enemy enters the castle it is gameover. The objective is to stop the enemies from entering the castle by destroying the enemies completely. Enemies have different health and speed. Some and stronger, some are faster.
- Style: Add post-processing to make the game look more retro style. With vignette effect and lens distortion, plus a bit of motion blur and noise to the screen.
- Special game objects

| Game Object | Description |
| -------- | -------- |
| ![](https://i.imgur.com/e1f1epX.png)     | Mario will climb up the slingshot every 2 seconds. Player can drag the Mario to shoot it out! Mario with a certain velocity can give 100 health damage to the enemy unit.|
| ![](https://i.imgur.com/Hpox9kg.png)     | Gomba, the easiest enemy. Have health of 100. Move speed of 0.5|
| ![](https://i.imgur.com/m6bVQL3.png)     | Spirry, the fastest enemy. Have health of 200. Move speed of 0.8    |
| ![](https://i.imgur.com/xBuuWGG.png)     | Troopa, the second fastest enemy, though it is a tortoise. Have health of 300. Move speed of 0.7|
| ![](https://i.imgur.com/JkFshLp.png)     | King Koopa, the boss at each level, sometimes come as solo, sometimes in pairs. Have health of 1000, extremely tough, but move speed is only 0.3 |
| ![](https://i.imgur.com/y5O07sr.png)     | The castle, Mario's defense base. If enemy enter the castle, it is considered game over. |

## Features

### Slingshot Mechanism

How does Mario get shot by the slingshot? I have experimented two ways and in the end chose to calculate the shooting direction and add a velocity to Mario's rigidbody2D that is proportional to the length of stretched slingshot strips to Mario to let it fly. The first way is of course to use the `Spring Joint 2D` component. However, I encountered a problem of referencing when I need to implement the strip stretching by using `Line Renderer 2D` at the slingshot as well. The script controls the stretching of the strips as well as detecting mouse down and up when user drag and release. Unity requires a collider to be in place for mouse click event to be detected. Since the `Spring Joint 2D` component is in Mario not in the slingshot, I have a separate script to detect mouse event for Mario as well so that I can disable the spring component to let Mario "fly". However, that would mean that I need another collider on Mario in order to detect the same mouse event, which will definitly be overlapping with the slingshot collider. I couldn't make both colliders capture the mouse event at once. Hence, I abandoned this idea in the end. 

Therefore, I chose to use velocity to shoot the Mario when releasing instead of relying on the momentum given by the `Spring` physics. I have a script in Slingshot gameobject that calculates the distance of current Mario position to the center of the slingshot. This distance is then multiplied by a given force value and multiplied by -1 to become the Mario's rigidbody velocity. The -1 value makes Mario to fly in the opposite direction of pulling. And it works pretty well!

For the slingshot, the strips are rendered by the `Line Rendered 2D`. The script sets 2 points for each line, and the first point is set to be at the anchor at each slingshot handle, whereas the second point will follow the mouse current position calculated in World Space. When release, the second point of each strip reset back to the original anchor position.

### Mario Animation

Some small details are designed to let Mario has a animation of running from the left and jumping onto the slingshot. I put this animator in a separate gameobject and it spawns immediately when the previous Mario gets detroyed after timeout. The animation will play for exact 2 seconds as my next Mario is set to spawned in 2 seconds. In two seconds, the Mario animation will make a "fake Mario" running from the left, jump onto the slingshot and detroy itself (gameobject) immediately, so that at the same time when the "real Mario" spawned at the slingshot location, it cannot be detected as an abrupt change by the player. It seems like the Mario just jumps onto the slingshot and the player is again able to control and shoot it out.

![](https://i.imgur.com/nL7sAFO.png)



### Level Design as Scenes

![](https://i.imgur.com/FcVqFkv.png)

The game consists of 4 levels in total. I put each level as a separate scene and uses a Game Manager object to load the next level and unload the previous level as the player progresses through levels. At any given moment, there will be two scenes in the game: a persistent UI scene that controls all the UI elements, and a level scene displaying the current level for player to play. 

![](https://i.imgur.com/ktDfVYX.png)

A coroutine is designed in the function that calls the next level, that after loading the next scene into the game for 2 seconds, it will set the level scene as the current active scene. This is to ensure that the subsequent gameobjects are instantiated in the level scene instead of the UI scene. As there will be duplicated Mario in the next level if all gameobjects are spawned in UI scene as UI scene is never unloaded.

```csharp=
private void SetLevel(int level)
{
    levelText.text = string.Format("Level {0}", level);
    if (level > 1 && level <= totalLevel)
    {
        SceneManager.UnloadSceneAsync(level - 1);
    }
    SceneManager.LoadScene(level, LoadSceneMode.Additive);
    StartCoroutine(ActivateScene(level));
}

IEnumerator ActivateScene(int level)
{
    yield return new WaitForSeconds(2);
    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(level));
}
```



### Scriptable Objects

![](https://i.imgur.com/wM1FPCY.png)


The aforementioned different types of enemies are defined by scriptable objects. Each enemy has name, move speed, health, reward points and so on. (Some attributes are not used in the end :p)

With this, it is very easy to change a type of enemy's attributes. I can adjust the speed and health of each type by just changing the values in the scriptable objects and all instances of the enemies will be updated. It is easy to create new types of enemies also.

### Health Bar
![](https://i.imgur.com/Hpox9kg.png)

Each enemy will have a health bar (i.e. slider UI component in a canvas as object) on top to display their current health level. This also gives the player a clear indication of how many more hits are required to kill this enemy so the player can strategize which enemy to hit first.

### Particle Effects

![](https://i.imgur.com/yIpFbLW.png)

As Mario or the enemy object gets detroyed, it will play a particle effect of explosion. The particle system consists of two layers, the outer system is responsible for displaying the inner explosion effect and the sub-system is responsible for displaying sparks that fly out from the explosion.

### Audio and Post Processing

Similar to Lab 1, I added post processing to make the game look more visually appealing. I also added audio to play when Mario is shot to the sky, when it hits an enemy and when explosion happens. There is background song playing and it will switch to boss song when King Koopa comes into the scene. It will also switch to gameover song or winning song respectively.