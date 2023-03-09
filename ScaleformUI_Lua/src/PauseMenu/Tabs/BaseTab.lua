BaseTab = setmetatable({}, BaseTab)
BaseTab.__index = BaseTab
BaseTab.__call = function()
    return "BaseTab", "BaseTab"
end

---@class BaseTab
---@field public Title string
---@field public Type number
---@field public Visible boolean
---@field public Focused boolean
---@field public Active boolean
---@field public Parent BaseTab
---@field public LeftItemList BasicTabItem[]
---@field public Activated fun(item: BaseTab)

---Creates a new BaseTab.
---@param title string
---@param type number
---@return BaseTab
function BaseTab.New(title, type)
    local data = {
        Title = title or "",
        Type = type or 0,
        Visible = false,
        Focused = false,
        Active = false,
        Parent = nil,
        LeftItemList = {},
        Activated = function(item)
        end
    }
    return setmetatable(data, BaseTab)
end
