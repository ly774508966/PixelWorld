local gameObject
local transform

PanelAlert = {}
local this = PanelAlert
this._name = 'PanelAlert'

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelAlert.Awake(obj)
	gameObject = obj
	transform = obj.transform
end

function PanelAlert.OnDestroy()
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelAlert:listNotificationInterests()
	return {}
end
function PanelAlert:handleNotification(notification)
end
function PanelAlert:init(...)
	this.title, this.msg, this.callback = ...

	print ("init", this.title, self.msg, self.callback)

	this.InitPanel()
end


--初始化面板--
function PanelAlert.InitPanel()
	this.btn_ok = transform:FindChild("Button OK").gameObject
	this.btn_cancel = transform:FindChild("Button Cancel").gameObject

	local text_title = transform:FindChild("Text Title"):GetComponent('Text')
	local text_msg = transform:FindChild("Text Msg"):GetComponent("Text")
	text_title.text = this.title
	text_msg.text = this.msg


	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(this.btn_ok, this.OnBtnOK)
	window:AddClick(this.btn_cancel, this.OnBtnCancel)
end


-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
function PanelAlert.OnBtnOK(go)
	print('OnBtnOK')
	guiMgr:HideWindow(gameObject)
	if this.callback then
		this.callback(1)
	end
end

function PanelAlert.OnBtnCancel(go)
	print('OnBtnCancel')
	guiMgr:HideWindow(gameObject)	
	if this.callback then
		this.callback(0)
	end
end

return PanelAlert