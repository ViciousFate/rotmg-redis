// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0A_g.Create

package _0A_g{
    import flash.utils.IDataOutput;

    public class Create extends _R_q {

        public var objectType_:int;

        public function Create(_arg1:uint){
            super(_arg1);
        }
        override public function writeToOutput(_arg1:IDataOutput):void{
            _arg1.writeShort(this.objectType_);
        }
        override public function toString():String{
            return (formatToString("CREATE", "objectType_"));
        }

    }
}//package _0A_g

