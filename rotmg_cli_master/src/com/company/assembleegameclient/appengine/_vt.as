// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.appengine._vt

package com.company.assembleegameclient.appengine{
    import flash.display.BitmapData;
    import com.company.util.AssetLibrary;
    import com.company.assembleegameclient.util.TextureRedrawer;

    public class _vt {

        public var _5U_:BitmapData;
        public var _O_k:String;
        public var _03P_:String;
        public var _qh:String;
        public var _W_e:int;

        public function _vt(_arg1:String, _arg2:String, _arg3:String, _arg4:String, _arg5:int){
            switch (_arg1)
            {
                case "fame":
                    this._5U_ = _07m();
                    break;
                case "news":
                default:
                    this._5U_ = _zK_();
            };
            this._O_k = _arg2;
            this._03P_ = _arg3;
            this._qh = _arg4;
            this._W_e = _arg5;
        }
        private static function _zK_():BitmapData{
            var _local1:BitmapData = AssetLibrary._xK_("lofiInterface2", 4);
            return (TextureRedrawer.redraw(_local1, 80, true, 0, 0));
        }
        private static function _07m():BitmapData{
            var _local1:BitmapData = AssetLibrary._xK_("lofiObj3", 224);
            return (TextureRedrawer.redraw(_local1, 80, true, 0, 0));
        }

    }
}//package com.company.assembleegameclient.appengine

