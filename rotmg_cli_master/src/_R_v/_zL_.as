// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_R_v._zL_

package _R_v{
    import com.company.assembleegameclient.game.GameSprite;
    import flash.events.Event;
    import flash.events.MouseEvent;

    public class _zL_ extends _X_i {

        public function _zL_(_arg1:GameSprite){
            super(_arg1, "Change Characters", "Change");
        }
        override protected function onButtonClick(_arg1:MouseEvent):void{
            gs_.dispatchEvent(new Event(Event.COMPLETE));
        }

    }
}//package _R_v

