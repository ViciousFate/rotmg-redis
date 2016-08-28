// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_F_1.CharsAndNews

package _F_1{
    import flash.display.Sprite;
    import com.company.assembleegameclient.screens.charrects.CharacterRectList;
    import flash.display.Shape;
    import flash.display.Graphics;
    import com.company.assembleegameclient.appengine._0K_R_;

    public class CharsAndNews extends Sprite {

        public static const WIDTH:int = 760;
        public static const HEIGHT:int = 430;

        public var charRectList_:CharacterRectList;
        public var _tZ_:News;

        public function CharsAndNews(_arg1:_0K_R_, _arg2:_05p){
            var _local3:Shape;
            var _local4:Graphics;
            super();
            this.charRectList_ = new CharacterRectList(_arg1, _arg2);
            addChild(this.charRectList_);
            this._tZ_ = new News(_arg1);
            this._tZ_.x = 320;
            addChild(this._tZ_);
            if (height > 400)
            {
                _local3 = new Shape();
                _local4 = _local3.graphics;
                _local4.beginFill(0);
                _local4.drawRect(0, 0, WIDTH, HEIGHT);
                _local4.endFill();
                addChild(_local3);
                mask = _local3;
            };
        }
        public function _0D__(_arg1:Number):void{
            this.charRectList_.y = _arg1;
            this._tZ_.y = _arg1;
        }

    }
}//package _F_1

