Thanks for downloading our Low Poly Shaders!

What is this?
Shaders optimized for Low Poly models. These are simple shaders which need less draw calls than the default shaders.

These shaders were made for our Low Poly assets:
https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:12124

How to use them?
Just create a new Material and assign the desired Shader.

How do they work?
Instead of doing the texture lookup's in the fragment/pixel stage they are doing it in the vertex stage. So the texture lookup is only made once for every vertex instead of once for every pixel on that object (per vertex texture lookup's).


The example models are taken from the "Low Poly Fantasy Weapons" asset:
https://www.assetstore.unity3d.com/en/#!/content/84956

If you have any suggestions or issues, please report them on GitHub: 
https://github.com/BrokenVector/LowPolyShaders


Broken Vector
support@brokenvector.net
https://www.brokenvector.net/