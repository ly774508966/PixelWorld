local gameObject
local transform

PanelBattle = {}
local this = PanelBattle
this._name = 'PanelBattle'

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
	return {}
end
function PanelBattle:handleNotification(notification)
end

function PanelBattle:init( ... )
	
	this.InitPanel()
end


--初始化面板--
function PanelBattle.InitPanel()
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

return PanelBattle