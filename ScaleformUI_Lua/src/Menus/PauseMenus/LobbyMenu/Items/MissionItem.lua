MissionItem = setmetatable({}, MissionItem)
MissionItem.__index = MissionItem
MissionItem.__call = function()
    return "LobbyItem", "MissionItem"
end

---@class MissionItem
---@field Label string
---@field Handle number
---@field ParentColumn MissionListColumn
---@field enabled boolean
---@field MainColor SColor
---@field HighlightColor SColor
---@field LeftIcon BadgeStyle
---@field LeftIconColor SColor
---@field RightIcon BadgeStyle
---@field RightIconColor SColor
---@field RightIconChecked boolean
---@field _Selected boolean
---@field hovered boolean
---@field Activated fun(item:MissionItem)
---@field Hovered fun(val:boolean)
---@field Enabled fun(bool:boolean)
---@field SetLeftIcon fun(icon:BadgeStyle, color:SColor)
---@field SetRightIcon fun(icon:BadgeStyle, color:SColor, checked:boolean)


---@param label string
---@param mainColor SColor
---@param highlightColor SColor
---@return table
function MissionItem.New(label, mainColor, highlightColor)
    local _data = {
        type = 0,
        Label = label,
        Handle = nil,
        ParentColumn = nil,
        enabled = true,
        MainColor = mainColor or SColor.HUD_Pause_bg,
        HighlightColor = highlightColor or SColor.HUD_White,
        LeftIcon = BadgeStyle.NONE,
        LeftIconColor = SColor.HUD_White,
        RightIcon = BadgeStyle.NONE,
        RightIconColor = SColor.HUD_White,
        RightIconChecked = false,
        _Selected = false,
        hovered = false,
        Activated = function(item)
        end
    }
    return setmetatable(_data, MissionItem)
end

function MissionItem:Hovered(val)
    if val == nil then
        return self.hovered
    else
        self.hovered = val
    end
end

function MissionItem:Enabled(bool)
    if bool == nil then
        return self.enabled
    else
        self.enabled = bool
        if self.ParentColumn ~= nil and self.ParentColumn.Parent ~= nil and self.ParentColumn.Parent:Visible() then
            local idx = self.ParentColumn.Pagination:GetScaleformIndex(IndexOf(self.ParentColumn.Items, self))
            local pSubT = self.ParentColumn.Parent()
            if pSubT == "LobbyMenu" then
                ScaleformUI.Scaleforms._pauseMenu._lobby:CallFunction("SET_MISSION_ITEM_ENABLED", idx, bool)
            elseif pSubT == "PauseMenu" and self.ParentColumn.ParentTab.Visible then
                ScaleformUI.Scaleforms._pauseMenu._pause:CallFunction("SET_PLAYERS_TAB_MISSION_ITEM_ENABLED", idx, bool)
            end
        end
    end
end

function MissionItem:SetLeftIcon(icon, color)
    self.LeftIcon = icon
    self.LeftIconColor = color
    if self.ParentColumn ~= nil and self.ParentColumn.Parent ~= nil and self.ParentColumn.Parent:Visible() then
        local idx = self.ParentColumn.Pagination:GetScaleformIndex(IndexOf(self.ParentColumn.Items, self))
        local pSubT = self.ParentColumn.Parent()
        if pSubT == "LobbyMenu" then
            ScaleformUI.Scaleforms._pauseMenu._lobby:CallFunction("SET_MISSION_ITEM_LEFT_ICON", idx, icon, color)
        elseif pSubT == "PauseMenu" and self.ParentColumn.ParentTab.Visible then
            ScaleformUI.Scaleforms._pauseMenu._pause:CallFunction("SET_PLAYERS_TAB_MISSION_ITEM_LEFT_ICON", idx, icon, color)
        end
    end
end

function MissionItem:SetRightIcon(icon, color, checked)
    self.RightIcon = icon
    self.RightIconColor = color
    self.RightIconChecked = checked or false
    if self.ParentColumn ~= nil and self.ParentColumn.Parent ~= nil and self.ParentColumn.Parent:Visible() then
        local idx = self.ParentColumn.Pagination:GetScaleformIndex(IndexOf(self.ParentColumn.Items, self))
        local pSubT = self.ParentColumn.Parent()
        if pSubT == "LobbyMenu" then
            ScaleformUI.Scaleforms._pauseMenu._lobby:CallFunction("SET_MISSION_ITEM_RIGHT_ICON", idx, icon, checked, color)
        elseif pSubT == "PauseMenu" and self.ParentColumn.ParentTab.Visible then
            ScaleformUI.Scaleforms._pauseMenu._pause:CallFunction("SET_PLAYERS_TAB_MISSION_ITEM_RIGHT_ICON", idx, icon, checked or false, color)
        end
    end
end

function MissionItem:Selected(bool)
    if bool == nil then
        return self._Selected
    else
        self._Selected = bool
    end
end
