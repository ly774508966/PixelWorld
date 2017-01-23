# PixelWorld
GamePlay Framework with Unity. 

* UI widgets with UGUI
* MVC framework
* AssetBundle Manager & Self-Update
* pixel world generate


## Update (AssetBundle)
our version-file format are like this:
```    
    version 1.0.1
    assetbundle0 hashcode filesize
    assetbundle1 hashcode filesize
    ...

```
we will download files which has different hashcode comparing to version-file from server.
game server ref:[pyGameServer](https://github.com/AdamWu/pyGameServer)


