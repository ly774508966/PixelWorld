using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;

public class TestLua : MonoBehaviour 
{
	LuaState lua = null; 

	void Start ()  {        
		lua = new LuaState();         
		lua.Start();    
        	LuaBinder.Bind(lua);    

		// lua path
		string fullPath = Path.Combine(Application.dataPath, "Lua");
		lua.AddSearchPath(fullPath);    

		// execute lua file
		lua.DoFile("testlua.lua"); 
	}

	void Update()
	{
		lua.CheckTop();
		lua.Collect();        
	}

	void OnApplicationQuit()
	{
		lua.Dispose();
		lua = null;
	}
}
