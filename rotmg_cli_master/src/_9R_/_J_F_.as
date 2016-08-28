// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_9R_._J_F_

package _9R_{
    import flash.events.Event;

    public class _J_F_ extends Event {

        public static const _hx:String = "GUILDRESULTEVENT";

        public var success_:Boolean;
        public var errorText_:String;

        public function _J_F_(_arg1:Boolean, _arg2:String){
            super(_hx);
            this.success_ = _arg1;
            this.errorText_ = _arg2;
        }
        override public function toString():String{
            return (formatToString("GUILDRESULTEVENT", "success_", "errorText_"));
        }

    }
}//package _9R_

