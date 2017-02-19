local gameObject
local transform

PanelItemDetail = {}
local this = PanelItemDetail


--启动事件--
function PanelItemDetail.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
	print("Awake--->>")
end


--初始化面板--
function PanelItemDetail.InitPanel()
	local btn_close = transform:FindChild("Button Close").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btn_close, this.OnBtnClose)
end


--单击事件--
function PanelItemDetail.OnDestroy()
	print("OnDestroy---->>>")
end

function PanelItemDetail.OnBtnClose(go)
	print('OnBtnClose')
	guiMgr:HideWindow(gameObject)
end