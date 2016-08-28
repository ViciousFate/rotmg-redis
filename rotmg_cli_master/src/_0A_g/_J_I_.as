// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0A_g._J_I_

package _0A_g{
    import flash.utils.IDataOutput;

    public class _J_I_ extends _R_q {

        public var time_:int;
        public var bulletId_:uint;
        public var targetId_:int;
        public var kill_:Boolean;

        public function _J_I_(_arg1:uint){
            super(_arg1);
        }
        override public function writeToOutput(_arg1:IDataOutput):void{
            _arg1.writeInt(this.time_);
            _arg1.writeByte(this.bulletId_);
            _arg1.writeInt(this.targetId_);
            _arg1.writeBoolean(this.kill_);
        }
        override public function toString():String{
            return (formatToString("ENEMYHIT", "time_", "bulletId_", "targetId_", "kill_"));
        }

    }
}//package _0A_g

