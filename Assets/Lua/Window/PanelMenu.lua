local gameObject
local transform

PanelMenu = {}
local this = PanelMenu

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelMenu.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
	print("Awake--->>")
end
function PanelMenu.OnDestroy()
	print("OnDestroy---->>>")
end

--初始化面板--
function PanelMenu.InitPanel()
	local btnAlert = transform:FindChild("Button Alert").gameObject
	local btnBag = transform:FindChild("Button Bag").gameObject
	local btnEquip = transform:FindChild("Button Equip").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btnAlert, this.OnBtnAlert)
	window:AddClick(btnBag, this.OnBtnBag)
	window:AddClick(btnEquip, this.OnBtnEquip)
end

--单击事件--

function PanelMenu.OnBtnAlert(go)
	print('OnBtnAlert')
	PanelAlert.setTitleMsg(lanMgr:GetValue('TITLE_TIP'), lanMgr:GetValue('NETWORK_TIMEOUT'))
	guiMgr:ShowWindow('PanelAlert', go)
end

function PanelMenu.OnBtnBag(go)
	print('OnBtnAlert')
	guiMgr:ShowWindow('PanelBag', go)
end

function PanelMenu.OnBtnEquip(go)
	print('OnBtnEquip')
	guiMgr:ShowWindow('PanelEquip', go)
end