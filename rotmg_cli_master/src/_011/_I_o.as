// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_011._I_o

package _011{
    import flash.utils.IDataInput;

    public class _I_o extends _01Q_ {

        public var bulletId_:uint;
        public var ownerId_:int;
        public var containerType_:int;
        public var angle_:Number;

        public function _I_o(_arg1:uint){
            super(_arg1);
        }
        override public function parseFromInput(_arg1:IDataInput):void{
            this.bulletId_ = _arg1.readUnsignedByte();
            this.ownerId_ = _arg1.readInt();
            this.containerType_ = _arg1.readShort();
            this.angle_ = _arg1.readFloat();
        }
        override public function toString():String{
            return (formatToString("ALLYSHOOT", "bulletId_", "ownerId_", "containerType_", "angle_"));
        }

    }
}//package _011

