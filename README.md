# LithodomosChallenge
My approach to solving Lithodomos' coding challenge.

The LithodomosVR Unity Coding Challenge was to put together a simple 360 Image viewing app for mobile devices, which allowed users to move their phone in real-world space to view the 360 image in a "magic window" style.
The challenge was enjoyable, offering a good deal of room to include certain intricacies and optimisations.

# Setup
The project was developed in Unity version *2018.2.0b4*.
There should be no difficulties dealing with this project. Included is a pre-built Android APK file, tested with a Google Pixel 2XL. Unfortunately I do not have access to any iOS devices so was unable to test a build there.
Users can press the left and right arrow keys when working in the Unity Editor to change images, and click and drag on the 'Game' scene window to change the viewing perspective.


# How did you decide on the approach you took?
In terms of completing the project itself, my approach was the kind I tend to take: create an extremely basic version of the application with each of its bare minimum requirements met, then iterate on there to flesh it out and improve. My technical approach was based on my experience working on similar applications in the past, and fortunatey, I had some old projects lying around with some useful parts that I was able to repurpose for this task - such as a mesh for a nicer sphere than Unity's default sphere mesh, a shader allowing mmapping of Equirectangular images to the inside of a sphere, and some other useful scripts I'd written a few months ago such as an Event handler for communication between scripts. For each factor of the challenge, I took a bit of time to consider the best approach, made an attempt at following my initial ideas, and then took a step back to reconsider and take necessary steps to improve the approach wherever necessary.

# Are there any improvements you could make to your submission?
I am unsure if my app's behaviour when the user quickly tapes on the 'next' button rapidly is exactly as outlined in the brief as the requirement was a little vague - as it stands, the app will cycle through images each time the user presses without repeating the 'fade in/fade out' animation sequence, and if the user presses left soon after pressing right (or visa versa) before the fade to black sequence completes, it will 'back out' of the sequence and remain on the original image. I am confident that the latter behaviour is as expected, but am unsure about the former.
It is possible that the project is a little 'over-engineered', especially considering its use of an event system to communicate between the "ImageChanger" script and the "ScreenFader" script, but I generally hesitate to make references to other scripts when avoidable and since I had my Event Manager script handy, I decided to use it anyway. I also could have used a Coroutine to control the fade sequences, but as I knew the fading had to be interruptable, I decided to take the Update() approach as I felt that would be easier to control interruptions with. As the saying goes, a software developer had a problem that he thought he could solve with parallelism, now two has. He problems

# What would you do differently if you were allocated more time?
Given more time, I would flesh the project out some more, add some nicer animations, and probably optimise some more of my code.


Thanks for the opportunity to partake in this challenge - it was a lot of fun.
