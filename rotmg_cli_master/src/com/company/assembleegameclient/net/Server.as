// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.net.Server

package com.company.assembleegameclient.net{
    import com.company.util._0K_N_;

    public class Server {

        public var name_:String;
        public var _09v:String;
        public var port_:int;
        public var _00r:_0K_N_;
        public var _mR_:Number;
        public var _03y:Boolean;

        public function Server(_arg1:String, _arg2:String, _arg3:int, _arg4:_0K_N_=null, _arg5:Number=0, _arg6:Boolean=false):void{
            this.name_ = _arg1;
            this._09v = _arg2;
            this.port_ = _arg3;
            this._00r = _arg4;
            this._mR_ = _arg5;
            this._03y = _arg6;
        }
        public function priority():int{
            if (this._03y)
            {
                return (2);
            };
            if (this._0J_m())
            {
                return (1);
            };
            return (0);
        }
        public function _0J_m():Boolean{
            return ((this._mR_ >= 0.66));
        }
        public function _02s():Boolean{
            return ((this._mR_ >= 1));
        }
        public function toString():String{
            return ((((((((((((("[" + this.name_) + ": ") + this._09v) + ":") + this.port_) + ":") + this._00r) + ":") + this._mR_) + ":") + this._03y) + "]"));
        }

    }
}//package com.company.assembleegameclient.net

