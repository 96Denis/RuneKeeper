# ⚔️ Rune Keeper

> A 3D First-Person Action-Adventure game built with Unity URP.
> **University Project - Computer Graphics Course**

![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-black?style=flat&logo=unity)
![Language](https://img.shields.io/badge/Language-C%23-blue?style=flat&logo=csharp)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?style=flat&logo=windows)
![License](https://img.shields.io/badge/License-MIT-green)

## 📖 About The Project

**Rune Keeper** is an Open World survival game where the player takes on the role of a guardian tasked with purifying a corrupted forest. The goal is to locate and activate 4 Ancient Runes protected by stone creatures (Golems) and territorial spiders.

This project demonstrates advanced **Game Development** concepts including AI Navigation, GPU Instancing for dense environments, and Shader optimization.

---

## ✨ Key Features

### 🧠 Advanced AI Behavior
* **Territorial Spiders:** Implements a detection zone logic. If the player leaves the zone, the spider retreats. Includes a **Combo Attack System** (alternating animations) and mathematical rotation fixes (Quaternion offsets) to solve model orientation issues.
* **Chasing Golems:** Persistent enemies that use NavMesh to navigate complex terrain and avoid obstacles dynamically.

### ⚡ Optimization & Graphics
* **High Performance:** Runs at 60+ FPS on mid-range hardware with over **15,000 trees** using **LOD (Level of Detail)** and **GPU Instancing**.
* **Universal Render Pipeline (URP):** Utilizes URP for performant lighting and post-processing.
* **Hybrid NavMesh:** Uses a "Bake & Destroy" technique to allow AI navigation through dense forests without collision overhead.
* **Dynamic Day/Night Cycle:** Real-time lighting changes affecting atmosphere and shadows.

### ⚔️ Combat & Feedback
* **Melee System:** Raycast-based hit detection with stamina management.
* **Game Feel (Juice):**
    * **Screen Shake:** Cinemachine Impulse feedback on impact.
    * **Dynamic Vignette:** Pulsating red screen effect when taking damage.
    * **Damage Popups:** World-space floating text indicating damage dealt.

---

## 🎮 Controls

| Key | Action |
| :---: | :--- |
| **W, A, S, D** | Movement |
| **Shift** | Sprint (Consumes Stamina) |
| **Space** | Jump |
| **Mouse** | Look Around |
| **Left Click** | Attack |
| **E** | Interact (Activate Runes) |
| **P** | Pause Menu |

> **Debug Keys:**
> * `5`: Super Speed
> * `6`: God Mode (One Hit Kill)

---

## 🛠️ Tech Stack

* **Engine:** Unity 2022.3 LTS
* **Pipeline:** Universal Render Pipeline (URP)
* **Language:** C#
* **IDE:** Visual Studio 2022
* **Tools:** ProBuilder, Cinemachine, TextMeshPro

---

## 📸 Screenshots

*(Place your screenshots in a folder named 'Screenshots' and uncomment these lines)*

| Main Menu | Victory Screen |
|:---:|:---:|
| | |

---

## 📂 Project Structure

* `Assets/Scripts/AI`: Contains logic for Golems and Spiders (`SpiderAI.cs`, `GolemAI.cs`).
* `Assets/Scripts/Player`: Movement and Combat logic (`PlayerCombat.cs`, `PlayerStats.cs`).
* `Assets/Scripts/Managers`: Singleton managers for Game Loop and UI (`GameManager.cs`).

---

## 🤝 Credits & Assets

* **Development:** Denis-George Bîrlădeanu
* **Environment:** [Polytope Studio - Low Poly Nature Pack](https://assetstore.unity.com)
* **Animations:** [Kevin Iglesias - Basic Motions](https://assetstore.unity.com)
* **Spider Asset:** [Dante's Anvil - Fantasy Spider](https://assetstore.unity.com)

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
