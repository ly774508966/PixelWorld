local gameObject
local transform

PanelEquip = {}
local this = PanelEquip


-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelEquip.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
	print("Awake--->>")
end

function PanelEquip.OnDestroy()
	print("OnDestroy---->>>")
end


--初始化面板--
function PanelEquip.InitPanel()
	local btn_close = transform:FindChild("Button Close").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btn_close, this.OnBtnClose)
end


-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
--单击事件
function PanelEquip.OnBtnClose(go)
	print('OnBtnClose')
	guiMgr:HideWindow(gameObject)
end