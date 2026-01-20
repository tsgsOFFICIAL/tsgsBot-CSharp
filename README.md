# tsgsBot

**tsgsBot** is a versatile, multi-purpose Discord bot built for the tsgsOFFICIAL community (and beyond).  
It offers fun utilities, moderation tools, giveaways, polls, CS2 stats, custom software support, games, and more - all while running reliably on a Raspberry Pi.

[![Discord](https://img.shields.io/discord/227048721710317569?color=5865F2&label=Discord&logo=discord&logoColor=white)](https://discord.gg/Cddu5aJ)
[![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet?logo=dotnet)](https://dotnet.microsoft.com/download)
[![Discord.Net](https://img.shields.io/badge/Discord.Net-3.x-blue?logo=discord)](https://github.com/discord-net/Discord.Net)

## Features

- **Slash commands** for clean, modern interaction
- **Persistent giveaways & polls** - stored in a database (survive restarts)
- Most non-user-specific commands work in **DMs** and **servers**
- Moderation tools (mute, purge, role management, nick changes…)
- Fun & utility commands (memes, random number, Tic-Tac-Toe, reminders…)
- CS2 stats lookup, random memes from Reddit or #liams-memes
- Support & verification for tsgsOFFICIAL software/products
- Easy status & presence management (admin only)
- Runs as a **systemd service** on Raspberry Pi for high uptime

## Tech Stack

- Language: **C#**
- Framework: **.NET 10**
- Discord library: **[Discord.Net](https://github.com/discord-net/Discord.Net)**
- Database: Used for persistent giveaways & polls (implementation not exposed in commands)
- Hosting: Raspberry Pi (running as systemd service named `tsgsbot`)

## Commands

All commands are **slash commands** (`/`). Type `/` in Discord to see available commands.

Most commands require only `UseApplicationCommands` permission - some need higher perms (shown below).

### General / Utility

- `/ping` → Check bot latency
- `/uptime` → Bot runtime
- `/help` → List commands you can use
- `/invite` → Get bot invite link
- `/serverinfo` → Server information
- `/userinfo [user]` → User details
- `/randomnumber [min] [max]` → Random number generator
- `/remind [task] [duration]` → Set a reminder
- `/report [user] [reason]` → Report user to moderators

### Fun & Games

- `/tictactoe [opponent]` → Challenge to Tic-Tac-Toe
- `/meme [subreddit]` → Random meme from Reddit
- `/liam` → Random meme from sacred #liams-memes channel

### Moderation & Management

- `/mute [target] [duration] [reason]` (MuteMembers)
- `/unmute [target]` (MuteMembers)
- `/nick [user] [nickname]` (ManageNicknames)
- `/mynick [nickname]` → Change your own nickname
- `/role-add [user] [role]` (ManageRoles)
- `/role-remove [user] [role]` (ManageRoles)
- `/purge [amount] [user]` (ManageChannels) → Delete messages / nuke channel
- `/say [message] [channel]` (ManageMessages) → Make bot speak
- `/status [type] [message]` (Administrator) → Change bot status

### CS2 & Software Related

- `/csstats [identifier]` → CS:GO/CS2 player stats
- `/autoaccept` → Directions for AutoAccept (CS2 tool)
- `/streamdropcollector` → StreamDropCollector install guide
- `/sdc` → Short version of StreamDropCollector guide
- `/software` → Links to tsgsOFFICIAL software downloads
- `/support` → Submit support request for applications
- `/verify [code]` → Verify donation → get supporter role

### Giveaways & Polls

- `/giveaway` (CreateEvents) → Start reacting giveaway
- `/poll` (SendPolls) → Create reacting poll

### Admin Only

- `/dm [user] [message]` (Administrator)

## Running & Managing the Bot (Raspberry Pi)

tsgsBot runs as a systemd service named `tsgsbot` on a Raspberry Pi.

All commands assume you're SSH'd in as user `tsgsofficial`.

### Bot Management Cheat Sheet (Raspberry Pi)

All commands assume you're SSH'd in as your user (`tsgsofficial`).

#### 1. Check bot status (most used)

```bash
sudo systemctl status tsgsbot
```

→ Shows if running, uptime, last log lines. Use this first when something feels off.

#### 2. View live bot logs (see what the bot is saying)

```bash
journalctl -u tsgsbot -f
```

→ `-f` = follow/live tail.  
Press `Ctrl+C` to stop watching.

#### 3. View last N lines of logs (good for quick check)

```bash
journalctl -u tsgsbot -n 50
```

→ `-n 50` = last 50 lines. Change number as needed.

#### 4. Restart the bot (after code change or crash)

```bash
sudo systemctl restart tsgsbot
```

→ Stops + starts again. Takes ~5-10 seconds.

#### 5. Stop the bot (turn off completely)

```bash
sudo systemctl stop tsgsbot
```

#### 6. Start the bot (if you stopped it)

```bash
sudo systemctl start tsgsbot
```

#### 7. Manually run the update script (pull code + rebuild + restart)

```bash
~/update-bot.sh
```

→ This fetches latest from GitHub, rebuilds, and restarts the service automatically.

#### 8. Reboot the entire Pi (tests auto-start on boot)

```bash
sudo reboot
```

→ After reboot, wait ~1 min, then check status again.

#### 9. Quick check if bot exe exists & is executable

```bash
ls -l ~/tsgsBot/publish/tsgsBot-CSharp
```

→ Should show `-rwxr-xr-x` (the `x` means executable).

#### 10. Update system packages (weekly-ish)

```bash
sudo apt update && sudo apt upgrade -y && sudo apt autoremove -y
```

### Bash aliases

1. Open your bash profile:

    ```bash
    nano ~/.bashrc
    ```

2. Scroll to the bottom and add these lines:

    ```bash
    # Bot aliases
    alias bot-status='sudo systemctl status tsgsbot'
    alias bot-logs='journalctl -u tsgsbot -f'
    alias bot-restart='sudo systemctl restart tsgsbot'
    alias bot-start='sudo systemctl start tsgsbot'
    alias bot-stop='sudo systemctl stop tsgsbot'
    alias bot-update='~/update-bot.sh'
    alias bot-rebuild='~/update-bot.sh && bot-restart'
    ```

3. Save/exit (Ctrl+O → Enter → Ctrl+X)

4. Apply the changes:
    ```bash
    source ~/.bashrc
    ```
