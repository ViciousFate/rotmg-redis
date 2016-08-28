// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0K_m._en

package _0K_m{

    import com.company.assembleegameclient.objects.GameObject;
    import flash.display.BitmapData;
    import com.company.util.AssetLibrary;
    import com.company.assembleegameclient.util.TextureRedrawer;


    public class _en extends _I_b {

        private var _L_1:Vector.<_7M_>;
        private var __try:Vector.<_7M_>;
        private var _J_7:GameObject;
        private var _0E_I_:Number = 0;
        private var _ge:Number = 0;
        private var _0G_x:_D_J_;
        private var bitmapData:BitmapData;
        private var _hX_:Number;

        public function _en(_arg1:_D_J_, _arg2:GameObject){
            this._J_7 = _arg2;
            this._L_1 = new Vector.<_7M_>();
            this.__try = new Vector.<_7M_>();
            this._0G_x = _arg1;
            if (this._0G_x.bitmapFile)
            {
                this.bitmapData = AssetLibrary._xK_(this._0G_x.bitmapFile, this._0G_x.bitmapIndex);
                this.bitmapData = TextureRedrawer.redraw(this.bitmapData, this._0G_x.size, true, 0, 0);
            } else
            {
                this.bitmapData = TextureRedrawer.redrawSolidSquare(this._0G_x.color, this._0G_x.size);
            };
        }
        public static function _rU_(_arg1:_D_J_, _arg2:GameObject):_en{
            return (new (_en)(_arg1, _arg2));
        }

        override public function update(_arg1:int, _arg2:int):Boolean{
            var _local4:Number;
            var _local7:int;
            var _local8:_7M_;
            var _local9:_7M_;
            var _local3:Number = (_arg1 / 1000);
            _local4 = (_arg2 / 1000);
            if (this._J_7.map_ == null)
            {
                return (false);
            };
            x_ = this._J_7.x_;
            y_ = this._J_7.y_;
            z_ = (this._J_7.z_ + this._0G_x.zOffset);
            this._ge = (this._ge + _local4);
            var _local5:Number = (this._0G_x.rate * this._ge);
            var _local6:int = (_local5 - this._0E_I_);
            _local7 = 0;
            while (_local7 < _local6)
            {
                if (this._L_1.length)
                {
                    _local8 = this._L_1.pop();
                } else
                {
                    _local8 = new _7M_(this.bitmapData);
                };
                _local8.initialize((this._0G_x.life + (this._0G_x.lifeVariance * ((2 * Math.random()) - 1))), (this._0G_x.speed + (this._0G_x.speedVariance * ((2 * Math.random()) - 1))), (this._0G_x.speed + (this._0G_x.speedVariance * ((2 * Math.random()) - 1))), (this._0G_x.rise + (this._0G_x.riseVariance * ((2 * Math.random()) - 1))), z_);
                map_.addObj(_local8, (x_ + (this._0G_x.rangeX * ((2 * Math.random()) - 1))), (y_ + (this._0G_x.rangeY * ((2 * Math.random()) - 1))));
                this.__try.push(_local8);
                _local7++;
            };
            this._0E_I_ = (this._0E_I_ + _local6);
            _local7 = 0;
            while (_local7 < this.__try.length)
            {
                _local9 = this.__try[_local7];
                _local9._G_n = (_local9._G_n - _local4);
                if (_local9._G_n <= 0)
                {
                    this.__try.splice(_local7, 1);
                    map_.removeObj(_local9.objectId_);
                    _local7--;
                    this._L_1.push(_local9);
                } else
                {
                    _local9._0A_7 = (_local9._0A_7 + (this._0G_x.riseAcc * _local4));
                    _local9.x_ = (_local9.x_ + (_local9._null_ * _local4));
                    _local9.y_ = (_local9.y_ + (_local9._0K_5 * _local4));
                    _local9.z_ = (_local9.z_ + (_local9._0A_7 * _local4));
                };
                _local7++;
            };
            return (true);
        }
        override public function removeFromMap():void{
            var _local1:_7M_;
            for each (_local1 in this.__try)
            {
                map_.removeObj(_local1.objectId_);
            };
            this.__try = null;
            this._L_1 = null;
            super.removeFromMap();
        }

    }
}//package _0K_m

