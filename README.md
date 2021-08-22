# Mars Protocol Bot

This is a Roleplay bot for the game : Mars Protocol.
It features an Adminitration Panel for quickly dispatching commands

Features : 
* Timed bomb
* Deadeye targeting (wip)
* Attack sound with precision depending on the weapon
* Play audio from a resource folder
* Quizz questions
* Cinematic mode with audio and images

Adding audio files for the BOT : 

* In the project : Drop the audio file in the "Resources" folder and select output to "Copy file, overwrite if newer", in the properties panel
* Or when the app is started : Simply drop the file in the "Resources" folder, the playlist should change immediately

Setup : 
1. Create a file "botToken.txt" with your discord bot token string
2. Ensure you have audio files
3. Drop ffmpeg.exe, ffplay.exe and ffprobe.exe in the project directory
4. Start the app and type !init to initialize the Discord Bot

Note : Don't forget to join a channel first, before doing any audio command