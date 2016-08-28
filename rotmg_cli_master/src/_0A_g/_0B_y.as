// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0A_g._0B_y

package _0A_g{
    import flash.utils.ByteArray;
    import flash.utils.IDataOutput;

    public class _0B_y extends _R_q {

        public var buildVersion_:String;
        public var gameId_:int = 0;
        public var guid_:String;
        public var password_:String;
        public var secret_:String;
        public var keyTime_:int = 0;
        public var key_:ByteArray;
        public var _2B_:String;
        public var _8U_:String = "";
        public var _yt:String = "";
        public var _J_k:String = "";
        public var playPlatform:String = "";

        public function _0B_y(_arg1:uint){
            this.buildVersion_ = new String();
            this.guid_ = new String();
            this.password_ = new String();
            this.secret_ = new String();
            this.key_ = new ByteArray();
            this._2B_ = new String();
            super(_arg1);
        }
        override public function writeToOutput(_arg1:IDataOutput):void{
            _arg1.writeUTF(this.buildVersion_);
            _arg1.writeInt(this.gameId_);
            _arg1.writeUTF(this.guid_);
            _arg1.writeUTF(this.password_);
            _arg1.writeUTF(this.secret_);
            _arg1.writeInt(this.keyTime_);
            _arg1.writeShort(this.key_.length);
            _arg1.writeBytes(this.key_);
            _arg1.writeInt(this._2B_.length);
            _arg1.writeUTFBytes(this._2B_);
            _arg1.writeUTF(this._8U_);
            _arg1.writeUTF(this._yt);
            _arg1.writeUTF(this._J_k);
            _arg1.writeUTF(this.playPlatform);
        }
        override public function toString():String{
            return (formatToString("HELLO", "buildVersion_", "gameId_", "guid_", "password_", "secret_"));
        }

    }
}//package _0A_g

