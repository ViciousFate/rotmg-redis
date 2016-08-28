// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0K_m._7M_

package _0K_m{
    import com.company.assembleegameclient.objects.BasicObject;

    import flash.geom.Matrix;
    import flash.display.GraphicsPath;
    import flash.display.GraphicsBitmapFill;
    import com.company.util.GraphicHelper;
    import flash.display.BitmapData;
    import flash.display.IGraphicsData;
    import com.company.assembleegameclient.map._0D_v;


    public class _7M_ extends BasicObject {

        public var _G_n:Number = 0;
        public var _null_:Number;
        public var _0K_5:Number;
        public var _0A_7:Number;
        protected var vS_:Vector.<Number>;
        protected var _01i:Matrix;
        protected var path_:GraphicsPath;
        protected var bitmapFill_:GraphicsBitmapFill;

        public function _7M_(_arg1:BitmapData){
            this.vS_ = new Vector.<Number>(8);
            this._01i = new Matrix();
            this.path_ = new GraphicsPath(GraphicHelper._H_2, null);
            this.bitmapFill_ = new GraphicsBitmapFill(null, null, false, false);
            super();
            this.bitmapFill_.bitmapData = _arg1;
            objectId_ = _7y();
        }
        public function initialize(_arg1:Number, _arg2:Number, _arg3:Number, _arg4:Number, _arg5:int):void{
            this._G_n = _arg1;
            this._null_ = _arg2;
            this._0K_5 = _arg3;
            this._0A_7 = _arg4;
            z_ = _arg5;
        }
        override public function draw(_arg1:Vector.<IGraphicsData>, _arg2:_0D_v, _arg3:int):void{
            var _local4:Number = (this.bitmapFill_.bitmapData.width / 2);
            var _local5:Number = (this.bitmapFill_.bitmapData.height / 2);
            this.vS_[6] = (this.vS_[0] = (_bY_[3] - _local4));
            this.vS_[3] = (this.vS_[1] = (_bY_[4] - _local5));
            this.vS_[4] = (this.vS_[2] = (_bY_[3] + _local4));
            this.vS_[7] = (this.vS_[5] = (_bY_[4] + _local5));
            this.path_.data = this.vS_;
            this._01i.identity();
            this._01i.translate(this.vS_[0], this.vS_[1]);
            this.bitmapFill_.matrix = this._01i;
            _arg1.push(this.bitmapFill_);
            _arg1.push(this.path_);
            _arg1.push(GraphicHelper.END_FILL);
        }
        override public function removeFromMap():void{
            map_ = null;
            _0H_B_ = null;
        }

    }
}//package _0K_m

