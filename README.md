UnityTouchSmoothing
===================

A unity implementation of [Exponential Smoothing](http://en.wikipedia.org/wiki/Exponential_smoothing) and the [One Euro Filter](http://www.lifl.fr/~casiez/1euro/) in CSharp.

How To Use:
-------------------
1. Add the script to a Game Object.
2. Set the public variables.
3. For Exponential Smoothing, set Alpha to 1 and decrease until jitter is reasonable. Alpha closer to 0 increases lag.
4. For One Euro Filter, set Jitter Reduction to 1 and Lag Reduction to 0. Increase the values to reduce jitter and lag.
5. When implementing, call OneEuroFilter and use filteredPosition instead of Input.GetTouch(0).position.

Comments:
-------------------
I will work on making a struct so that it takes all touches from Input.touches and has filteredPosition for each touch.
Any suggestions can be emailed to me at [slek120@gmail.com](mailto:slek120@gmail.com?Subject=Unity%20Touch%20Smoothing)
