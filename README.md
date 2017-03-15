# PixelWorld

## Introduction
GamePlay is a simple framework for game develop with Unity3D. It is written in c# and lua. It includes many usefull modules for game development, such as mvc, game managers, Assetbundle&lua update.
some plugins using in this project：

* UGUI
* [tolua (ulua)](https://github.com/topameng/tolua) 
* [LitJson](https://lbv.github.io/litjson/) 
* [DOTween](http://dotween.demigiant.com/) 
* [Behavior Designer](https://www.assetstore.unity3d.com/en/#!/content/15277) 


## Features
* UI widgets with UGUI
* Game Managers
* Network
* MVC framework
* AssetBundle Manager 
* Self-Update (assetbundle&lua)
* AI
* pixel world generate (todo)

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

## Blog
[AdamWu's Blog](http://blog.csdn.net/adamwu1988)