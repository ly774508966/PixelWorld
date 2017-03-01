--[[
	network protocol
	buildin: [0, 100)
]]

Protocol = {
	-- common
	ACK_MESSAGE 	= 100,
	ACK_NOTICE		= 101,

	-- login
	REQ_LOGIN 		= 1000,
	ACK_LOGIN 		= 1001,
	REQ_ENTER 		= 1002,
	ACK_ENTER		= 1003,

	-- bag
	REQ_SELL		= 1100,
	ACK_SELL 		= 1101,
	
}