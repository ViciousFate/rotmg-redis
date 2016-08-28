// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.ui.Inventory

package com.company.assembleegameclient.ui{
    import flash.display.Sprite;
    import com.company.assembleegameclient.game.GameSprite;
    import com.company.assembleegameclient.objects.GameObject;



    public class Inventory extends Sprite {

        private static const _pm:Array = [0, 0, 0, 0];
        private static const _ls:Object = {
            "4":[[1, 0, 0, 1], _pm, _pm, [0, 1, 1, 0]],
            "8":[[1, 0, 0, 0], _pm, _pm, [0, 1, 0, 0], [0, 0, 0, 1], _pm, _pm, [0, 0, 1, 0]],
            "12":[[1, 0, 0, 1], _pm, _pm, [0, 1, 1, 0], [1, 0, 0, 0], _pm, _pm, [0, 1, 0, 0], [0, 0, 0, 1], _pm, _pm, [0, 0, 1, 0]]
        };

        public var gs_:GameSprite;
        public var _iA_:GameObject;
        public var _A_H_:String;
        public var slots_:Vector.<_E_6>;

        public function Inventory(_arg1:GameSprite, _arg2:GameObject, _arg3:String, _arg4:Vector.<int>, _arg5:int, _arg6:Boolean):void{
            var _local8:_E_6;
            this.slots_ = new Vector.<_E_6>();
            super();
            this.gs_ = _arg1;
            this._iA_ = _arg2;
            this._A_H_ = _arg3;
            if (((_arg1) && ((_arg2 == this.gs_.map_.player_))))
            {
                this.gs_.map_.player_.inventory = this;
            };
            var _local7:int;
            while (_local7 < _arg5)
            {
                _local8 = new _E_6(this, _local7, _arg4[_local7], ((_arg6) ? (_local7 - 3) : -1), _ls[_arg4.length][_local7]);
                _local8.x = (int((_local7 % 4)) * (Slot.WIDTH + 4));
                _local8.y = (int((_local7 / 4)) * (Slot.HEIGHT + 4));
                this.slots_.push(_local8);
                addChild(_local8);
                _local7++;
            };
        }
        public function draw(_arg1:Vector.<int>):void{
            var _local3:_E_6;
            var _local2:int;
            while (_local2 < this.slots_.length)
            {
                _local3 = this.slots_[_local2];
                _local3.draw(((((!((_arg1 == null))) && ((_local2 < _arg1.length)))) ? _arg1[_local2] : -1));
                _local2++;
            };
        }
        public function refresh():void{
            var _local1:_E_6;
            for each (_local1 in this.slots_)
            {
                _local1.refresh();
            };
        }
        public function useItem(_arg1:int):void{
            if ((((_arg1 < 0)) || ((_arg1 >= this.slots_.length))))
            {
                return;
            };
            this.slots_[_arg1].attemptUse();
        }
        public function _mK_():Boolean{
            var _local3:_E_6;
            if (!this.gs_)
            {
                return (false);
            };
            var _local1:Boolean = true;
            var _local2:uint = 4;
            while (_local2 < 12)
            {
                _local3 = this.gs_.map_.player_.inventory.slots_[_local2];
                if (_local3._X_B_ == null)
                {
                    _local1 = false;
                };
                _local2++;
            };
            return (_local1);
        }

    }
}//package com.company.assembleegameclient.ui

