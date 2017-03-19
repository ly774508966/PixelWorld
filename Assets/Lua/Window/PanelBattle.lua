local gameObject
local transform

PanelBattle = {}
local this = PanelBattle
this._name = 'PanelBattle'
local TAG = 'PanelBattle'

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelBattle.Awake(obj)
	gameObject = obj
	transform = obj.transform
end

function PanelBattle.OnDestroy()
	gameObject = nil
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelBattle:listNotificationInterests()
	return {BATTLE_HP_CHANGE, BATTLE_MP_CHANGE}
end
function PanelBattle:handleNotification(notification)
	if gameObject == nil then return end

	local name = notification._name
	if name == BATTLE_HP_CHANGE or name == BATTLE_HP_CHANGE then
		this.RefreshAttrs(notification._body)
	end

end

function PanelBattle:init( ... )
	this.InitPanel()
end


--初始化面板--
function PanelBattle.InitPanel()

	this.bar_hp = transform:Find('bar hp'):GetComponent('Image')
	this.bar_mp = transform:Find('bar mp'):GetComponent('Image')
	this.bar_hp.fillAmount = 1
	this.bar_mp.fillAmount = 1


	local btn_exit = transform:Find("Button Exit").gameObject

	window = transform:GetComponent('LuaBehaviour')
	window:AddClick(btn_exit, this.OnBtnExit)
end

function PanelBattle.OnBtnExit(go)
	print('OnBtnExit')
    local data = {lanMgr:GetValue('TITLE_TIP'), lanMgr:GetValue('BATTLE_EXIT'), 
    	function (ret) 
    		if ret == 1 then
    			-- exit
            	sceneMgr:GotoScene(SceneID.Main)
    		end 
    	end
		}
    facade:sendNotification(OPEN_WINDOW, {name="PanelAlert", data=data})
end

function PanelBattle.RefreshAttrs(data)
	print(TAG, "RefreshAttrs")
	if data.hp then
		this.bar_hp.fillAmount = data.hp / 100
	end 
end

return PanelBattle