local gameObject
local transform

PanelMenu = {}
local this = PanelMenu

--启动事件--
function PanelMenu.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
	print("Awake--->>")
end

--初始化面板--
function PanelMenu.InitPanel()
	this.btnAlert = transform:FindChild("Button Alert").gameObject

	window = transform:GetComponent('LuaBehaviour');

	window:AddClick(this.btnAlert, this.OnBtnAlert);
end

--单击事件--
function PanelMenu.OnDestroy()
	print("OnDestroy---->>>")
end

function PanelMenu.OnBtnAlert(go)
	print('OnBtnAlert')
	--guiMgr:ShowWindow('PanelAlert', go)
end