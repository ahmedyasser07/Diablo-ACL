# Diablo - Unity Game Development Project


Welcome to the **Diablo** project repository! This game is inspired by *Diablo IV* and is a student-driven isometric action role-playing game (RPG). The goal of the project is to create a thrilling and immersive experience featuring character progression, intense combat, and strategic gameplay. Below are the detailed features and development guidelines for this exciting project.

---

## 🎮 **Game Overview**
The game takes players on an adventurous journey through the Sanctuary, where they battle evil forces, gain XP, level up, and unlock powerful abilities. The ultimate challenge lies in collecting Rune Fragments to confront and defeat the formidable Boss, Lilith.

---

## 🧙 **Player Mechanics**
- **Perspective:** Overhead tilted perspective with a dynamic camera that follows the player (Wanderer).
- **Character Classes:** 
  - **Barbarian**: Strength-focused melee warrior.  
  - **Sorcerer**: Magic-wielding spellcaster.  
  - **Rogue**: Agile fighter (available for teams of 7+ members).  
- **Unique Abilities:** Each class comes with distinct skills and attributes to suit diverse playstyles.

---

## 🏹 **Character Leveling and Abilities**
- Start at **Level 1** and gain XP by defeating enemies.
- Leveling up rewards:
  - Increased maximum HP.
  - Ability points to unlock special abilities.
  - Full HP refill.

---

## 👹 **Enemies**
1. **Minions**: Low health, moderate XP rewards.  
2. **Demons**: Higher health, greater XP rewards.  
3. **Boss (Lilith)**:  
   - Two phases with varying health and attack patterns.  
   - The ultimate challenge for players.

---

## 🌍 **Levels**
- **Main Level:** Open space with scattered enemy camps containing Rune Fragments.  
- **Boss Level:** Arena-style battle with Lilith after collecting all Rune Fragments.

---

## 🗺️ **Optional Mini-map** (Teams of 8+)
Displays key elements:
- Wanderer’s location.  
- Enemy camps, potions, enemies, and Rune Fragments.

---

## 🎮 **Controls**
- **Movement and Abilities:** Mouse clicks.  
- **Special Actions:**  
  - Healing potions.  
  - Pausing the game.  
- **Custom Keybinds:** For activating unique abilities.

---

## 🖥️ **Heads-Up Display (HUD)**
Displays vital information, including:
- Player HP, XP, level, ability points, healing potions, Rune Fragments, and enemy health.

---

## 📺 **Screens**
1. **Main Menu:** Start, options, and exit.  
2. **Character Class Screen:** Choose your class.  
3. **Pause Screen:** Pause and resume gameplay.  
4. **Game Over Screen:** Retry or exit.

---

## 🎨 **Graphics, Animations, and Audio**
- **Models and Effects:** Detailed designs for the Wanderer and enemies.  
- **Animations:** Smooth movements and attack sequences.  
- **Audio:** Engaging sound effects and background music.

---

## 🛠️ **Cheat Codes**
Mandatory for testing purposes:
- Modify health, enable invincibility, slow motion, cooldown resets, ability unlocking, and instant XP gain.

---

## 📋 **Team Guidelines and Submission**
- **Teamwork:** Collaborate effectively and credit all external assets appropriately.  
- **Submission:** Follow naming conventions and submission procedures.  
- **Originality:** Ensure all work is unique and adheres to academic integrity.

---

## 🗂️ **Repository Structure**
```plaintext
├── Assets/
│   ├── Scripts/          # Game logic and mechanics
│   ├── Models/           # 3D models and assets
│   ├── Animations/       # Player and enemy animations
│   ├── Audio/            # Sound effects and background music
│   ├── Scenes/           # Game levels and UI screens
│   └── Prefabs/          # Reusable game objects
├── README.md             # Project documentation
└── ProjectSettings/      # Unity project settings
```

---

## 📢 **How to Contribute**
1. Clone the repository:  
   ```bash
   git clone https://github.com/yourusername/diablo-unity-game.git
   ```
2. Create a feature branch:  
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes and push:  
   ```bash
   git commit -m "Add feature-name"
   git push origin feature-name
   ```
4. Open a pull request for review.

