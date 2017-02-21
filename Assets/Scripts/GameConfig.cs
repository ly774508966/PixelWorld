using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameConfig {
	public const bool DebugMode = false;                       //调试模式

	public const int TimerInterval = 1;
	public const int GameFrameRate = 30;                        //游戏帧频

	public const string WebUrl = "http://192.168.1.106:8080/update/AssetBundles/";      //更新资源地址

	public static string SocketAddress = string.Empty;	//Socket服务器地址
	public static int SocketPort = 0;                          	 //Socket服务器端口

	public static string UserID = string.Empty;                 //用户ID
}