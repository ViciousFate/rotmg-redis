// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_R_v._w6

package _R_v{

    import com.company.assembleegameclient.ui._gV_;
    import _E_7._c4;
    import _0D_B_._02p;
    import com.company.assembleegameclient.objects._ez;
    import flash.events.Event;
    import com.company.assembleegameclient.game.GameSprite;
    import flash.events.MouseEvent;
    import com.company.assembleegameclient.objects.Player;
    import flash.geom.ColorTransform;
    import flash.utils.getTimer;
    import com.company.util.MoreColorUtil;


    public class _w6 extends Panel {

        public var _X_T_:Vector.<_gV_>;
        private var toolTip_:_c4 = null;
        private var _0G_Y_:_02p = null;
        private var _68:Boolean = false;

        public function _w6(_arg1:GameSprite){
            this._X_T_ = new Vector.<_gV_>(_ez._tn, true);
            super(_arg1);
            this._X_T_[0] = this._00Y_(0, 0);
            this._X_T_[1] = this._00Y_(100, 0);
            this._X_T_[2] = this._00Y_(0, 32);
            this._X_T_[3] = this._00Y_(100, 32);
            this._X_T_[4] = this._00Y_(0, 64);
            this._X_T_[5] = this._00Y_(100, 64);
            addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
            addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }
        private function _00Y_(_arg1:int, _arg2:int):_gV_{
            var _local3:_gV_ = new _gV_(0xFFFFFF, false, null);
            addChild(_local3);
            _local3.x = _arg1;
            _local3.y = _arg2;
            return (_local3);
        }
        private function onAddedToStage(_arg1:Event):void{
            var _local2:_gV_;
            for each (_local2 in this._X_T_)
            {
                _local2.addEventListener(MouseEvent.MOUSE_OVER, this.onMouseOver);
                _local2.addEventListener(MouseEvent.MOUSE_OUT, this.onMouseOut);
                _local2.addEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            };
        }
        private function onRemovedFromStage(_arg1:Event):void{
            var _local2:_gV_;
            this._X_S_();
            this._7C_();
            for each (_local2 in this._X_T_)
            {
                _local2.removeEventListener(MouseEvent.MOUSE_OVER, this.onMouseOver);
                _local2.removeEventListener(MouseEvent.MOUSE_OUT, this.onMouseOut);
                _local2.removeEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            };
        }
        private function onMouseOver(_arg1:MouseEvent):void{
            this._X_S_();
            if (((!((this._0G_Y_ == null))) && (!((this._0G_Y_.parent == null)))))
            {
                return;
            };
            var _local2:_gV_ = (_arg1.target as _gV_);
            var _local3:Player = (_local2.go_ as Player);
            if ((((_local3 == null)) || ((_local3.texture_ == null))))
            {
                return;
            };
            this.toolTip_ = new _c4(_local3);
            stage.addChild(this.toolTip_);
            this._68 = true;
        }
        private function onMouseOut(_arg1:MouseEvent):void{
            this._X_S_();
            this._68 = false;
        }
        private function onMouseDown(_arg1:MouseEvent):void{
            this._X_S_();
            this._7C_();
            var _local2:_gV_ = (_arg1.target as _gV_);
            this._0G_Y_ = new _02p(gs_, (_local2.go_ as Player));
            stage.addChild(this._0G_Y_);
        }
        private function _X_S_():void{
            if (this.toolTip_ != null)
            {
                if (this.toolTip_.parent != null)
                {
                    this.toolTip_.parent.removeChild(this.toolTip_);
                };
                this.toolTip_ = null;
            };
        }
        private function _7C_():void{
            if (this._0G_Y_ != null)
            {
                if (this._0G_Y_.parent != null)
                {
                    this._0G_Y_.parent.removeChild(this._0G_Y_);
                };
                this._0G_Y_ = null;
            };
        }
        override public function draw():void{
            var _local4:_gV_;
            var _local5:Player;
            var _local6:ColorTransform;
            var _local7:Number;
            var _local8:int;
            var _local1:_ez = gs_.map_.party_;
            if (_local1 == null)
            {
                for each (_local4 in this._X_T_)
                {
                    _local4.draw(null);
                };
                return;
            };
            var _local2:int;
            var _local3:int;
            while (_local3 < _ez._tn)
            {
                if (((this._68) || (((!((this._0G_Y_ == null))) && (!((this._0G_Y_.parent == null)))))))
                {
                    _local5 = (this._X_T_[_local3].go_ as Player);
                } else
                {
                    _local5 = _local1._X_e[_local3];
                };
                if (((!((_local5 == null))) && ((_local5.map_ == null))))
                {
                    _local5 = null;
                };
                _local6 = null;
                if (_local5 != null)
                {
                    if (_local5._aY_ < (_local5._L_T_ * 0.2))
                    {
                        if (_local2 == 0)
                        {
                            _local2 = getTimer();
                        };
                        _local7 = (int((Math.abs(Math.sin((_local2 / 200))) * 10)) / 10);
                        _local8 = 128;
                        _local6 = new ColorTransform(1, 1, 1, 1, (_local7 * _local8), (-(_local7) * _local8), (-(_local7) * _local8));
                    };
                    if (!_local5.starred_)
                    {
                        if (_local6 != null)
                        {
                            _local6.concat(MoreColorUtil._3f);
                        } else
                        {
                            _local6 = MoreColorUtil._3f;
                        };
                    };
                };
                this._X_T_[_local3].draw(_local5, _local6);
                _local3++;
            };
            if (this.toolTip_ != null)
            {
                this.toolTip_.draw();
            };
        }

    }
}//package _R_v

