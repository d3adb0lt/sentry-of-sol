# Asset Pipeline

## Sign videos
Original vids live outside the repo (e.g. `SignVideosRaw/`) on your local machine. Only processed copies go in
`Assets/Signs/Videos/`. By processed, they must be Re-encoded from the originals.

Required format: H.264 **Baseline**, 480p, 30 fps, no audio. Other H.264 profiles cause
slow loading and timestamp warnings in Unity. Full on stall of about 3 seconds.
Below is the cmd that I used to re-encode the videos.

```powershell
ffmpeg -y -i INPUT.mp4 -c:v libx264 -profile:v baseline -level 3.1 -pix_fmt yuv420p `
  -vf "scale=-2:480" -r 30 -an -movflags +faststart OUTPUT.mp4
```

When replacing a video, overwrite the file with the **same name** so its `.meta`
(and every reference to it) is preserved. Less headache imo.