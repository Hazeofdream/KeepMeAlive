# KeepMeAlive for SPT-AKI

## Overview
KeepMeAlive adds a second-chance mechanic to SPT. Instead of immediately dying to lethal damage, your PMC can enter a critical (downed) state and recover through the revive system.

## Features
- Critical/downed state instead of immediate death in supported cases.
- Self revive flow with configurable local keybind and server-authoritative revive behavior.
- Teammate revive flow for co-op sessions.
- Configurable post-revive effects (health restore percentages, invulnerability window, optional status effects) via server config.
- Movement limits while downed, plus optional hardcore/death-mode toggles from server config.
- Server-authoritative runtime config snapshot synced to clients (with cached fallback).
- Optional local debug switches for testing and diagnosis.

## Requirements
- SPT-AKI
- Fika (required by this plugin build)

## Installation
1. Download the latest release files.
2. Extract them into your SPT root folder
3. Start the game once to generate config files.

## Quick Start
1. Carry your configured revive item (default template ID: `5c052e6986f7746b207bc3c9`).
2. Enter raid normally.
3. If you enter critical state, hold the self-revive key (default: `F`) or wait for teammate to revive you.
4. Use give-up key (default: `Backspace`) if you want to forfeit immediately.

## Configuration
Client local config (BepInEx):
- `BepInEx/config/com.KeepMeAlive.cfg`
- Contains local-only settings such as `Self Revival Key`, `Give Up Key`, and debug toggles.

Server gameplay config (authoritative source):
- `user/mods/KeepMeAlive/config.json` (server mod folder)
- Contains revive mechanics, post-revive effects, team-heal tuning, protection/hardcore toggles, and revive item template/trader price data.
- Clients fetch this snapshot at startup and use it at runtime.


## Troubleshooting
- Revive does not trigger: verify revive item ID and keybind in the config.
- Dying instantly: check whether hardcore/death settings are enabled.
- Unexpected behavior in co-op: ensure all players are on matching mod/plugin versions.

## Credits
- Developed by KaiKiNoodles
- Special thanks to the SPT-AKI development team
- Fika Co-op integration support from the Fika team

## License
This project is licensed under the MIT License - see the LICENSE file for details.

## Support
If you encounter any issues or have suggestions for improvement, please open an issue on the GitHub repository or contact me through the SPT-AKI Discord.

---

*Note: This mod is not affiliated with or endorsed by Battlestate Games. Use at your own risk in accordance with the SPT-AKI project guidelines.*