// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_R_v._sc

package _R_v{

    import com.company.assembleegameclient.ui.Slot;
    import com.company.assembleegameclient.ui.Inventory;
    import com.company.assembleegameclient.game.GameSprite;
    import com.company.assembleegameclient.objects.Container;


    public class _sc extends Panel {

        private static const _hP_:Vector.<int> = new <int>[Slot.__get, Slot.__get, Slot.__get, Slot.__get, Slot.__get, Slot.__get, Slot.__get, Slot.__get];

        public var _e9:Inventory;

        public function _sc(_arg1:GameSprite, _arg2:Container){
            super(_arg1);
            this._e9 = new Inventory(gs_, _arg2, _arg2._include(), _hP_, 8, false);
            this._e9.x = 8;
            addChild(this._e9);
        }
        override public function draw():void{
            this._e9.draw(this._e9._iA_._zq);
        }

    }
}//package _R_v

