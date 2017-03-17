local gameObject
local transform

PanelAlert = {}
local this = PanelAlert
this._name = 'PanelAlert'
local TAG = 'PanelAlert'

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelAlert.Awake(obj)
	print(TAG, "Awake'")
	gameObject = obj
	transform = obj.transform
end

function PanelAlert.OnDestroy()
	print(TAG, "OnDestroy'")
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

	print (TAG, "init", this.title, this.msg, this.callback)

	this.InitPanel()
end


--初始化面板--
this.bInit = false
function PanelAlert.InitPanel()
	print(TAG, 'InitPanel', this.bInit)
	this.btn_ok = transform:FindChild("Button OK").gameObject
	this.btn_cancel = transform:FindChild("Button Cancel").gameObject

	local text_title = transform:FindChild("Text Title"):GetComponent('Text')
	local text_msg = transform:FindChild("Text Msg"):GetComponent("Text")
	text_title.text = this.title
	text_msg.text = this.msg

	window = transform:GetComponent('LuaBehaviour')
	window:ClearClick()
	window:AddClick(this.btn_ok, this.OnBtnOK)
	window:AddClick(this.btn_cancel, this.OnBtnCancel)

end


-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
function PanelAlert.OnBtnOK(go)
	print('OnBtnOK')
	if this.callback then
		this.callback(1)
	end
	guiMgr:HideWindow(gameObject)
end

function PanelAlert.OnBtnCancel(go)
	print('OnBtnCancel')
	if this.callback then
		this.callback(0)
	end
	guiMgr:HideWindow(gameObject)	
end

return PanelAlert