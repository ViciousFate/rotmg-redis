// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_011._08K_

package _011{
    import flash.utils.IDataInput;

    public class _08K_ extends _01Q_ {

        public var result_:int;

        public function _08K_(_arg1:uint){
            super(_arg1);
        }
        override public function parseFromInput(_arg1:IDataInput):void{
            this.result_ = _arg1.readInt();
        }
        override public function toString():String{
            return (formatToString("INVRESULT", "result_"));
        }

    }
}//package _011

